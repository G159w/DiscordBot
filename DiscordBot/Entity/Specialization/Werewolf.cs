using DSharpPlus.Entities;

namespace DiscordBot.Entity.Specialization
{
    public class Werewolf : Villager
    {

        public Werewolf(DiscordDmChannel discordDmChannel, DiscordUser discordUser, DiscordEmoji discordEmoji) : base(discordDmChannel, discordUser, discordEmoji)
        {
            Role = Role.Werewolf;
            Name = "Werewolf";
        }
    }
}