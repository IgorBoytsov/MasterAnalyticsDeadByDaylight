using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class OfferingCategoriesMapper
    {
        public static OfferingCategoryDTO ToDTO(this OfferingCategoryDomain offeringCategory)
        {
            return new OfferingCategoryDTO
            {
                IdCategory = offeringCategory.IdCategory,
                CategoryName = offeringCategory.CategoryName,
                Description = offeringCategory.Description,
            };
        }

        public static List<OfferingCategoryDTO> ToDTO(this IEnumerable<OfferingCategoryDomain> offeringCategories)
        {
            var list = new List<OfferingCategoryDTO>();

            foreach (var offeringCategory in offeringCategories)
            {
                list.Add(offeringCategory.ToDTO());
            }

            return list;
        }
    }
}