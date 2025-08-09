using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class WhoPlacedMapsMapper
    {
        public static WhoPlacedMapDTO ToDTO(this WhoPlacedMapDomain whoPlacedMap)
        {
            return new WhoPlacedMapDTO
            {
                IdWhoPlacedMap = whoPlacedMap.IdWhoPlacedMap,
                WhoPlacedMapName = whoPlacedMap.WhoPlacedMapName,
                Description = whoPlacedMap.Description,
            };
        }

        public static List<WhoPlacedMapDTO> ToDTO(this IEnumerable<WhoPlacedMapDomain> whoPlacedMaps)
        {
            var list = new List<WhoPlacedMapDTO>();

            foreach (var whoPlacedMap in whoPlacedMaps)
            {
                list.Add(whoPlacedMap.ToDTO());
            }

            return list;
        }
    }
}