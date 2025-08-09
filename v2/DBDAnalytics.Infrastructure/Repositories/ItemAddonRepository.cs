using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class ItemAddonRepository(Func<DBDContext> context) : IItemAddonRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var itemAddonEntity = new ItemAddon
                {
                    IdItem = idItem,
                    IdRarity = idRarity,
                    ItemAddonName = itemAddonName,
                    ItemAddonImage = itemAddonImage,
                    ItemAddonDescription = itemAddonDescription
                };

                await _dbContext.ItemAddons.AddAsync(itemAddonEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.ItemAddons
                    .Where(x => x.IdItem == idItem)
                        .OrderByDescending(x => x.IdItemAddon)
                            .Select(x => x.IdItemAddon)
                                .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idItemAddon, int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.ItemAddons.FirstOrDefaultAsync(x => x.IdItemAddon == idItemAddon);

                if (entity != null)
                {
                    entity.IdItem = idItem;
                    entity.IdRarity = idRarity;
                    entity.ItemAddonName = itemAddonName;
                    entity.ItemAddonImage = itemAddonImage;
                    entity.ItemAddonDescription = itemAddonDescription;

                    _dbContext.ItemAddons.Update(entity);
                    _dbContext.SaveChanges();

                    return idItemAddon;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить ItemAddon на уровне базы данных для Id: {idItemAddon}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idItemAddon)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.ItemAddons
                            .Where(x => x.IdItemAddon == idItemAddon)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<ItemAddonDomain?> GetAsync(int idItemAddon)
        {
            using (var _dbContext = _contextFactory())
            {
                var itemEntity = await _dbContext.ItemAddons.FirstOrDefaultAsync(x => x.IdItemAddon == idItemAddon);

                if (itemEntity == null)
                {
                    Console.WriteLine($"Не удалось получить ItemAddon для Id: {idItemAddon}");
                    return null;
                }

                var (CreatedItemAddon, Message) = ItemAddonDomain.Create(
                    itemEntity.IdItemAddon,
                    itemEntity.IdItem, 
                    itemEntity.IdRarity, 
                    itemEntity.ItemAddonName, 
                    itemEntity.ItemAddonImage, 
                    itemEntity.ItemAddonDescription);

                if (CreatedItemAddon == null)
                {
                    Debug.WriteLine($"Не удалось создать ItemAddonDomain для Id: {idItemAddon}. Ошибка: {Message}");
                    return null;
                }

                return CreatedItemAddon;
            }
        }

        public ItemAddonDomain? Get(int idItemAddon)
        {
            return Task.Run(() => GetAsync(idItemAddon)).Result;
        }

        public async Task<IEnumerable<ItemAddonDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var itemAddonsEntity = await _dbContext.ItemAddons.ToListAsync();
                var itemAddonsDomain = new List<ItemAddonDomain>();

                foreach (var itemAddonEntity in itemAddonsEntity)
                {
                    var id = itemAddonEntity.IdItemAddon;

                    var (CreatedItemAddon, Message) = ItemAddonDomain.Create(
                        itemAddonEntity.IdItemAddon, 
                        itemAddonEntity.IdItem, 
                        itemAddonEntity.IdRarity,
                        itemAddonEntity.ItemAddonName, 
                        itemAddonEntity.ItemAddonImage, 
                        itemAddonEntity.ItemAddonDescription);

                    if (CreatedItemAddon == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции ItemAddonDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    itemAddonsDomain.Add(CreatedItemAddon);
                }

                return itemAddonsDomain;
            }
        }

        public IEnumerable<ItemAddonDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string itemAddonName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.ItemAddons.AnyAsync(x => x.ItemAddonName == itemAddonName);
            }
        }

        public async Task<bool> ExistAsync(int idItemAddon)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.ItemAddons.AnyAsync(x => x.IdItemAddon == idItemAddon);
            }
        }

        public bool Exist(string itemAddonName)
        {
            return Task.Run(() => ExistAsync(itemAddonName)).Result;
        }

        public bool Exist(int idItemAddon)
        {
            return Task.Run(() => ExistAsync(idItemAddon)).Result;
        }
    }
}