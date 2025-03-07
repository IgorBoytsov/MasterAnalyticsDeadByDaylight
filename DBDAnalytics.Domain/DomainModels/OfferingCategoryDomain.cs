namespace DBDAnalytics.Domain.DomainModels
{
    public class OfferingCategoryDomain
    {
        private OfferingCategoryDomain(int idCategory, string categoryName)
        {
            IdCategory = idCategory;
            CategoryName = categoryName;
        }

        public int IdCategory { get; private set; }

        public string CategoryName { get; private set; } = null!;


        public static (OfferingCategoryDomain? OfferingCategoryDomain, string? Message) Create(int idCategory, string categoryName)
        {
            string message = string.Empty;
            const int MaxCategoryNameLength = 100;

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return (null, "Укажите название категории.");
            }

            if (categoryName.Length > MaxCategoryNameLength)
            {
                return (null, $"Максимально допустимая длинна названия категории - {MaxCategoryNameLength}");
            }

            var category = new OfferingCategoryDomain(idCategory, categoryName);

            return (category, message);
        }
    }
}