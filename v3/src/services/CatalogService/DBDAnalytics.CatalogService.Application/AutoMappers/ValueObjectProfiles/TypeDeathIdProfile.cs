using AutoMapper;
using DBDAnalytics.Shared.Domain.ValueObjects;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.ValueObjectProfiles
{
    public sealed class TypeDeathIdProfile : Profile
    {
        public TypeDeathIdProfile()
        {
            CreateMap<TypeDeathId, int>().ConvertUsing(src => src.Value);
            CreateMap<TypeDeathId?, int?>().ConvertUsing(src => src.HasValue ? src.Value.Value : (int?)null);
        }
    }
}