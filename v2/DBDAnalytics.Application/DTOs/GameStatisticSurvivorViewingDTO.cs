namespace DBDAnalytics.Application.DTOs
{
    public class GameStatisticSurvivorViewingDTO
    {
        public int IdGameStatistic { get; set; }

        public int IdSurvivor { get; set; }

        public int IdTypeDeath { get; set; }

        public byte[] SurvivorImage { get;  set; } = null!;

        public DateTime? DateMatch { get; set; }

        public string MatchTime { get; set; } = null!;

        public string MapName { get; set; } = null!;

        public int CountKill { get; set; }

        public int CountHook { get; set; }

        public int CountRecentGenerator { get; set; }
    }
}