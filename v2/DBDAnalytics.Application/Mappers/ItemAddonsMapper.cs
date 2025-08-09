using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class ItemAddonsMapper
    {
        public static ItemAddonDTO ToDTO(this ItemAddonDomain itemAddon)
        {
            return new ItemAddonDTO
            {
                IdItemAddon = itemAddon.IdItemAddon,
                IdItem = itemAddon.IdItem,
                IdRarity = itemAddon.IdRarity,
                ItemAddonName = itemAddon.ItemAddonName,
                ItemAddonImage = itemAddon.ItemAddonImage,
                ItemAddonDescription = itemAddon.ItemAddonDescription,
            };
        }

        public static List<ItemAddonDTO> ToDTO(this IEnumerable<ItemAddonDomain> itemAddons)
        {
            var list = new List<ItemAddonDTO>();

            foreach (var itemAddon in itemAddons)
            {
                list.Add(itemAddon.ToDTO());
            }

            return list;
        }
    }
}