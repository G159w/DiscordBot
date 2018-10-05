using System.Collections.Generic;

namespace DiscordBot.GameRequests.Models
{
    public class FortniteResponse
    {
        public List<LifetimeStatsResponse> LifeTimeStats { get; set; }
        public InnerStats Stats { get; set; }

        public class LifetimeStatsResponse
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public class InnerStats
        {
            public P2 p2 { get; set; }
            public P10 p10 { get; set; }
            public P9 p9 { get; set; }
        }

        public class P2
        {
            public Top1 Top1 { get; set; }
        }

        public class P10
        {
            public Top1 Top1 { get; set; }
        }

        public class P9
        {
            public Top1 Top1 { get; set; }

        }

        public class Top1
        {
            public string Value { get; set; }
        }
    }
}
