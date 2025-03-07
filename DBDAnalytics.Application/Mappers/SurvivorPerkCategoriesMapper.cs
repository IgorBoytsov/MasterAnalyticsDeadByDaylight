using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class SurvivorPerkCategoriesMapper
    {
        public static SurvivorPerkCategoryDTO ToDTO(this SurvivorPerkCategoryDomain survivorPerkCategory)
        {
            return new SurvivorPerkCategoryDTO
            {
                IdSurvivorPerkCategory = survivorPerkCategory.IdSurvivorPerkCategory,
                CategoryName = survivorPerkCategory.CategoryName,
            };
        }

        public static List<SurvivorPerkCategoryDTO> ToDTO(this IEnumerable<SurvivorPerkCategoryDomain> survivorPerkCategories)
        {
            var list = new List<SurvivorPerkCategoryDTO>();

            foreach (var survivorPerkCategory in survivorPerkCategories)
            {
                list.Add(survivorPerkCategory.ToDTO());
            }

            return list;
        }
    }
}