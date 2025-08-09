using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class PlatformDTO : BaseDTO<PlatformDTO>
    {
        public int IdPlatform { get; set; }

        public string PlatformName { get; set; } = null!;

        public string? PlatformDescription { get; set; }
    }
}