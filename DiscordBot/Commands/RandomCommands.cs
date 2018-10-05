using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    public sealed class RandomCommands
    {
        
        [Command("poll"), Description("Run a poll with reactions."), RequireRolesAttribute("NAFondateur")]
        public async Task Poll(CommandContext ctx, string title, params string[] options)
        {
            await PollMethod(ctx, title, false, options);
        }
        
        [Command("pollall"), Description("Run a poll with reactions."), RequireRolesAttribute("NAFondateur")]
        public async Task PollAll(CommandContext ctx, string title, params string[] options)
        {
            await PollMethod(ctx, title, true, options);
        }

        public static async Task<DiscordMessage> PollMethod(CommandContext ctx, string title, bool ping, params string[] options)
        {
            var embed = CreateEmbed(title, ping, options);
            var message = ping ? "@everyone" : string.Empty;
            var msg = await ctx.RespondAsync(message, embed: embed);
            await CreatePollReaction(ctx, msg, options.Length + 1);
            await ctx.Message.DeleteAsync();
            return msg;
        }

        public static DiscordEmbedBuilder CreateEmbed(string title, bool ping, params string[] options)
        {
            var message = string.Empty;
            var count = 1;
            
            foreach (var option in options)
            {
                message += $":{Tools.Tools.IntToEmoji(count)}: {option}\n \n";
                count++;
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = title ,
                Description = message,
                Color = new DiscordColor(0x00FF00)
            };

            return embed;
        }
       
        public static async Task CreatePollReaction(CommandContext ctx, DiscordMessage msg, int count)
        {
            // add the options as reactions
            for (var i = 1; i < count; i++)
                await msg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, $":{Tools.Tools.IntToEmoji(i)}:"));
        }
        
        [Command("clear"), Description("Clear messages"), RequireRolesAttribute("NAFondateur")]
        public async Task Clear(CommandContext ctx, int number)
        {
            await ctx.Message.DeleteAsync();
            var messages = await ctx.Channel.GetMessagesAsync(number);

            foreach (var message in messages)
            {
                Task.Run(() => ctx.Channel.DeleteMessageAsync(message));
            }
        }
        
        [Command("rule"), Description("Remember Rules"), RequireRolesAttribute("NAFondateur")]
        public async Task Rule(CommandContext ctx, int number)
        {
            number--;
            
            var channel = ctx.Guild.Channels.FirstOrDefault(_ => _.Name == "regles");
            
            if (channel == null)
                throw new Exception("Where is rule channel ?");
            
            var messages = channel.GetMessagesAsync(99).Result.Reverse().ToArray();
            
            var embed = new DiscordEmbedBuilder
            {
                Title = "Listen:" ,
                Description = messages[number].Content,
                Color = new DiscordColor(0x00FF00)
            };
            
            await ctx.RespondAsync(embed:embed);
        }
    }
}