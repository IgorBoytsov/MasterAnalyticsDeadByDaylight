namespace DBDAnalytics.Domain.DomainModels
{
    public class RarityDomain
    {
        private RarityDomain(int idRarity, string rarityName)
        {
            IdRarity = idRarity;
            RarityName = rarityName;
        }
        public int IdRarity { get; private set; }

        public string RarityName { get; private set; } = null!;

        public static (RarityDomain? RarityDomain, string? Message) Create(int idRarity, string rarityName)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(rarityName))
                return (null, "Вы не дали названия качеству.");

            var association = new RarityDomain(idRarity, rarityName);

            return (association, message);
        }
    }
}