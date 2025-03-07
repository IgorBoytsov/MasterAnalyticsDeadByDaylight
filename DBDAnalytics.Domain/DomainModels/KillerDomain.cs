namespace DBDAnalytics.Domain.DomainModels
{
    public class KillerDomain
    {
        private KillerDomain(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage)
        {
            IdKiller = idKiller;
            KillerName = killerName;
            KillerImage = killerImage;
            KillerAbilityImage = killerAbilityImage;
        }
        public int IdKiller { get; private set; }

        public string KillerName { get; private set; } = null!;

        public byte[]? KillerImage { get; private set; }

        public byte[]? KillerAbilityImage { get; private set; }

        public static (KillerDomain? KillerDomain, string? Message) Create(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage)
        {
            string message = string.Empty;
            const int MaxKillerNameLength = 100;

            if (string.IsNullOrWhiteSpace(killerName))
            {
                return (null, "Вы оставили имя убийцы пустым.");
            }

            if (killerName.Length > MaxKillerNameLength)
            {
                return (null, $"Максимально допустимая длинна имени киллеру - {MaxKillerNameLength}");
            }

            var killer = new KillerDomain(idKiller, killerName, killerImage, killerAbilityImage);

            return (killer, message);
        }
    }
}