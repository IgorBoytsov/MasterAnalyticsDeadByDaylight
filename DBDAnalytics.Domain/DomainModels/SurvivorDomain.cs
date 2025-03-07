namespace DBDAnalytics.Domain.DomainModels
{
    public class SurvivorDomain
    {
        private SurvivorDomain(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription)
        {
            IdSurvivor = idSurvivor;
            SurvivorName = survivorName;
            SurvivorImage = survivorImage;
            SurvivorDescription = survivorDescription;
        }

        public int IdSurvivor { get; private set; }

        public string SurvivorName { get; private set; } = null!;

        public byte[]? SurvivorImage { get; private set; }

        public string? SurvivorDescription { get; private set; }

        public static (SurvivorDomain? SurvivorDomain, string Message) Create(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription)
        {
            string message = string.Empty;
            const int MaxSurvivorNameLength = 100;
            const int MaxSurvivorDescriptionLength = 5000;

            if (string.IsNullOrWhiteSpace(survivorName))
            {
                return (null, "Вы не указали имя выжившему.");
            }

            if (survivorName.Length > MaxSurvivorNameLength)
            {
                return (null, $"Максимально допустимая длинна имени - {MaxSurvivorNameLength}");
            }

            if (survivorDescription?.Length > MaxSurvivorDescriptionLength)
            {
                return (null, $"Максимально допустимая длинна описания - {MaxSurvivorDescriptionLength}");
            }

            var survivor = new SurvivorDomain(idSurvivor, survivorName, survivorImage, survivorDescription);

            return (survivor, message);
        }
    }
}