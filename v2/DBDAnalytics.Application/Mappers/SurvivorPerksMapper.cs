using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class SurvivorPerksMapper
    {
        public static SurvivorPerkDTO ToDTO(this SurvivorPerkDomain survivorPerk)
        {
            return new SurvivorPerkDTO
            {
                IdCategory = survivorPerk.IdCategory,
                IdSurvivor = survivorPerk.IdSurvivor,
                IdSurvivorPerk = survivorPerk.IdSurvivorPerk,
                PerkImage = survivorPerk.PerkImage,
                PerkDescription = survivorPerk.PerkDescription,
                PerkName = survivorPerk.PerkName,
            };
        }

        public static List<SurvivorPerkDTO> ToDTO(this IEnumerable<SurvivorPerkDomain> survivorPerks)
        {
            var list = new List<SurvivorPerkDTO>();

            foreach (var survivorPerk in survivorPerks)
            {
                list.Add(survivorPerk.ToDTO());
            }

            return list;
        }
    }
}