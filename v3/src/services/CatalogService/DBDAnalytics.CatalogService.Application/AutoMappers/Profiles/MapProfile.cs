using AutoMapper;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Maps;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.Profiles
{
    public sealed class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Map, MapResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.MeasurementId, opt => opt.MapFrom(src => src.MeasurementId.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.ImageKey, opt => opt.MapFrom(src => src.ImageKey != null ? src.ImageKey.ToString() : null));
        }
    }
}