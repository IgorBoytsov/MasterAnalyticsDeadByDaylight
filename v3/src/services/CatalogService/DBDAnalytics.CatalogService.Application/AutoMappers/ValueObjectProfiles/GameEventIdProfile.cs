using AutoMapper;
using DBDAnalytics.Shared.Domain.ValueObjects;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.ValueObjectProfiles
{
    public sealed class GameEventIdProfile : Profile
    {
        public GameEventIdProfile()
        {
            CreateMap<GameEventId, int>().ConvertUsing(src => src.Value);
            CreateMap<GameEventId?, int?>().ConvertUsing(src => src.HasValue ? src.Value.Value : (int?)null);
        }
    }
}