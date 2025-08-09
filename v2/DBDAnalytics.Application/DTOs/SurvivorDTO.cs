using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class SurvivorDTO : BaseDTO<SurvivorDTO>
    {
        public int IdSurvivor { get; set; }

        public string SurvivorName { get; set; } = null!;

        public byte[]? SurvivorImage { get; set; }

        public string? SurvivorDescription { get; set; }
    }
}