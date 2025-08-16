using AutoMapper;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Match;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.Profiles
{
    public sealed class GameEventProfile : Profile
    {
        public GameEventProfile()
        {
            CreateMap<GameEvent, GameEventResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OldId, opt => opt.MapFrom(src => src.OldId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()));
        }
    }
}