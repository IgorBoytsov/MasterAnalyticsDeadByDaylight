using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class KillerPerkDTO : BaseDTO<KillerPerkDTO>
    {
        public int IdKillerPerk { get; set; }

        public int IdKiller { get; set; }

        public string PerkName { get; set; } = null!;

        public byte[]? PerkImage { get; set; }

        public int? IdCategory { get; set; }

        public string? PerkDescription { get; set; }
    }
}
