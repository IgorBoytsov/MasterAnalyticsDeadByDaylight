namespace DBDAnalytics.Domain.DomainModels
{
    public class SurvivorPerkCategoryDomain
    {
        private SurvivorPerkCategoryDomain(int idSurvivorPerkCategory, string categoryName, string? description)
        {
            IdSurvivorPerkCategory = idSurvivorPerkCategory;
            CategoryName = categoryName;
            Description = description;
        }

        public int IdSurvivorPerkCategory { get; private set; }

        public string CategoryName { get; private set; } = null!;

        public string? Description { get; private set; }

        public static (SurvivorPerkCategoryDomain? SurvivorPerkCategoryDomain, string? Message) Create(int idSurvivorPerkCategory, string categoryName, string? description)
        {
            string message = string.Empty;
            const int MaxDescriptionLength = 20000;

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return (null, "Укажите название категории.");
            }

            if (description?.Length > MaxDescriptionLength)
            {
                return (null, $"Максимально допустимая длинна описания - {MaxDescriptionLength}");
            }
               
            var category = new SurvivorPerkCategoryDomain(idSurvivorPerkCategory, categoryName, description);

            return (category, message);
        }
    }
}