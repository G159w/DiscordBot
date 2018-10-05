namespace DiscordBot.GameRequests.Models
{
    public class FortniteStatsResponse
    {
        public int SoloTop1 { get; set; }

        public int SoloTop10 { get; set; }

        public int SoloTop25 { get; set; }

        public int DuoTop1 { get; set; }

        public int DuoTop5 { get; set; }

        public int DuoTop12 { get; set; }

        public int SquadTop1 { get; set; }

        public int SquadTop3 { get; set; }

        public int SquadTop6 { get; set; }

        public int Matches { get; set; }

        public double Kd { get; set; }

        public int Kills { get; set; }

        public int Wins { get; set; }

        public int WinsPercent { get; set; }
        public object UserGame { get; set; }
    }
}
