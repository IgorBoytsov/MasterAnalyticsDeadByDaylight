using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class ItemAddonDTO : BaseDTO<ItemAddonDTO>
    {
        public int IdItemAddon { get; set; }

        public int IdItem { get; set; }

        public int? IdRarity { get; set; }

        public string ItemAddonName { get; set; } = null!;

        public byte[]? ItemAddonImage { get; set; }

        public string? ItemAddonDescription { get; set; }
    }
}
