namespace DBDAnalytics.Domain.DomainModels
{
    public class SurvivorPerkDomain
    {
        private SurvivorPerkDomain(int idSurvivorPerk, int idSurvivor, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            IdSurvivorPerk = idSurvivorPerk;
            IdSurvivor = idSurvivor;
            PerkName = perkName;
            PerkImage = perkImage;
            IdCategory = idCategory;
            PerkDescription = perkDescription;
        }
        public int IdSurvivorPerk { get; private set; }

        public int IdSurvivor { get; private set; }

        public string PerkName { get; private set; } = null!;

        public byte[]? PerkImage { get; private set; }

        public int? IdCategory { get; private set; }

        public string? PerkDescription { get; private set; }

        public static (SurvivorPerkDomain? SurvivorPerkDomain, string? Message) Create(int idSurvivorPerk, int idSurvivor, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(perkName))
            {
                return (null, "Вы не указали название.");
            }
 
            var perk = new SurvivorPerkDomain(idSurvivorPerk, idSurvivor, perkName, perkImage, idCategory, perkDescription);

            return (perk, message);
        }
    }
}