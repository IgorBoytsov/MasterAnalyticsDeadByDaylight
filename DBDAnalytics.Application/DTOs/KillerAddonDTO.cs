using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class KillerAddonDTO : BaseDTO<KillerAddonDTO>
    {
        public int IdKillerAddon { get; set; }

        public int IdKiller { get; set; }

        public int? IdRarity { get; set; }

        public string AddonName { get; set; } = null!;

        public byte[]? AddonImage { get; set; }

        public string? AddonDescription { get; set; }
    }
}