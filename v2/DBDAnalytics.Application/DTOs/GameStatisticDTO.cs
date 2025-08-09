using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class GameStatisticDTO : BaseDTO<GameStatisticDTO>
    {
        public int IdGameStatistic { get; set; }

        public int IdKiller { get; set; }

        public int IdMap { get; set; }

        public int IdWhoPlacedMap { get; set; }

        public int IdWhoPlacedMapWin { get; set; }

        public int IdPatch { get; set; }

        public int IdGameMode { get; set; }

        public int IdGameEvent { get; set; }

        public int IdSurvivors1 { get; set; }

        public int IdSurvivors2 { get; set; }

        public int IdSurvivors3 { get; set; }

        public int IdSurvivors4 { get; set; }

        public DateTime? DateTimeMatch { get; set; }

        public string? GameTimeMatch { get; set; }

        public int CountKills { get; set; }

        public int CountHooks { get; set; }

        public int NumberRecentGenerators { get; set; }

        public string? DescriptionGame { get; set; }

        public byte[]? ResultMatch { get; set; }

        public int IdMatchAttribute { get; set; }
    }
}