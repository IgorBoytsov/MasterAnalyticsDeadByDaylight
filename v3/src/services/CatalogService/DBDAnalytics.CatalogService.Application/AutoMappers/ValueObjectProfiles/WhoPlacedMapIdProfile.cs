using AutoMapper;
using DBDAnalytics.CatalogService.Domain.ValueObjects.WhoPlacedMap;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.ValueObjectProfiles
{
    public sealed class WhoPlacedMapIdProfile : Profile
    {
        public WhoPlacedMapIdProfile()
        {
            CreateMap<WhoPlacedMapId, int>().ConvertUsing(src => src.Value);
            CreateMap<WhoPlacedMapId?, int?>().ConvertUsing(src => src.HasValue ? src.Value.Value : (int?)null);
        }
    }
}