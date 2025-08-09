using DBDAnalytics.Application.DTOs.BaseDTOs;
using System.Collections.ObjectModel;

namespace DBDAnalytics.Application.DTOs
{
    public class ItemWithAddonsDTO : BaseDTO<ItemWithAddonsDTO>
    {
        public int IdItem { get; set; }

        public string ItemName { get; set; } = null!;

        public byte[]? ItemImage { get; set; }

        public string? ItemDescription { get; set; }

        public ObservableCollection<ItemAddonDTO> ItemAddons { get; set; } = new ObservableCollection<ItemAddonDTO>();
    }
}