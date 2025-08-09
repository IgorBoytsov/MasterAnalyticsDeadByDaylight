using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class GameEventDTO : BaseDTO<GameEventDTO>
    {
        public int IdGameEvent { get; set; }

        public string GameEventName { get; set; } = null!;

        public string? GameEventDescription { get; set; }
    }
}
