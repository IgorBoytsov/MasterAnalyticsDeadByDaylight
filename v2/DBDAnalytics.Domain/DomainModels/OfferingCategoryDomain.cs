namespace DBDAnalytics.Domain.DomainModels
{
    public class OfferingCategoryDomain
    {
        private OfferingCategoryDomain(int idCategory, string categoryName, string? description)
        {
            IdCategory = idCategory;
            CategoryName = categoryName;
            Description = description;
        }

        public int IdCategory { get; private set; }

        public string CategoryName { get; private set; } = null!;

        public string? Description { get; set; }

        public static (OfferingCategoryDomain? OfferingCategoryDomain, string? Message) Create(int idCategory, string categoryName, string? description)
        {
            string message = string.Empty;
            const int MaxCategoryNameLength = 100;
            const int MaxCategoryDescriptionLength = 2000;

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return (null, "Укажите название категории.");
            }

            if (categoryName.Length > MaxCategoryNameLength)
            {
                return (null, $"Максимально допустимая длинна названия категории - {MaxCategoryNameLength}");
            } 
            
            if (description?.Length > MaxCategoryDescriptionLength)
            {
                return (null, $"Максимально допустимая длинна описания категории - {MaxCategoryDescriptionLength}");
            }

            var category = new OfferingCategoryDomain(idCategory, categoryName, description);

            return (category, message);
        }
    }
}