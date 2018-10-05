using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;

namespace DiscordBot.Commands
{
    public class WhoOfUsCommands
    {
        private readonly TimeSpan _responseTime = TimeSpan.FromSeconds(30);

        [Command("qdn")]
        public async Task WhoOfUs(CommandContext ctx, string title, params string[] options)
        {
            var users = ctx.Guild.Members.Where(_ => _.PermissionsIn(ctx.Channel) == Permissions.AccessChannels);
            var responseDictionary = new Dictionary<DiscordEmoji, string>();

            for (int i = 0; i < options.Length; i++)
            {
                responseDictionary.Add(DiscordEmoji.FromName(ctx.Client, $":{Tools.Tools.IntToEmoji(i + 1)}:"), 
                    options[i]);
            }
            
            var baseTitle = "[Qdn]" + title;
            var embed = RandomCommands.CreateEmbed(baseTitle, false, options);

            var messages = new Dictionary<DiscordUser, DiscordMessage>();

            foreach (var user in users)
            {
                var dmChannel = await user.CreateDmChannelAsync();
                var message = await dmChannel.SendMessageAsync(embed: embed);
                messages.Add(user, message);
                await RandomCommands.CreatePollReaction(ctx, message, options.Length + 1);
            }

            ctx.Client.DebugLogger.LogMessage(LogLevel.Info, "NafBot", $"Start collecting Reaction", DateTime.Now);

            var tasks = new List<Task<ReactionCollectionContext>>();
            var interactivity = ctx.Client.GetInteractivityModule();

            foreach (var message in messages)
            {
                var task = Task.Run(() => interactivity.CollectReactionsAsync(message.Value, _responseTime));
                tasks.Add(task);
            }

            var results = await Task.WhenAll(tasks);

            ctx.Client.DebugLogger.LogMessage(LogLevel.Info, "NafBot", $"Finish collecting Reaction", DateTime.Now);

            var dicoResults = new Dictionary<string, int>();
            foreach (var result in results)
            {
                foreach (var resultReaction in result.Reactions)
                {
                    var option = responseDictionary[resultReaction.Key];

                    if (option == null)
                        continue;

                    if (dicoResults.ContainsKey(option))
                    {
                        dicoResults[option] = dicoResults[option] + resultReaction.Value;
                    }
                    else
                    {
                        dicoResults.Add(option, resultReaction.Value);
                    }
                }
            }

            
        }
            
    }
}