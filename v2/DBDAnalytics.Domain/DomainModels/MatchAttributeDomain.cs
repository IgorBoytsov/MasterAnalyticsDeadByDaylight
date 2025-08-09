namespace DBDAnalytics.Domain.DomainModels
{
    public class MatchAttributeDomain
    {
        private MatchAttributeDomain(int idMatchAttribute, string attributeName, string? attributeDescription, DateTime createdAt, bool isHide)
        {
            IdMatchAttribute = idMatchAttribute;
            AttributeName = attributeName;
            AttributeDescription = attributeDescription;
            CreatedAt = createdAt;
            IsHide = isHide;
        }

        public int IdMatchAttribute { get; private set; }

        public string AttributeName { get; private set; } = null!;

        public string? AttributeDescription { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public bool IsHide { get; private set; }

        public static (MatchAttributeDomain? MatchAttributeDomain, string Message) Create(int idMatchAttribute, string attributeName, string? attributeDescription, DateTime createdAt, bool isHide)
        {
            string message = string.Empty;

            var matchAttributeDomain = new MatchAttributeDomain(idMatchAttribute, attributeName, attributeDescription, createdAt, isHide);

            return (matchAttributeDomain, message);
        }
    }
}