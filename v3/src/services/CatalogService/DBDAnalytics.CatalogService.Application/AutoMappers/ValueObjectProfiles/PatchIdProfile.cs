using AutoMapper;
using DBDAnalytics.Shared.Domain.ValueObjects;

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