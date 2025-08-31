using AutoMapper;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.ValueObjectProfiles
{
    public sealed class SurvivorPerkCategoryIdProfile : Profile
    {
        public SurvivorPerkCategoryIdProfile()
        {
            CreateMap<SurvivorPerkCategoryId, int>().ConvertUsing(src => src.Value);
            CreateMap<SurvivorPerkCategoryId?, int?>().ConvertUsing(src => src.HasValue ? src.Value.Value : (int?)null);
        }
    }
}