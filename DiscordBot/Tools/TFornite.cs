namespace DiscordBot.Tools
{
    public partial class TFortnite
    {
        public long Id { get; set; }
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
        public int WinPercent { get; set; }
        public long UserGameId { get; set; }
    }
}