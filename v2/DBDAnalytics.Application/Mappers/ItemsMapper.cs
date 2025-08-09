using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;
using System.Collections.ObjectModel;

namespace DBDAnalytics.Application.Mappers
{
    internal static class ItemsMapper
    {
        public static ItemDTO ToDTO(this ItemDomain item)
        {
            return new ItemDTO
            {
                IdItem = item.IdItem,
                ItemName = item.ItemName,
                ItemDescription = item.ItemDescription,
                ItemImage = item.ItemImage,
            };
        }

        public static List<ItemDTO> ToDTO(this IEnumerable<ItemDomain> items)
        {
            var list = new List<ItemDTO>();

            foreach (var item in items)
            {
                list.Add(item.ToDTO());
            }

            return list;
        }

        public static ItemWithAddonsDTO ToDTO(this ItemWithAddonsDomain itemWithAddons)
        {

            var itemsAddonDTO = new ObservableCollection<ItemAddonDTO>();

            foreach (var item in itemWithAddons.ItemAddons.ToDTO())
            {
                itemsAddonDTO.Add(item);
            }

            return new ItemWithAddonsDTO
            {
                IdItem = itemWithAddons.IdItem,
                ItemName = itemWithAddons.ItemName,
                ItemImage = itemWithAddons.ItemImage,
                ItemDescription = itemWithAddons.ItemDescription,
                ItemAddons = itemsAddonDTO
            };
        }

        public static List<ItemWithAddonsDTO> ToDTO(this IEnumerable<ItemWithAddonsDomain> itemWithAddons)
        {
            var itemWithAddonsDTO = new List<ItemWithAddonsDTO>();

            foreach (var item in itemWithAddons)
            {
                var itemAddonsDTO = new ObservableCollection<ItemAddonDTO>();

                foreach (var itemAddon in item.ItemAddons.ToDTO())
                {
                    itemAddonsDTO.Add(itemAddon);
                }

                itemWithAddonsDTO.Add(new ItemWithAddonsDTO
                {
                    IdItem = item.IdItem,
                    ItemName = item.ItemName,
                    ItemImage = item.ItemImage,
                    ItemDescription = item.ItemDescription,
                    ItemAddons = itemAddonsDTO,
                });
            }

            return itemWithAddonsDTO;
        }       
    }
}