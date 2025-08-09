namespace DBDAnalytics.Domain.DomainModels
{
    public class PlatformDomain
    {
        private PlatformDomain(int idPlatform, string platformName, string? platformDescription)
        {
            IdPlatform = idPlatform;
            PlatformName = platformName;
            PlatformDescription = platformDescription;
        }
        public int IdPlatform { get; private set; }

        public string PlatformName { get; private set; } = null!;

        public string? PlatformDescription { get; private set; }

        public static (PlatformDomain? PlatformDomain, string? Message) Create(int idPlatform, string platformName, string? platformDescription)
        {
            string message = string.Empty;
            const int MaxPlatformNameLength = 50;
            const int MaxPlatformDescriptionLength = 50;

            if (string.IsNullOrWhiteSpace(platformName))
            {
                return (null, "Вы не назвали платформу.");
            }

            if (platformName.Length > MaxPlatformNameLength)
            {
                return (null, $"Максимально допустимая длинна названия платформы - {MaxPlatformNameLength}");
            }

            if (platformDescription?.Length > MaxPlatformDescriptionLength)
            {
                return (null, $"Максимально допустимая длинна описания платформы - {MaxPlatformDescriptionLength}");
            }
                
            var platform = new PlatformDomain(idPlatform, platformName, platformDescription);

            return (platform, message);
        }
    }
}