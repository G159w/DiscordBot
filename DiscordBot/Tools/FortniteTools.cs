using System;
using System.Globalization;
using System.Linq;
using DiscordBot.GameRequests.Models;

namespace DiscordBot.Tools
{
    public static class FortniteTools
    {
        public static (Func<TFortnite, object>, Func<TFortnite, object>) GetSortingFunctions(string mode)
        {
            if (mode == "wins")
            {
                object First(TFortnite s) => s.Wins;
                object Second(TFortnite s) => s.Wins;
                return (First, Second);
            }

            if (mode == "kills")
            {
                object First(TFortnite s) => s.Kills;
                object Second(TFortnite s) => s.Kills;
                return (First, Second);
            }

            if (mode == "kd")
            {
                object First(TFortnite s) => s.Kd;
                object Second(TFortnite s) => s.Kd;
                return (First, Second);
            }

            if (mode == "top1")
            {
                object First(TFortnite s) => s.SoloTop1;
                object Second(TFortnite s) => s.SoloTop1;
                return (First, Second);
            }
            
            return (null, null);
        }


        public static FortniteStatsResponse ConvertToResponse(this TFortnite fortniteDbStats)
        {
            if (fortniteDbStats == null)
            {
                return null;
            }
            FortniteStatsResponse fortniteResponse = new FortniteStatsResponse
            {
                SoloTop1 = fortniteDbStats.SoloTop1,
                SoloTop10 = fortniteDbStats.SoloTop10,
                SoloTop25 = fortniteDbStats.SoloTop25,
                DuoTop1 = fortniteDbStats.DuoTop1,
                DuoTop5 = fortniteDbStats.DuoTop5,
                DuoTop12 = fortniteDbStats.DuoTop12,
                SquadTop1 = fortniteDbStats.SquadTop1,
                SquadTop3 = fortniteDbStats.SquadTop3,
                SquadTop6 = fortniteDbStats.SquadTop6,
                Kd = fortniteDbStats.Kd,
                Kills = fortniteDbStats.Kills,
                Matches = fortniteDbStats.Matches,
                Wins = fortniteDbStats.Wins,
                WinsPercent = fortniteDbStats.WinPercent
            };
            return fortniteResponse;
        }

        public static FortniteStatsResponse ConvertFromApiResponse(this FortniteResponse fortniteResponse)
        {
            var fortniteStatsResponse = new FortniteStatsResponse();
            fortniteStatsResponse.DuoTop1 = Int32.Parse(fortniteResponse.Stats.p10.Top1.Value);
            fortniteStatsResponse.DuoTop5 = Int32.Parse(fortniteResponse.LifeTimeStats.FirstOrDefault(i => i.Key == "Top 5s")?.Value);
            fortniteStatsResponse.DuoTop12 = Int32.Parse(fortniteResponse.LifeTimeStats.FirstOrDefault(i => i.Key == "Top 12s")?.Value);
            fortniteStatsResponse.SoloTop1 = Int32.Parse(fortniteResponse.Stats.p2.Top1.Value);
            fortniteStatsResponse.SoloTop25 = Int32.Parse(fortniteResponse.LifeTimeStats.FirstOrDefault(i => i.Key == "Top 25s")?.Value);
            fortniteStatsResponse.SquadTop1 = Int32.Parse(fortniteResponse.Stats.p9.Top1.Value);
            fortniteStatsResponse.SquadTop3 = Int32.Parse(fortniteResponse.LifeTimeStats.FirstOrDefault(i => i.Key == "Top 3s")?.Value);
            fortniteStatsResponse.SquadTop6 = Int32.Parse(fortniteResponse.LifeTimeStats.FirstOrDefault(i => i.Key == "Top 6s")?.Value);
            fortniteStatsResponse.Kills = Int32.Parse(fortniteResponse.LifeTimeStats.FirstOrDefault(i => i.Key == "Kills")?.Value);
            fortniteStatsResponse.Kd = float.Parse(fortniteResponse.LifeTimeStats.FirstOrDefault(i => i.Key == @"K/d")?.Value, CultureInfo.InvariantCulture.NumberFormat);
            fortniteStatsResponse.Matches = Int32.Parse(fortniteResponse.LifeTimeStats.FirstOrDefault(i => i.Key == "Matches Played")?.Value);
            fortniteStatsResponse.WinsPercent = Int32.Parse(fortniteResponse.LifeTimeStats.FirstOrDefault(i => i.Key == @"Win%")?.Value.Split('%')[0]);
            fortniteStatsResponse.Wins = Int32.Parse(fortniteResponse.LifeTimeStats.FirstOrDefault(i => i.Key == "Wins")?.Value);
            return fortniteStatsResponse;
        }
    }
}
