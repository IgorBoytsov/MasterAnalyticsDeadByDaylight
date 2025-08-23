using AutoMapper;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.Profiles
{
    public sealed class SurvivorPerkProfile : Profile
    {
        public SurvivorPerkProfile()
        {
            CreateMap<SurvivorPerk, SurvivorPerkResponse>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
              .ForMember(dest => dest.OldId, opt => opt.MapFrom(stc => stc.OldId))
              .ForMember(dest => dest.SurvivorId, opt => opt.MapFrom(stc => stc.SurvivorId.ToString()))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()))
              .ForMember(dest => dest.ImageKey, opt => opt.MapFrom(src => src.ImageKey != null ? src.ImageKey.ToString() : null))
              .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId != null ? src.CategoryId : null));
        }
    }
}