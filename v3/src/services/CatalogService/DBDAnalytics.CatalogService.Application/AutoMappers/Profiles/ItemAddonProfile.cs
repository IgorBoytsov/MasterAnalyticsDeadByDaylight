using AutoMapper;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.Profiles
{
    public sealed class ItemAddonProfile : Profile
    {
        public ItemAddonProfile()
        {
            CreateMap<ItemAddon, ItemAddonResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OldId, opt => opt.MapFrom(src => src.OldId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()))
                .ForMember(dest => dest.RarityId, opt => opt.MapFrom(src => src.RarityId.HasValue ? src.RarityId.Value.Value : (int?)null))
                .ForMember(dest => dest.ImageKey, opt => opt.MapFrom(src => src.ImageKey != null ? src.ImageKey.ToString() : null));
        }
    }
}