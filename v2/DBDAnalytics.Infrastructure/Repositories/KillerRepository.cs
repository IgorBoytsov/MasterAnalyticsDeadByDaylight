using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class KillerRepository(Func<DBDContext> context) : IKillerRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string killerName, byte[]? killerImage, byte[]? killerAbilityImage)
        {
            using (var _dbContext = _contextFactory())
            {
                var killerEntity = new Killer
                {
                    KillerName = killerName,
                    KillerImage = killerImage,
                    KillerAbilityImage = killerAbilityImage
                };

                await _dbContext.Killers.AddAsync(killerEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.Killers
                     .OrderByDescending(x => x.IdKiller)
                            .Select(x => x.IdKiller)
                                .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.Killers.FirstOrDefaultAsync(x => x.IdKiller == idKiller);

                if (entity != null)
                {
                    entity.KillerName = killerName;
                    entity.KillerImage = killerImage;
                    entity.KillerAbilityImage = killerAbilityImage;

                    _dbContext.Killers.Update(entity);
                    _dbContext.SaveChanges();

                    return idKiller;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить Killer на уровне базы данных для Id: {idKiller}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idKiller)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.Killers
                            .Where(x => x.IdKiller == idKiller)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<KillerDomain?> GetAsync(int idKiller)
        {
            using (var _dbContext = _contextFactory())
            {
                var killerEntity = await _dbContext.Killers
                        .FirstOrDefaultAsync(x => x.IdKiller == idKiller);

                if (killerEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Killer для Id: {idKiller}");
                    return null;
                }

                var (CreatedKiller, Message) = KillerDomain.Create(
                    killerEntity.IdKiller,
                    killerEntity.KillerName,
                    killerEntity.KillerImage,
                    killerEntity.KillerAbilityImage);

                if (CreatedKiller == null)
                {
                    Debug.WriteLine($"Не удалось создать KillerDomain для Id: {idKiller}. Ошибка: {Message}");
                    return null;
                }

                return CreatedKiller;
            }
        }

        public async Task<IEnumerable<KillerDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var killersEntities = await _dbContext.Killers.AsNoTracking().ToListAsync();

                var killersDomain = new List<KillerDomain>();

                foreach (var killerEntity in killersEntities)
                {
                    if (killerEntity == null)
                    {
                        Debug.WriteLine($"Не удалось получить элемент Killer из БД");
                        continue;
                    }

                    var id = killerEntity.IdKiller;

                    var (CreatedKiller, Message) = KillerDomain.Create(
                        killerEntity.IdKiller, 
                        killerEntity.KillerName, 
                        killerEntity.KillerImage, 
                        killerEntity.KillerAbilityImage);

                    if (CreatedKiller == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции KillerDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;       
                    }

                    killersDomain.Add(CreatedKiller);
                }

                return killersDomain;
            }
        }

        public IEnumerable<KillerDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        public async Task<KillerLoadoutDomain?> GetKillerWithAddonsAndPerksAsync(int idKiller)
        {
            using (var _dbContext = _contextFactory())
            {
                var killerEntity = await _dbContext.Killers.Where(x => x.IdKiller == idKiller).AsNoTracking().FirstOrDefaultAsync();

                if (killerEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить элемент Killer из БД с ID: {idKiller}.");
                    return null;
                }

                var killerAddonsEntity = await _dbContext.KillerAddons.Where(x => x.IdKiller == idKiller).AsNoTracking().ToListAsync();
                var killerPerksEntity = await _dbContext.KillerPerks.Where(x => x.IdKiller == idKiller).AsNoTracking().ToListAsync();

                var killerAddonsDomainList = new List<KillerAddonDomain>();
                var killerPerksDomainList = new List<KillerPerkDomain>();

                foreach (var killerAddon in killerAddonsEntity)
                {
                    var id = killerAddon.IdKillerAddon;

                    var (CreatedKillerAddon, MessageKillerAddon) = KillerAddonDomain.Create(
                        killerAddon.IdKillerAddon, 
                        killerAddon.IdKiller, 
                        killerAddon.IdRarity,
                        killerAddon.AddonName, 
                        killerAddon.AddonImage, 
                        killerAddon.AddonDescription);

                    if (CreatedKillerAddon != null)
                    {
                        killerAddonsDomainList.Add(CreatedKillerAddon);
                    }
                    else
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции KillerAddonDomain для Id {id} при получение из БД. Ошибка: {MessageKillerAddon}");
                    }

                }

                foreach (var killerPerk in killerPerksEntity)
                {
                    var id = killerPerk.IdKillerPerk;

                    var (CreatedKillerPerk, MessageKillerPerk) = KillerPerkDomain.Create(
                        killerPerk.IdKillerPerk, 
                        killerPerk.IdKiller, 
                        killerPerk.PerkName, 
                        killerPerk.PerkImage, 
                        killerPerk.IdCategory, 
                        killerPerk.PerkDescription);

                    if (CreatedKillerPerk != null)
                    {
                        killerPerksDomainList.Add(CreatedKillerPerk);
                    }
                    else
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции KillerPerkDomain для Id {id} при получение из БД. Ошибка: {MessageKillerPerk}");
                    }
                }

                var (CreatedKillerLoadout, Message) = KillerLoadoutDomain.Create(
                    killerEntity.IdKiller,
                    killerEntity.KillerName,
                    killerEntity.KillerImage,
                    killerEntity.KillerAbilityImage,
                    killerAddonsDomainList, killerPerksDomainList);

                if (CreatedKillerLoadout != null)
                {
                    return CreatedKillerLoadout;
                }
                else
                {
                    Debug.WriteLine($"Не удалось создать объект коллекции KillerLoadoutDomain. Ошибка: {Message}");
                    return null;
                }
            }
        }

        public async Task<IEnumerable<KillerLoadoutDomain>> GetKillersWithAddonsAndPerksAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var killersEntity = await _dbContext.Killers.AsNoTracking().ToListAsync();
                var killerAddonsEntity = await _dbContext.KillerAddons.AsNoTracking().ToListAsync();
                var killerPerksEntity = await _dbContext.KillerPerks.AsNoTracking().ToListAsync();

                var killerLoadoutDomain = new List<KillerLoadoutDomain>();

                foreach (var killer in killersEntity)
                {
                    if (killer == null)
                    {
                        Debug.WriteLine($"Не удалось получить объект Killer.");
                        continue;
                    }

                    var killerAddonsDomain = new List<KillerAddonDomain?>();
                    var killerPerksDomain = new List<KillerPerkDomain?>();

                    foreach (var killerAddon in killerAddonsEntity.Where(x => x.IdKiller == killer.IdKiller))
                    {
                        if (killerAddon == null)
                        {
                            Debug.WriteLine($"Не удалось получить объект KillerAddon.");
                            continue;
                        }

                        var id = killerAddon.IdKillerAddon;

                        var (CreatedKillerAddon, Message) = KillerAddonDomain.Create(
                            killerAddon.IdKillerAddon,
                            killer.IdKiller,
                            killerAddon.IdRarity,
                            killerAddon.AddonName,
                            killerAddon.AddonImage,
                            killerAddon.AddonDescription);

                        if (CreatedKillerAddon == null)
                        {
                            Debug.WriteLine($"Не удалось создать объект KillerAddonDomain с ID: {id}. Ошибка: {Message}");
                            continue;
                        }

                        killerAddonsDomain.Add(CreatedKillerAddon);
                    }

                    foreach (var killerPerk in killerPerksEntity.Where(x => x.IdKiller == killer.IdKiller))
                    {
                        if (killerPerk == null)
                        {
                            Console.WriteLine($"Не удалось получить объект KillerPerk.");
                            continue;
                        }

                        var id = killerPerk.IdKillerPerk;

                        var (CreatedKillerPerk, Message) = KillerPerkDomain.Create(
                            killerPerk.IdKillerPerk,
                            killer.IdKiller,
                            killerPerk.PerkName,
                            killerPerk.PerkImage,
                            killerPerk.IdCategory,
                            killerPerk.PerkDescription);

                        if (CreatedKillerPerk == null)
                        {
                            Debug.WriteLine($"Не удалось создать объект KillerPerkDomain с ID: {id}. Ошибка: {Message}");
                            continue;
                        }

                        killerPerksDomain.Add(CreatedKillerPerk);
                    }

                    var (CreatedKillerLoadout, MessageKillerLoadout) = KillerLoadoutDomain.Create(
                        killer.IdKiller,
                        killer.KillerName,
                        killer.KillerImage,
                        killer.KillerAbilityImage,
                        killerAddonsDomain,
                        killerPerksDomain);

                    if (CreatedKillerLoadout == null)
                    {
                        Debug.WriteLine($"Не удалось создать объект KillerLoadoutDomain. Ошибка: {MessageKillerLoadout}");
                        continue;
                    }

                    killerLoadoutDomain.Add(CreatedKillerLoadout);
                }

                return killerLoadoutDomain;
            }
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(int idKiller)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Killers.AnyAsync(x => x.IdKiller == idKiller);
            }
        }

        public async Task<bool> ExistAsync(string killerName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Killers.AnyAsync(x => x.KillerName == killerName);
            }
        }

        public bool Exist(int idKiller)
        {
            return Task.Run(() => ExistAsync(idKiller)).Result;
        }

        public bool Exist(string killerName)
        {
            return Task.Run(() => ExistAsync(killerName)).Result;
        }
    }
}

