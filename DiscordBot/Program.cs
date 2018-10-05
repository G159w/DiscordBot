using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using DiscordBot.Commands;
using DiscordBot.Tools;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot
{
    public static class Program
    {
        private static DiscordClient _discord;
        private static CommandsNextModule _commands;
        public static ServiceProvider ServiceProvider;

        public static void Main(string[] args)
        {
            SetUpAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task SetUpAsync()
        {
            // Client configuration
            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "<KEY>",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            _discord.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehaviour = TimeoutBehaviour.Ignore,
                // default pagination timeout to 5 minutes
                PaginationTimeout = TimeSpan.FromMinutes(5),
                // default timeout for other actions to 2 minutes
                Timeout = TimeSpan.FromMinutes(2)
            });
            

            // Client method handling
            _discord.Ready += DiscordHandling.Client_Ready;
            _discord.GuildAvailable += DiscordHandling.Client_GuildAvailable;
            _discord.ClientErrored += DiscordHandling.Client_ClientError;
            
            
            // Command configuration
            _commands = _discord.UseCommandsNext(new CommandsNextConfiguration
            {   
                StringPrefix = "/",
                EnableDms = true
            });
            
            
            // Command method handling
            _commands.CommandExecuted += DiscordHandling.Commands_CommandExecuted;
            _commands.CommandErrored += DiscordHandling.Commands_CommandErrored;
            
            ServiceProvider = Container.Configure();
            
            _commands.RegisterCommands<FortniteCommands>();
            _commands.RegisterCommands<LolCommands>();
            _commands.RegisterCommands<RandomCommands>();
            _commands.RegisterCommands<WerewolvesCommands>();
            _commands.RegisterCommands<WhoOfUsCommands>();

            await _discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}