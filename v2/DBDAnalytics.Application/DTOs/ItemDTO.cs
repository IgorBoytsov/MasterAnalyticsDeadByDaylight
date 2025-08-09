using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class ItemDTO : BaseDTO<ItemDTO>
    {
        public int IdItem { get; set; }

        public string ItemName { get; set; } = null!;

        public byte[]? ItemImage { get; set; }

        public string? ItemDescription { get; set; }
    }
}
