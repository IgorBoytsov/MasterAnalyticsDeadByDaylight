using AutoMapper;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Maps;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.Profiles
{
    public sealed class WhoPlacedMapProfile : Profile
    {
        public WhoPlacedMapProfile()
        {
            CreateMap<WhoPlacedMap, WhoPlacedMapResponse>()
                .ForMember(w => w.Id, dest => dest.MapFrom(src => src.Id))
                .ForMember(w => w.OldId, dest => dest.MapFrom(src => src.OldId))
                .ForMember(w => w.Name, dest => dest.MapFrom(src => src.Name.ToString()));
        }
    }
}