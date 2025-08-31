using AutoMapper;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.ValueObjectProfiles
{
    public sealed class RarityIdProfile : Profile
    {
        public RarityIdProfile()
        {
            CreateMap<RarityId, int>().ConvertUsing(src => src.Value);
            CreateMap<RarityId?, int?>().ConvertUsing(src => src.HasValue ? src.Value.Value : (int?)null);
        }
    }
}