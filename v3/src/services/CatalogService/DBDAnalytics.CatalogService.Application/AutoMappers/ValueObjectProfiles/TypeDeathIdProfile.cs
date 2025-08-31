using AutoMapper;
using DBDAnalytics.CatalogService.Domain.ValueObjects.TypeDeath;

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