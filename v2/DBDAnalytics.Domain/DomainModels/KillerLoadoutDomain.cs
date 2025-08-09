namespace DBDAnalytics.Domain.DomainModels
{
    public class KillerLoadoutDomain
    {
        private KillerLoadoutDomain(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage, IEnumerable<KillerAddonDomain?> killerAddons, IEnumerable<KillerPerkDomain?> killerPerks)
        {
            IdKiller = idKiller;
            KillerName = killerName;
            KillerImage = killerImage;
            KillerAbilityImage = killerAbilityImage;

            foreach (var item in killerAddons)
                KillerAddons.Add(item);

            foreach (var item in killerPerks)
                KillerPerks.Add(item);
        }

        public int IdKiller { get; private set; }

        public string KillerName { get; private set; } = null!;

        public byte[]? KillerImage { get; private set; }

        public byte[]? KillerAbilityImage { get; private set; }

        public List<KillerAddonDomain?> KillerAddons { get; private set; } = [];

        public List<KillerPerkDomain?> KillerPerks { get; private set; } = [];

        public static (KillerLoadoutDomain? KillerLoadoutDomain, string? Message) Create(
            int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage,
            IEnumerable<KillerAddonDomain?> killerAddons, IEnumerable<KillerPerkDomain?> killerPerks)
        {
            string message = string.Empty;

            var killer = new KillerLoadoutDomain(idKiller, killerName, killerImage, killerAbilityImage, killerAddons, killerPerks);

            return (killer, message);
        }
    }
}