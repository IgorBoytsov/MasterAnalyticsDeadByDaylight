using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class SurvivorPerkDTO : BaseDTO<SurvivorPerkDTO>
    {
        public int IdSurvivorPerk { get; set; }

        public int IdSurvivor { get; set; }

        public string PerkName { get; set; } = null!;

        public byte[]? PerkImage { get; set; }

        public int? IdCategory { get; set; }

        public string? PerkDescription { get; set; }
    }
}
