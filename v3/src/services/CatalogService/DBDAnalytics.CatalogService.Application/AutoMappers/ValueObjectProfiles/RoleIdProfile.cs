using AutoMapper;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Role;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.ValueObjectProfiles
{
    public sealed class RoleIdProfile : Profile
    {
        public RoleIdProfile()
        {
            CreateMap<RoleId, int>().ConvertUsing(src => src.Value);
            CreateMap<RoleId?, int?>().ConvertUsing(src => src.HasValue ? src.Value.Value : (int?)null);
        }
    }
}