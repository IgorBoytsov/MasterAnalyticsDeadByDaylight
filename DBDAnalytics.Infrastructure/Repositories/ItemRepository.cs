using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class ItemRepository(Func<DBDContext> context) : IItemRepository
    {
        private readonly Func<DBDContext> _contextFactory= context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string itemName, byte[] itemImage, string itemDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var itemEntity = new Item
                {
                    ItemName = itemName,
                    ItemImage = itemImage,
                    ItemDescription = itemDescription
                };

                await _dbContext.Items.AddAsync(itemEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.Items
                    .Where(x => x.ItemName == itemName)
                        .Select(x => x.IdItem)
                            .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idItem, string itemName, byte[]? itemImage, string? itemDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.Items.FirstOrDefaultAsync(x => x.IdItem == idItem);

                if (entity != null)
                {
                    entity.ItemName = itemName;
                    entity.ItemImage = itemImage;
                    entity.ItemDescription = itemDescription;

                    _dbContext.Items.Update(entity);
                    _dbContext.SaveChanges();

                    return idItem;
                }
                {
                    Debug.WriteLine($"Не удалось обновить Item на уровне базы данных для Id: {idItem}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idItem)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.Items
                            .Where(x => x.IdItem == idItem)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<ItemDomain?> GetAsync(int idItem)
        {
            using (var _dbContext = _contextFactory())
            {
                var itemEntity = await _dbContext.Items.FirstOrDefaultAsync(x => x.IdItem == idItem);

                if (itemEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Item для Id: {idItem}");
                    return null;
                }

                var (CreatedItem, Message) = ItemDomain.Create(
                    itemEntity.IdItem, 
                    itemEntity.ItemName, 
                    itemEntity.ItemImage, 
                    itemEntity.ItemDescription);

                if (CreatedItem == null)
                {
                    Debug.WriteLine($"Не удалось создать KillerDomain для Id: {idItem}. Ошибка: {Message}");
                    return null;
                }

                return CreatedItem;
            }
        }

        public ItemDomain? Get(int idItem)
        {
            return Task.Run(() => GetAsync(idItem)).Result;
        }

        public async Task<IEnumerable<ItemDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var itemsEntity = await _dbContext.Items.ToListAsync();
                var itemsDomain = new List<ItemDomain>();

                foreach (var itemEntity in itemsEntity)
                {
                    var id = itemEntity.IdItem;

                    var (CreatedItem, Message) = ItemDomain.Create(
                        itemEntity.IdItem, 
                        itemEntity.ItemName, 
                        itemEntity.ItemImage, 
                        itemEntity.ItemDescription);

                    if (CreatedItem == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции ItemDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    itemsDomain.Add(CreatedItem);
                }

                return itemsDomain;
            }
        }

        public IEnumerable<ItemDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        public async Task<ItemWithAddonsDomain> GetItemWithAddonsAsync(int idItem)
        {
            using (var _dbContext = _contextFactory())
            {
                var itemEntity = await _dbContext.Items.FirstOrDefaultAsync(x => x.IdItem == idItem);

                if (itemEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить элемент Item из БД с ID: {idItem}.");
                    return null;
                }

                var itemAddonsEntity = await _dbContext.ItemAddons.Where(x => x.IdItem == idItem).ToListAsync();

                var itemAddonsDomainList = new List<ItemAddonDomain>();

                foreach (var itemAddon in itemAddonsEntity)
                {
                    var id = itemAddon.IdItem;

                    var (CreatedItemAddon, MessageItemAddon) = ItemAddonDomain.Create(
                        itemAddon.IdItemAddon,
                        itemAddon.IdItem, 
                        itemAddon.IdRarity,
                        itemAddon.ItemAddonName, 
                        itemAddon.ItemAddonImage,
                        itemAddon.ItemAddonDescription);

                    if (CreatedItemAddon != null)
                    {
                        itemAddonsDomainList.Add(CreatedItemAddon);
                    }
                    else
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции ItemAddonDomain для Id {id} при получение из БД. Ошибка: {MessageItemAddon}");
                    }
                }

                var (CreatedItemWithAddon, Message) = ItemWithAddonsDomain.Create(
                    itemEntity.IdItem, 
                    itemEntity.ItemName, 
                    itemEntity.ItemImage, 
                    itemEntity.ItemDescription, 
                    itemAddonsDomainList);

                if (CreatedItemWithAddon != null)
                {
                    return CreatedItemWithAddon;
                }
                else
                {
                    Debug.WriteLine($"Не удалось создать объект коллекции ItemWithAddonDomain для предмета с ID: {idItem}. Ошибка: {Message}");
                    return null;
                }
            }
        }

        public ItemWithAddonsDomain GetItemWithAddons(int idItem)
        {
            return Task.Run( () => GetItemWithAddonsAsync(idItem)).Result;
        }

        public async Task<IEnumerable<ItemWithAddonsDomain>> GetItemsWithAddonsAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var itemsEntity = await _dbContext.Items.AsNoTracking().ToListAsync();
                var itemAddonsEntity = await _dbContext.ItemAddons.AsNoTracking().ToListAsync();

                var itemsWithAddonsDomainList = new List<ItemWithAddonsDomain>();

                foreach (var item in itemsEntity)
                {
                    if (item == null)
                    {
                        Console.WriteLine($"Не удалось получить объект Item.");
                        continue;
                    }

                    var itemAddonsDomain = new List<ItemAddonDomain>();

                    foreach (var itemAddon in itemAddonsEntity.Where(x => x.IdItem == item.IdItem))
                    {
                        if (itemAddon == null)
                        {
                            Console.WriteLine($"Не удалось получить объект ItemAddon.");
                            continue;
                        }

                        var id = itemAddon.IdItem;

                        var (CreatedItemAddon, Message) = ItemAddonDomain.Create(
                            itemAddon.IdItemAddon, 
                            itemAddon.IdItem, 
                            itemAddon.IdRarity,
                            itemAddon.ItemAddonName, 
                            itemAddon.ItemAddonImage, 
                            itemAddon.ItemAddonDescription);

                        if (CreatedItemAddon == null)
                        {
                            Console.WriteLine($"Не удалось создать объект ItemAddonDomain с ID: {id}. Ошибка: {Message}");
                            continue;
                        }

                        itemAddonsDomain.Add(CreatedItemAddon);
                    }

                    var (CreatedItemWithAddon, MessageItemWithAddons) = ItemWithAddonsDomain.Create(
                        item.IdItem, 
                        item.ItemName, 
                        item.ItemImage, 
                        item.ItemDescription, itemAddonsDomain);

                    if (CreatedItemWithAddon != null)
                    {
                        itemsWithAddonsDomainList.Add(CreatedItemWithAddon);
                    }
                    else
                    {
                        Debug.WriteLine($"Не удалось создать объект ItemWithAddonsDomain.");
                    }
                }

                return itemsWithAddonsDomainList;
            }
        }

        public IEnumerable<ItemWithAddonsDomain> GetItemsWithAddons()
        {
            return Task.Run(() => GetItemsWithAddonsAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string itemName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Items.AnyAsync(x => x.ItemName == itemName);
            }
        }

        public async Task<bool> ExistAsync(int idItem)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Items.AnyAsync(x => x.IdItem == idItem);
            }
        }

        public bool Exist(string itemName)
        {
            return Task.Run(() => ExistAsync(itemName)).Result;
        }

        public bool Exist(int idItem)
        {
            return Task.Run(() => ExistAsync(idItem)).Result;
        }
    }
}