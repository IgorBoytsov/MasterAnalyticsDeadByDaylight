using AutoMapper;
using DBDAnalytics.Shared.Domain.ValueObjects;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.ValueObjectProfiles
{
    public sealed class PlayerAssociationIdProfile : Profile
    {
        public PlayerAssociationIdProfile()
        {
            CreateMap<PlayerAssociationId, int>().ConvertUsing(src => src.Value);
            CreateMap<PlayerAssociationId?, int?>().ConvertUsing(src => src.HasValue ? src.Value.Value : (int?)null);
        }
    }
}