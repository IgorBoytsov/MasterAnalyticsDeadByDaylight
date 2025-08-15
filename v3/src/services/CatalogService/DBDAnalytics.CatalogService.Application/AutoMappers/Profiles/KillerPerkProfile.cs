using AutoMapper;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Killers;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.Profiles
{
    public sealed class KillerPerkProfile : Profile
    {
        public KillerPerkProfile()
        {
            CreateMap<KillerPerk, KillerPerkResponse>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
              .ForMember(dest => dest.OldId, opt => opt.MapFrom(stc => stc.OldId))
              .ForMember(dest => dest.KillerId, opt => opt.MapFrom(stc => stc.KillerId.ToString()))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()))
              .ForMember(dest => dest.ImageKey, opt => opt.MapFrom(src => src.ImageKey != null ? src.ImageKey.ToString() : null));
        }
    }
}