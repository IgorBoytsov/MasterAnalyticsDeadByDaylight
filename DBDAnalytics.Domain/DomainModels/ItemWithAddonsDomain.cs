using System.Collections.ObjectModel;

namespace DBDAnalytics.Domain.DomainModels
{
    public class ItemWithAddonsDomain
    {
        private ItemWithAddonsDomain(int idItem, string itemName, byte[]? itemImage, string? itemDescription, IEnumerable<ItemAddonDomain?> itemAddons)
        {
            IdItem = idItem;
            ItemName = itemName;
            ItemImage = itemImage;
            ItemDescription = itemDescription;

            foreach (var itemAddon in itemAddons)
                ItemAddons.Add(itemAddon);
        }

        public int IdItem { get; private set; }

        public string ItemName { get; private set; } = null!;

        public byte[]? ItemImage { get; private set; }

        public string? ItemDescription { get; private set; }

        public List<ItemAddonDomain> ItemAddons { get; private set; } = new List<ItemAddonDomain>();

        public static (ItemWithAddonsDomain? ItemWithAddonsDomain, string? Message) Create(int idItem, string itemName, byte[]? itemImage, string? itemDescription, IEnumerable<ItemAddonDomain?> itemAddons)
        {
            string message = string.Empty;

            var itemWithAddons = new ItemWithAddonsDomain(idItem, itemName, itemImage, itemDescription, itemAddons);

            return (itemWithAddons, message);
        }
    }
}