using AutoMapper;
using DBDAnalytics.CatalogService.Domain.ValueObjects.PlayerAssociation;

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