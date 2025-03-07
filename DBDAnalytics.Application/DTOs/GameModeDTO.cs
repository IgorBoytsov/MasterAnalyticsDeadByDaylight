using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class GameModeDTO : BaseDTO<GameModeDTO>
    {
        public int IdGameMode { get; set; }

        public string GameModeName { get; set; } = null!;

        public string? GameModeDescription { get; set; }
    }
}
