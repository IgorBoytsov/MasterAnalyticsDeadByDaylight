namespace DBDAnalytics.Application.DTOs
{
    public class GameStatisticViewingDTO
    {
        public int IdGameStatistic { get; set; }

        public int IdKiller { get; set; }

        public byte[] KillerImage { get; set; } = null!;

        public DateTime? DateMatch { get; set; }

        public string MatchTime { get; set; } = null!;

        public string MapName { get; set; } = null!;

        public byte[] MapImage { get; set; } = null!;

        public int CountKill { get; set; }

        public int CountHook { get; set; }

        public int CountRecentGenerator { get; set; }

        public byte[]? ResultMatch { get; set; }
    }
}