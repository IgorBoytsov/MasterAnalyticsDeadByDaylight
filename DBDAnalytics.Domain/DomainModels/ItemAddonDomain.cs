namespace DBDAnalytics.Domain.DomainModels
{
    public class ItemAddonDomain
    {
        private ItemAddonDomain(int idItemAddon, int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription)
        {
            IdItemAddon = idItemAddon;
            IdItem = idItem;
            IdRarity = idRarity;
            ItemAddonName = itemAddonName;
            ItemAddonImage = itemAddonImage;
            ItemAddonDescription = itemAddonDescription;
        }

        public int IdItemAddon { get; private set; }

        public int IdItem { get; private set; }

        public int? IdRarity { get; private set; }

        public string ItemAddonName { get; private set; } = null!;

        public byte[]? ItemAddonImage { get; private set; }

        public string? ItemAddonDescription { get; private set; }

        public static (ItemAddonDomain? ItemAddonDomain, string? Message) Create(int idItemAddon, int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription)
        {
            string message = string.Empty;
            const string DefaultAddonDescription = "Описание отсутствует.";
            const int MaxAddonNameLength = 100;
            const int MaxAddonDescriptionLength = 1000;

            if (string.IsNullOrWhiteSpace(itemAddonName))
            {
                return (null, "Вы не указали название аддона.");
            }

            if (idItem < 0)
            {
                return (null, "Вы не указали ID предмета, к которому относиться аддон.");
            }  
            
            if (idRarity < 0)
            {
                return (null, "Вы не указали ID качества, к которому относиться аддон.");
            }
            
            if (itemAddonName?.Length > MaxAddonNameLength)
            {
                return (null, $"Максимальная допустимая длинна названия - {MaxAddonNameLength}");
            }  
            
            if (itemAddonDescription?.Length > MaxAddonDescriptionLength)
            {
                return (null, $"Максимальная допустимая длинна описания - {MaxAddonDescriptionLength}");
            }

            string finalItemAddonDescription = string.IsNullOrWhiteSpace(itemAddonDescription) ? DefaultAddonDescription : itemAddonDescription;

            var itemAddon = new ItemAddonDomain(idItemAddon, idItem, idRarity, itemAddonName, itemAddonImage, finalItemAddonDescription);

            return (itemAddon, message);
        }
    }
}