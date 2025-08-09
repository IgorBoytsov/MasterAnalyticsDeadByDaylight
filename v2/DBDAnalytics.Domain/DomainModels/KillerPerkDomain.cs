namespace DBDAnalytics.Domain.DomainModels
{
    public class KillerPerkDomain
    {
        private KillerPerkDomain(int idKillerPerk, int idKiller, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            IdKillerPerk = idKillerPerk;
            IdKiller = idKiller;
            PerkName = perkName;
            PerkImage = perkImage;
            IdCategory = idCategory;
            PerkDescription = perkDescription;
        }

        public int IdKillerPerk { get; private set; }

        public int IdKiller { get; private set; }

        public string PerkName { get; private set; } = null!;

        public byte[]? PerkImage { get; private set; }

        public int? IdCategory { get; private set; }

        public string? PerkDescription { get; set; }

        public static (KillerPerkDomain? KillerPerkDomain, string? Message) Create(int idKillerPerk, int idKiller, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            string message = string.Empty;
            const int MaxPerkNameLenght = 150;
            const int MaxPerkDescriptionLenght = 2000;

            if (string.IsNullOrWhiteSpace(perkName))
            {
                return (null, "Укажите название перку.");
            }
                   
            if (perkName.Length > MaxPerkNameLenght)
            {
                return (null, $"Максимально допустимая длинна названия - {MaxPerkNameLenght}");
            }  
            
            if (perkDescription?.Length > MaxPerkDescriptionLenght)
            {
                return (null, $"Максимально допустимая длинна названия - {MaxPerkDescriptionLenght}");
            }
                
            var perk = new KillerPerkDomain(idKillerPerk, idKiller, perkName, perkImage, idCategory, perkDescription);

            return (perk, message);
        }
    }
}