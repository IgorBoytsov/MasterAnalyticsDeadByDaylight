using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class KillerAddonRepository(Func<DBDContext> context) : IKillerAddonRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var killerAddonEntity = new KillerAddon
                {
                    IdKiller = idKiller,
                    IdRarity = idRarity,
                    AddonName = addonName,
                    AddonImage = addonImage,
                    AddonDescription = addonDescription
                };

                await _dbContext.KillerAddons.AddAsync(killerAddonEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.KillerAddons
                    .Where(x => x.IdKiller == idKiller)
                        .OrderByDescending(x => x.IdKillerAddon)
                            .Select(x => x.IdKillerAddon)
                                .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idAddon, int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.KillerAddons.FirstOrDefaultAsync(x => x.IdKillerAddon == idAddon);

                if (entity != null)
                {
                    entity.IdRarity = idRarity;
                    entity.AddonName = addonName;
                    entity.AddonImage = addonImage;
                    entity.AddonDescription = addonDescription;

                    _dbContext.KillerAddons.Update(entity);
                    _dbContext.SaveChanges();

                    return idAddon;
                }
                {
                    Debug.WriteLine($"Не удалось обновить KillerAddon на уровне базы данных для Id: {idAddon}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idKillerAddon)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.KillerAddons
                            .Where(x => x.IdKillerAddon == idKillerAddon)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<KillerAddonDomain?> GetAsync(int idKillerAddon)
        {
            using (var _dbContext = _contextFactory())
            {
                var killerAddonEntity = await _dbContext.KillerAddons.FirstOrDefaultAsync(x => x.IdKillerAddon == idKillerAddon);

                if (killerAddonEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить KillerAddon для Id: {idKillerAddon}");
                    return null;
                }

                var (CreatedKillerAddon, Message) = KillerAddonDomain.Create(
                    killerAddonEntity.IdKillerAddon, 
                    killerAddonEntity.IdKiller, 
                    killerAddonEntity.IdRarity,
                    killerAddonEntity.AddonName, 
                    killerAddonEntity.AddonImage, 
                    killerAddonEntity.AddonDescription);

                if (CreatedKillerAddon == null)
                {
                    Debug.WriteLine($"Не удалось создать KillerDomain для Id: {idKillerAddon}. Ошибка: {Message}");
                    return null;
                }

                return CreatedKillerAddon;
            }
        }

        public KillerAddonDomain? Get(int idKillerAddon)
        {
            return Task.Run(() => GetAsync(idKillerAddon)).Result;
        }

        public async Task<IEnumerable<KillerAddonDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var killerAddonsEntity = await _dbContext.KillerAddons.ToListAsync();
                var killerAddonsDomain = new List<KillerAddonDomain>();

                foreach (var killerAddonEntity in killerAddonsEntity)
                {
                    var id = killerAddonEntity.IdKillerAddon;

                    var (CreatedKillerAddon, Message) = KillerAddonDomain.Create(
                         killerAddonEntity.IdKillerAddon, 
                         killerAddonEntity.IdKiller,
                         killerAddonEntity.IdRarity,
                         killerAddonEntity.AddonName, 
                         killerAddonEntity.AddonImage,
                         killerAddonEntity.AddonDescription);

                    if (CreatedKillerAddon == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции KillerAddonDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    killerAddonsDomain.Add(CreatedKillerAddon);
                }

                return killerAddonsDomain;
            }
        }

        public IEnumerable<KillerAddonDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        public async Task<List<KillerAddonDomain>> GetAllByIdKiller(int idKiller)
        {
            using (var _dbContext = _contextFactory())
            {
                var killerAddonsEntity = _dbContext.KillerAddons
                    .AsNoTracking()
                        .Where(x => x.IdKiller == idKiller)
                            .AsQueryable();

                var killerAddonsDomain = (await killerAddonsEntity.ToListAsync())
                    .Select(x =>
                    {
                        var result = KillerAddonDomain.Create(
                            x.IdKillerAddon,
                            x.IdKiller,
                            x.IdRarity,
                            x.AddonName,
                            x.AddonImage,
                            x.AddonDescription);

                        if (result.KillerAddonDomain is null)
                        {
                            Debug.WriteLine($"Не удалось создать элемент коллекции KillerAddonDomain для Id {x.IdKillerAddon} при получение из БД. Ошибка: {result.Message}");
                        }

                        return result.KillerAddonDomain;
                    })
                    .Where(x => x != null)
                    .ToList();

                return killerAddonsDomain;
            }
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(int idKillerAddon)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.KillerAddons.AnyAsync(x => x.IdKillerAddon == idKillerAddon);
            }
        }

        public bool Exist(int idKillerAddon)
        {
            return Task.Run(() => ExistAsync(idKillerAddon)).Result;
        }
    }
}