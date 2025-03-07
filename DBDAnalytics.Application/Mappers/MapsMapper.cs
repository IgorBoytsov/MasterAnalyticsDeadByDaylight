using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class MapsMapper
    {
        public static MapDTO ToDTO(this MapDomain map)
        {
            return new MapDTO
            {
                IdMap = map.IdMap,
                IdMeasurement = map.IdMeasurement,
                MapName = map.MapName,
                MapDescription = map.MapDescription,
                MapImage = map.MapImage,
            };
        }

        public static List<MapDTO> ToDTO(this IEnumerable<MapDomain> maps)
        {
            var list = new List<MapDTO>();

            foreach (var map in maps)
            {
                list.Add(map.ToDTO());
            }

            return list;
        }
    }
}