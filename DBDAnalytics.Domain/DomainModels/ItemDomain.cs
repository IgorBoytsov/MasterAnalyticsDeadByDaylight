namespace DBDAnalytics.Domain.DomainModels
{
    public class ItemDomain
    {
        private ItemDomain(int idItem, string itemName, byte[]? itemImage, string? itemDescription)
        {
            IdItem = idItem;
            ItemName = itemName;
            ItemImage = itemImage;
            ItemDescription = itemDescription;
        }

        public int IdItem { get; private set; }

        public string ItemName { get; private set; } = null!;

        public byte[]? ItemImage { get; private set; }

        public string? ItemDescription { get; private set; }

        public static (ItemDomain? ItemDomain, string? Message) Create(int idItem, string itemName, byte[]? itemImage, string? itemDescription)
        {
            string message = string.Empty;
            const int MaxItemNameLength = 100;
            const int MaxItemDescriptionLength = 1000;

            if (string.IsNullOrWhiteSpace(itemName))
            {
                return (null, "Укажите название предмета.");
            }

            if (itemName.Length > MaxItemNameLength)
            {
                return (null, $"Максимальная длинна имени не может превышать - {MaxItemNameLength}");
            }

            if (itemDescription?.Length > MaxItemDescriptionLength)
            {
                return (null, $"Максимальная длинна имени не может превышать - {MaxItemDescriptionLength}");
            }

            var item = new ItemDomain(idItem, itemName, itemImage, itemDescription);

            return (item, message);
        }
    }
}