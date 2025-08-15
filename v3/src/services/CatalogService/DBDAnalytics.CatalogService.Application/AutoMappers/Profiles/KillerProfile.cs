using AutoMapper;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Killers;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.Profiles
{
    public sealed class KillerProfile : Profile
    {
        public KillerProfile()
        {
            CreateMap<Killer, KillerResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()))
                .ForMember(dest => dest.KillerImageKey, opt => opt.MapFrom(src => src.KillerImageKey != null ? src.KillerImageKey.ToString() : null))
                .ForMember(dest => dest.AbilityImageKey, opt => opt.MapFrom(src => src.AbilityImageKey != null ? src.AbilityImageKey.ToString() : null));
        }
    }
}