using AutoMapper;
using DBDAnalytics.CatalogService.Domain.ValueObjects.GameMode;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.ValueObjectProfiles
{
    public sealed class GameModeIdProfile : Profile
    {
        public GameModeIdProfile()
        {
            CreateMap<GameModeId, int>().ConvertUsing(src => src.Value);
            CreateMap<GameModeId?, int?>().ConvertUsing(src => src.HasValue ? src.Value.Value : (int?)null);
        }
    }
}