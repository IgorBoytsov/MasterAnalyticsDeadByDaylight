using AutoMapper;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Killers;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.Profiles
{
    public sealed class KillerPerkCategoryProfile : Profile
    {
        public KillerPerkCategoryProfile()
        {
            CreateMap<KillerPerkCategory, KillerPerkCategoryResponse>()
                .ForMember(dest => dest.OldId, opt => opt.MapFrom(src => src.OldId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()));
        }
    }
}