//public async Task<IEnumerable<KillerLoadoutDomain?>> GetKillersWithAddonsAndPerksAsync()
//{
//    using (var _dbContext = _contextFactory())
//    {
//        var killersEntity = await _dbContext.Killers.AsNoTracking().ToListAsync();
//        var killerAddonsEntity = await _dbContext.KillerAddons.AsNoTracking().ToListAsync();
//        var killerPerksEntity = await _dbContext.KillerPerks.AsNoTracking().ToListAsync();

//        var killerLoadoutDomain = new List<KillerLoadoutDomain?>();

//        foreach (var item in killersEntity)
//        {
//            if (item != null)
//            {
//                var killerAddonsDomain = new List<KillerAddonDomain?>();
//                var killerPerksDomain = new List<KillerPerkDomain?>();

//                // TODO : Стоит проверять создания KillerAddon, KillerPerk, KillerLoadout на null
//                killerAddonsDomain.AddRange(killerAddonsEntity.Where(x => x.IdKiller == item.IdKiller)
//                                    .Select(x => 
//                                        KillerAddonDomain.Create(
//                                            x.IdKillerAddon, 
//                                            x.IdKiller, 
//                                            x.IdRarity, 
//                                            x.AddonName,
//                                            x.AddonImage, 
//                                            x.AddonDescription).KillerAddonDomain));

//                killerPerksDomain.AddRange(killerPerksEntity.Where(x => x.IdKiller == item.IdKiller)
//                                    .Select(x => 
//                                        KillerPerkDomain.Create(
//                                            x.IdKillerPerk, 
//                                            x.IdKiller, 
//                                            x.PerkName, 
//                                            x.PerkImage, 
//                                            x.IdCategory, 
//                                            x.PerkDescription).KillerPerkDomain));

//                killerLoadoutDomain.Add(KillerLoadoutDomain.Create(
//                    item.IdKiller, 
//                    item.KillerName, 
//                    item.KillerImage, 
//                    item.KillerAbilityImage, 
//                    killerAddonsDomain, 
//                    killerPerksDomain).KillerLoadoutDomain);
//            }
//            else
//            {
//                Console.WriteLine($"Не удалось получить объект Killer.");
//            }
//        }

//        return killerLoadoutDomain;
//    }
//}