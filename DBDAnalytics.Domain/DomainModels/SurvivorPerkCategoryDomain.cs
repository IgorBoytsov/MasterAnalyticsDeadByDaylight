namespace DBDAnalytics.Domain.DomainModels
{
    public class SurvivorPerkCategoryDomain
    {
        private SurvivorPerkCategoryDomain(int idSurvivorPerkCategory, string categoryName)
        {
            IdSurvivorPerkCategory = idSurvivorPerkCategory;
            CategoryName = categoryName;
        }

        public int IdSurvivorPerkCategory { get; private set; }

        public string CategoryName { get; private set; } = null!;

        public static (SurvivorPerkCategoryDomain? SurvivorPerkCategoryDomain, string? Message) Create(int idSurvivorPerkCategory, string categoryName)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return (null, "Укажите название категории.");
            }
               
            var category = new SurvivorPerkCategoryDomain(idSurvivorPerkCategory, categoryName);

            return (category, message);
        }
    }
}