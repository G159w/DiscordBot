using System.ComponentModel.Design;
using DiscordBot.Commands;
using DiscordBot.GameRequests;
using DiscordBot.RequestFactory;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot
{
    public static class Container
    {
        public static ServiceProvider Configure()
        {
            var serviceProvider = new ServiceCollection();
            
            serviceProvider.AddSingleton<IRequestFactory, RequestFactory.RequestFactory>();
            serviceProvider.AddSingleton<FortniteApi>();
            serviceProvider.AddSingleton<FortniteCommands>();
            serviceProvider.BuildServiceProvider();
            
            return serviceProvider.BuildServiceProvider();;
        }
    }
}