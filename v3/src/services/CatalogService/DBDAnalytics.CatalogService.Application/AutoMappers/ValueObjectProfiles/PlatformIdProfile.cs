using AutoMapper;
using DBDAnalytics.Shared.Domain.ValueObjects;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.ValueObjectProfiles
{
    public sealed class PlatformIdProfile : Profile
    {
        public PlatformIdProfile()
        {
            CreateMap<PlatformId, int>().ConvertUsing(src => src.Value);
            CreateMap<PlatformId?, int?>().ConvertUsing(src => src.HasValue ? src.Value.Value : (int?)null);
        }
    }
}