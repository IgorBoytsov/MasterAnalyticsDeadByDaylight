using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class KillerPerksMapper
    {
        public static KillerPerkDTO ToDTO(this KillerPerkDomain killerPerk)
        {
            return new KillerPerkDTO
            {
                IdCategory = killerPerk.IdCategory,
                IdKiller = killerPerk.IdKiller,
                IdKillerPerk = killerPerk.IdKillerPerk,
                PerkDescription = killerPerk.PerkDescription,
                PerkImage = killerPerk.PerkImage,
                PerkName = killerPerk.PerkName,
            };
        }

        public static List<KillerPerkDTO> ToDTO(this IEnumerable<KillerPerkDomain> killerPerks)
        {
            var list = new List<KillerPerkDTO>();

            foreach (var killerPerk in killerPerks)
            {
                list.Add(killerPerk.ToDTO());
            }

            return list;
        }
    }
}