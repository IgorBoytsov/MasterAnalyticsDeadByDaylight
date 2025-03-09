using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class RaritiesMapper
    {
        public static RarityDTO ToDTO(this RarityDomain rarity)
        {
            return new RarityDTO
            {
                IdRarity = rarity.IdRarity,
                RarityName = rarity.RarityName,
                Description = rarity.Description,
            };
        }

        public static List<RarityDTO> ToDTO(this IEnumerable<RarityDomain> rarities)
        {
            var list = new List<RarityDTO>();

            foreach (var rarity in rarities)
            {
                list.Add(rarity.ToDTO());
            }

            return list;
        }
    }
}