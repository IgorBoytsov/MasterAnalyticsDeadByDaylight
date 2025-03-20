namespace DBDAnalytics.Application.DTOs
{
    public class GameStatisticKillerViewingDTO
    {
        public int IdGameStatistic { get; set; }

        public int IdKiller { get; set; }

        public byte[] KillerImage { get; set; } = null!;

        public DateTime? DateMatch { get; set; }

        public string MatchTime { get; set; } = null!;

        public string MapName { get; set; } = null!;

        public int CountKill { get; set; }

        public int CountHook { get; set; }

        public int CountRecentGenerator { get; set; }
    }
}