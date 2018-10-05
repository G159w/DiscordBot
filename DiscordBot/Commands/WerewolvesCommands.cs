using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DiscordBot.Entity;
using DiscordBot.Tools;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;

namespace DiscordBot.Commands
{
    public class WerewolvesCommands
    {
        private bool _inGame = false;

        private readonly TimeSpan _turnTimeSpan = TimeSpan.FromSeconds(50);

        private readonly TimeSpan _subTimeSpan = TimeSpan.FromSeconds(20);

        private readonly TimeSpan _wereWolvesTurn = TimeSpan.FromSeconds(20);

        private MultipleKeyDictionnary<DiscordUser, DiscordEmoji, Villager> _users;

        [Command("startlg"), Description("Commencez un loup garrou"), RequireRolesAttribute("NAFondateur")]
        public async Task Start(CommandContext ctx)
        {
            if (_inGame)
                throw new Exception("Please finish the current game");

            var baseTitle = "Creation of werewolf game";
            var embed = new DiscordEmbedBuilder
            {
                Title = baseTitle,
                Description = "press :heavy_plus_sign: (reaction) to subscribe",
                Color = new DiscordColor(0x00FF00)
            };

            var message = await ctx.RespondAsync(embed: embed);
            await message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, $":heavy_plus_sign:"));

            for (int i = _subTimeSpan.Seconds; i >= 1; i--)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                if (i % 5 != 0)
                    continue;

                embed.Title = baseTitle + $"({i.ToString()} seconds left)";
                await message.ModifyAsync(embed: embed);
            }

            embed.Title = baseTitle + $"(STARTED)";
            await message.ModifyAsync(embed: embed);

            var userResults = await message.GetReactionsAsync(DiscordEmoji.FromName(ctx.Client, ":heavy_plus_sign:"));
            _users = new MultipleKeyDictionnary<DiscordUser, DiscordEmoji, Villager>();
            int userCount = 1;
            foreach (var user in userResults)
            {
                if (user == message.Author)
                    continue;

                var dm = await ctx.Client.CreateDmAsync(user);
                var discordEmoji = DiscordEmoji.FromName(ctx.Client, $":{Tools.Tools.IntToEmoji(userCount)}:");
                _users.Add(user, discordEmoji, new Villager(dm, user, discordEmoji));
                await dm.SendMessageAsync($"You going to start a werewolf game");
                userCount++;
            }

            if (_users.Count == 0)
            {
                var embedEmpty = new DiscordEmbedBuilder
                {
                    Title = "End of the game :cry:",
                    Description = "No enought ppl to play",
                    Color = new DiscordColor(0xFF0000)
                };

                await ctx.RespondAsync(embed: embedEmpty);
                return;
            }

            await CreateRoles(ctx);
            _inGame = true;
        }

        [Command("stoplg"), Description("Start a werewolf"), RequireRolesAttribute("NAFondateur")]
        public async Task Stop(CommandContext ctx)
        {
            if (!_inGame)
                throw new Exception("T'es con ou tu le fais expres ?");

            var embed = new DiscordEmbedBuilder
            {
                Title = "End of the game",
                Description = "So  ? Who won ? :upside_down:",
                Color = new DiscordColor(0xFF0000)
            };

            await ctx.RespondAsync(embed: embed);

            _inGame = false;
        }

        public async Task Killed(CommandContext ctx, DiscordUser discordUser)
        {
            var user = _users[discordUser];

            var embed = new DiscordEmbedBuilder
            {
                Title = "Someone has been killed ",
                Description = $"{discordUser.Mention} is dead :), he/she was a {user}",
                Color = new DiscordColor(0x00FF00)
            };

            await ctx.RespondAsync(embed: embed);
            await user.DiscordDmChannel.SendMessageAsync("You have been kileld, sad :)");
            _users.Remove(discordUser);
        }


        public async Task<Villager> WereWolvesTurn(CommandContext ctx)
        {
            var baseTitle = "Who want you kill ?";
            var embed = RandomCommands.CreateEmbed(baseTitle, false, _users.Keys.Select(_ => _.Username).ToArray());

            var messages = new Dictionary<DiscordUser, DiscordMessage>();
            var loups = _users.Values.Where(_ => _.Role == Role.Werewolf).ToList();

            foreach (var loup in loups)
            {
                var message = await loup.DiscordDmChannel.SendMessageAsync(embed: embed);
                messages.Add(loup.DiscordUser, message);
                await RandomCommands.CreatePollReaction(ctx, message, _users.Count + 1);
            }

            ctx.Client.DebugLogger.LogMessage(LogLevel.Info, "NafBot", $"Start collecting Reaction", DateTime.Now);

            var tasks = new List<Task<ReactionCollectionContext>>();
            var interactivity = ctx.Client.GetInteractivityModule();

            foreach (var message in messages)
            {
                var task = Task.Run(() => interactivity.CollectReactionsAsync(message.Value, _wereWolvesTurn));
                tasks.Add(task);
            }

            var results = await Task.WhenAll(tasks);

            ctx.Client.DebugLogger.LogMessage(LogLevel.Info, "NafBot", $"Finish collecting Reaction", DateTime.Now);

            var dicoResults = new Dictionary<Villager, int>();
            foreach (var result in results)
            {
                foreach (var resultReaction in result.Reactions)
                {
                    var user = _users[resultReaction.Key];

                    if (user == null)
                        continue;

                    if (dicoResults.ContainsKey(user))
                    {
                        dicoResults[user] = dicoResults[user] + resultReaction.Value;
                    }
                    else
                    {
                        dicoResults.Add(user, resultReaction.Value);
                    }
                }
            }

            var killedUser = dicoResults.OrderByDescending(_ => _.Value).Take(1).Select(_ => _.Key).First();
            await ctx.RespondAsync($"{killedUser.Name} a été choisi pour mourir :)");
            return killedUser;
        }

        public async Task CreateRoles(CommandContext ctx)
        {
            var rnd = new Random();
            var roles = new List<Role>();
            int numberWereWolf = _users.Count > 5 ? 1 : 2;
            foreach (var user in _users)
            {
                var roleFound = false;
                Role role = Role.Werewolf;
                while (!roleFound)
                {
                    role = (Role) rnd.Next(0, 5);
                    if (role is Role.Werewolf && numberWereWolf > 0)
                    {
                        roleFound = true;
                        numberWereWolf--;
                    }
                    else if (!roles.Contains(role))
                    {
                        roleFound = true;
                        roles.Add(role);
                    }
                }
                user.Value.Role = role;
                await user.Value.DiscordDmChannel.SendMessageAsync($"Votre rôle est: {role}");
            }
        }
    }
}