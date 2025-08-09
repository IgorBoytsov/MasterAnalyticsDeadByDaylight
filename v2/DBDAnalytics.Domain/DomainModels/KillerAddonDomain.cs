namespace DBDAnalytics.Domain.DomainModels
{
    public class KillerAddonDomain
    {
        private KillerAddonDomain(int idKillerAddon, int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription)
        {
            IdKillerAddon = idKillerAddon;
            IdKiller = idKiller;
            IdRarity = idRarity;
            AddonName = addonName;
            AddonImage = addonImage;
            AddonDescription = addonDescription;
        }

        public int IdKillerAddon { get; private set; }

        public int IdKiller { get; private set; }

        public int? IdRarity { get; private set; }

        public string AddonName { get; private set; } = null!;

        public byte[]? AddonImage { get; private set; }

        public string? AddonDescription { get; private set; }

        public static (KillerAddonDomain? KillerAddonDomain, string? Message) Create(int idKillerAddon, int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription)
        {
            string message = string.Empty;
            const int MaxNameAddonLength = 100;
            const int MaxAddonDescriptionLength = 1000;

            if (string.IsNullOrWhiteSpace(addonName))
            {
                return (null, "Укажите название улучшение");
            } 
            
            if (addonName.Length > MaxNameAddonLength)
            {
                return (null, $"Максимально допустимая длинна названия - {MaxNameAddonLength}");
            } 
            
            if (addonDescription?.Length > MaxAddonDescriptionLength)
            {
                return (null, $"Максимально допустимая длинна описания - {MaxAddonDescriptionLength}");
            }
                
            if (idKiller < 0)
            {
                return (null, "Вы не выбрали к какому убийце относиться улучшение");             
            }

            if (idRarity < 0)
            {
                return (null, "Вы не выбрали качество улучшения.");
            }

            var killerAddon = new KillerAddonDomain(idKillerAddon, idKiller, idRarity, addonName, addonImage, addonDescription);

            return (killerAddon, message);
        }
    }
}
