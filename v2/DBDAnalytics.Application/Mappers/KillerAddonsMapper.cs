using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class KillerAddonsMapper
    {
        public static KillerAddonDTO? ToDTO(this KillerAddonDomain? killerAddon)
        {
            return new KillerAddonDTO
            {
                IdKillerAddon = killerAddon.IdKillerAddon,
                IdKiller = killerAddon.IdKiller,
                IdRarity = killerAddon.IdRarity,
                AddonName = killerAddon.AddonName,
                AddonImage = killerAddon.AddonImage,
                AddonDescription = killerAddon.AddonDescription,
            };
        }

        public static List<KillerAddonDTO?> ToDTO(this IEnumerable<KillerAddonDomain?> killerAddons)
        {
            var list = new List<KillerAddonDTO>();

            foreach (var killerAddon in killerAddons)
            {
                list.Add(killerAddon.ToDTO());
            }

            return list;
        }
    }
}