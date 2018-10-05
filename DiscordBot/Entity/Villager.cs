using DSharpPlus.Entities;

namespace DiscordBot.Entity
{
    public class Villager
    {
        public DiscordDmChannel DiscordDmChannel { get; }
        
        public DiscordUser DiscordUser { get; }
        
        public DiscordEmoji DiscordEmoji { get; set; }
        
        public string Name { get; set; }
        
        public Role Role { get; set; }

        public Villager(DiscordDmChannel discordDmChannel, DiscordUser discordUser, DiscordEmoji discordEmoji)
        {
            DiscordDmChannel = discordDmChannel;
            DiscordUser = discordUser;
            DiscordEmoji = discordEmoji;
            Name = "Villager";
        }
    }


    public enum Role
    {
        Werewolf,
        Villager,
        Doctor,
        Voyante,
        Hunter,
    }
}