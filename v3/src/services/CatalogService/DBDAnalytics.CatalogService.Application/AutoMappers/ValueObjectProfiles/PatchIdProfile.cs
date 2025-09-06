using AutoMapper;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Patch;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.ValueObjectProfiles
{
    public sealed class PatchIdProfile : Profile
    {
        public PatchIdProfile()
        {
            CreateMap<PatchId, int>().ConvertUsing(src => src.Value);
            CreateMap<PatchId?, int?>().ConvertUsing(src => src.HasValue ? src.Value.Value : (int?)null);
        }
    }
}