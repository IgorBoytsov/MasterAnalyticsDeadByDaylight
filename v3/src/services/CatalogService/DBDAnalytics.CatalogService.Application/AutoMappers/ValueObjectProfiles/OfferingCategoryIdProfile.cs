using AutoMapper;
using DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.ValueObjectProfiles
{
    public sealed class OfferingCategoryIdProfile : Profile
    {
        public OfferingCategoryIdProfile()
        {
            CreateMap<OfferingCategoryId, int>().ConvertUsing(src => src.Value);
            CreateMap<OfferingCategoryId?, int?>().ConvertUsing(src => src.HasValue ? src.Value.Value : (int?)null);
        }
    }
}