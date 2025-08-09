namespace DBDAnalytics.Domain.DomainModels
{
    public class RarityDomain
    {
        private RarityDomain(int idRarity, string rarityName, string? description)
        {
            IdRarity = idRarity;
            RarityName = rarityName;
            Description = description;
        }
        public int IdRarity { get; private set; }

        public string RarityName { get; private set; } = null!;

        public string? Description { get; private set; }

        public static (RarityDomain? RarityDomain, string? Message) Create(int idRarity, string rarityName, string? description)
        {
            string message = string.Empty;
            const int MaxDescriptionLength = 200;

            if (string.IsNullOrWhiteSpace(rarityName))
                return (null, "Вы не дали названия качеству.");

            if (description?.Length > MaxDescriptionLength)
            {
                return (null, $"Максимально допустимая длинна описания - {MaxDescriptionLength}");
            }

            var association = new RarityDomain(idRarity, rarityName, description);

            return (association, message);
        }
    }
}