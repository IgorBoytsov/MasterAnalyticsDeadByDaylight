using AutoMapper;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Offering;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.Profiles
{
    public sealed class OfferingProfile : Profile
    {
        public OfferingProfile()
        {
            CreateMap<Offering, OfferingResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()))
                .ForMember(dest => dest.ImageKey, opt => opt.MapFrom(src => src.ImageKey != null ? src.ImageKey.ToString() : null))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.RarityId, opt => opt.MapFrom(src => src.RarityId.HasValue ? src.RarityId.Value.Value : (int?)null))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId.HasValue ? src.CategoryId.Value.Value : (int?)null));
        }
    }
}