namespace DBDAnalytics.Domain.DomainModels
{
    public class KillerPerkCategoryDomain
    {
        private KillerPerkCategoryDomain(int idKillerPerkCategory, string categoryName)
        {
            IdKillerPerkCategory = idKillerPerkCategory;
            CategoryName = categoryName;
        }

        public int IdKillerPerkCategory { get; private set; }

        public string CategoryName { get; private set; } = null!;

        public static (KillerPerkCategoryDomain? KillerPerkCategoryDTO, string? Message) Create(int idKillerPerkCategory, string categoryName)
        {
            string message = string.Empty;

            if (string.IsNullOrEmpty(categoryName))
                return (null, "Укажите название для категории.");

            var category = new KillerPerkCategoryDomain(idKillerPerkCategory, categoryName);

            return (category, message);
        }
    }
}