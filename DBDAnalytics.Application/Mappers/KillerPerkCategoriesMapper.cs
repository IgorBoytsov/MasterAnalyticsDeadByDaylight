using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class KillerPerkCategoriesMapper
    {
        public static KillerPerkCategoryDTO ToDTO(this KillerPerkCategoryDomain killerPerkCategory)
        {
            return new KillerPerkCategoryDTO
            {
                IdKillerPerkCategory = killerPerkCategory.IdKillerPerkCategory,
                CategoryName = killerPerkCategory.CategoryName,
            };
        }

        public static List<KillerPerkCategoryDTO> ToDTO(this IEnumerable<KillerPerkCategoryDomain> killerPerkCategories)
        {
            var list = new List<KillerPerkCategoryDTO>();

            foreach (var killerPerkCategory in killerPerkCategories)
            {
                list.Add(killerPerkCategory.ToDTO());
            }

            return list;
        }
    }
}