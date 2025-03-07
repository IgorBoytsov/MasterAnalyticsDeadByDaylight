namespace DBDAnalytics.Domain.DomainModels
{
    public class SurvivorWithPerksDomain
    {
        private SurvivorWithPerksDomain(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription, IEnumerable<SurvivorPerkDomain> survivorPerks)
        {
            IdSurvivor = idSurvivor;
            SurvivorName = survivorName;
            SurvivorImage = survivorImage;
            SurvivorDescription = survivorDescription;

            foreach (var item in survivorPerks)
                SurvivorPerks.Add(item);
        }

        public int IdSurvivor { get; set; }

        public string SurvivorName { get; set; } = null!;

        public byte[]? SurvivorImage { get; set; }

        public string? SurvivorDescription { get; set; }

        public List<SurvivorPerkDomain> SurvivorPerks { get; set; } = new List<SurvivorPerkDomain>();

        public static (SurvivorWithPerksDomain? SurvivorWithPerksDomain, string? Message) Create(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription, IEnumerable<SurvivorPerkDomain> survivorPerks)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(survivorName))
                return (null, "Укажите имя персонажу.");

            var survivorWithPerks = new SurvivorWithPerksDomain(idSurvivor, survivorName, survivorImage, survivorDescription, survivorPerks);

            return (survivorWithPerks, message);
        }
    }
}