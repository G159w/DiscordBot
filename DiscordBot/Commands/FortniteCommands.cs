using System;
using System.Threading.Tasks;
using DiscordBot.GameRequests;
using DiscordBot.Tools;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Commands
{
    public class FortniteCommands
    {
        private readonly FortniteApi _fortniteApi;

        public FortniteCommands()
        {
            _fortniteApi = Program.ServiceProvider.GetService<FortniteApi>();
        }

        /// <summary>
        /// Command to get forntnite stats of a user 
        /// </summary>
        /// <param name="ctx">command context</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">No user given</exception>
        /// <exception cref="Exception">failed to get user after fortnite call</exception>
        [Command("rankft")]
        public async Task GetStats(CommandContext ctx)
        {
            var user = ctx.RawArgumentString.Trim();
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("Invalid user input");
            
            var fortniteStat = _fortniteApi.GetStatsFromApi(user);
            
            if (user == null)
                throw new Exception("Failed to get user information");
            
            await ctx.RespondAsync($"{user} a {fortniteStat.Result.Kd} de kd");
        }
    }
}