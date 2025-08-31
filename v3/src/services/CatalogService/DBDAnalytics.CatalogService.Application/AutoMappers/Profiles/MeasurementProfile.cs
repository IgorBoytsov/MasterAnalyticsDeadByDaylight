using AutoMapper;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Maps;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.Profiles
{
    public sealed class MeasurementProfile : Profile
    {
        public MeasurementProfile()
        {
            CreateMap<Measurement, MeasurementResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.OldId, opt => opt.MapFrom(src => src.OldId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

            CreateMap<Measurement, MeasurementSoloResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.OldId, opt => opt.MapFrom(src => src.OldId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));
        }
    }
}