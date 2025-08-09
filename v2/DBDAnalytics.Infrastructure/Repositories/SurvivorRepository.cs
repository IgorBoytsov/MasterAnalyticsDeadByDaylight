using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class SurvivorRepository(Func<DBDContext> context) : ISurvivorRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        public async Task<int> CreateAsync(string survivorName, byte[]? survivorImage, string? survivorDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var survivorEntity = new Survivor
                {
                    SurvivorName = survivorName,
                    SurvivorImage = survivorImage,
                    SurvivorDescription = survivorDescription
                };

                await _dbContext.Survivors.AddAsync(survivorEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.Survivors
                     .OrderByDescending(x => x.IdSurvivor)
                            .Select(x => x.IdSurvivor)
                                .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.Survivors.FirstOrDefaultAsync(x => x.IdSurvivor == idSurvivor);

                if (entity != null)
                {
                    entity.SurvivorName = survivorName;
                    entity.SurvivorImage = survivorImage;
                    entity.SurvivorDescription = survivorDescription;

                    _dbContext.Survivors.Update(entity);
                    _dbContext.SaveChanges();

                    return idSurvivor;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить Killer на уровне базы данных для Id: {idSurvivor}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idSurvivor)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.Survivors
                            .Where(x => x.IdSurvivor == idSurvivor)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<SurvivorDomain?> GetAsync(int idSurvivor)
        {
            using (var _dbContext = _contextFactory())
            {
                var survivorEntity = await _dbContext.Survivors
                        .FirstOrDefaultAsync(x => x.IdSurvivor == idSurvivor);

                if (survivorEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Survivor для Id: {idSurvivor}");
                    return null;
                }

                var (CreatedSurvivor, Message) = SurvivorDomain.Create(
                    survivorEntity.IdSurvivor, 
                    survivorEntity.SurvivorName, 
                    survivorEntity.SurvivorImage, 
                    survivorEntity.SurvivorDescription);

                if (CreatedSurvivor == null)
                {
                    Debug.WriteLine($"Не удалось создать SurvivorDomain для Id: {idSurvivor}. Ошибка: {Message}");
                    return null;
                }

                return CreatedSurvivor;
            }
        }

        public async Task<IEnumerable<SurvivorDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var survivorsEntities = await _dbContext.Survivors.AsNoTracking().ToListAsync();

                var survivorsDomain = new List<SurvivorDomain>();

                foreach (var survivorEntity in survivorsEntities)
                {
                    if (survivorEntity == null)
                    {
                        Debug.WriteLine($"Не удалось получить элемент Survivor из БД");
                        continue;
                    }

                    var id = survivorEntity.IdSurvivor;

                    var (CreatedSurvivor, Message) = SurvivorDomain.Create(
                        survivorEntity.IdSurvivor, 
                        survivorEntity.SurvivorName, 
                        survivorEntity.SurvivorImage, 
                        survivorEntity.SurvivorDescription);

                    if (CreatedSurvivor == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции SurvivorDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    survivorsDomain.Add(CreatedSurvivor);
                }

                return survivorsDomain;
            }
        }

        public IEnumerable<SurvivorDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        public async Task<SurvivorWithPerksDomain?> GetSurvivorWithPerksAsync(int idSurvivor)
        {
            using (var _dbContext = _contextFactory())
            {
                var survivorEntity = await _dbContext.Survivors.Where(x => x.IdSurvivor == idSurvivor).AsNoTracking().FirstOrDefaultAsync();

                if (survivorEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить элемент Survivor из БД с ID: {idSurvivor}.");
                    return null;
                }

                var survivorPerksEntity = await _dbContext.SurvivorPerks.Where(x => x.IdSurvivor == idSurvivor).AsNoTracking().ToListAsync();

                var survivorPerksDomainList = new List<SurvivorPerkDomain>();

                foreach (var survivorPerk in survivorPerksDomainList)
                {
                    if (survivorPerk == null)
                    {
                        Debug.WriteLine($"Не удалось получить элемент SurvivorPerk из БД");
                        continue;
                    }

                    var id = survivorPerk.IdSurvivorPerk;

                    var (CreatedSurvivorPerk, MessageSurvivorPerk) = SurvivorPerkDomain.Create(
                        survivorPerk.IdSurvivorPerk, 
                        survivorPerk.IdSurvivor, 
                        survivorPerk.PerkName, 
                        survivorPerk.PerkImage, 
                        survivorPerk.IdCategory, 
                        survivorPerk.PerkDescription);

                    if (CreatedSurvivorPerk == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции SurvivorPerkDomain для Id {id} при получение из БД. Ошибка: {MessageSurvivorPerk}");
                        continue;
                    }

                    survivorPerksDomainList.Add(CreatedSurvivorPerk);
                }

                var (CreatedSurvivorWithPerks, MessageSurvivorWithPerks) = SurvivorWithPerksDomain.Create(
                    survivorEntity.IdSurvivor,
                    survivorEntity.SurvivorName,
                    survivorEntity.SurvivorImage, 
                    survivorEntity.SurvivorDescription,
                    survivorPerksDomainList);

                if (CreatedSurvivorWithPerks == null)
                {
                    Debug.WriteLine($"Не удалось создать объект коллекции SurvivorWithPerksDomain. Ошибка: {MessageSurvivorWithPerks}");
                    return null;
                }

                return CreatedSurvivorWithPerks;
            }
        }

        public async Task<IEnumerable<SurvivorWithPerksDomain>> GetSurvivorsWithPerksAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var survivorsEntity = await _dbContext.Survivors.AsNoTracking().ToListAsync();
                var survivorPerksEntity = await _dbContext.SurvivorPerks.AsNoTracking().ToListAsync();

                var survivorWithPerksDomainList = new List<SurvivorWithPerksDomain>();

                foreach (var survivor in survivorsEntity)
                {
                    if (survivor == null)
                    {
                        Debug.WriteLine($"Не удалось получить объект Survivor.");
                        continue;
                    }

                    var survivorPerksDomain = new List<SurvivorPerkDomain>();

                    foreach (var survivorPerk in survivorPerksEntity.Where(x => x.IdSurvivor == survivor.IdSurvivor))
                    {
                        if (survivorPerk == null)
                        {
                            Debug.WriteLine($"Не удалось получить объект SurvivorPerk.");
                            continue;
                        }

                        var id = survivorPerk.IdSurvivorPerk;

                        var (CreatedSurvivorPerk, MessageSurvivorPerk) = SurvivorPerkDomain.Create(
                            survivorPerk.IdSurvivorPerk, 
                            survivorPerk.IdSurvivor,
                            survivorPerk.PerkName, 
                            survivorPerk.PerkImage, 
                            survivorPerk.IdCategory, 
                            survivorPerk.PerkDescription);

                        if (CreatedSurvivorPerk == null)
                        {
                            Debug.WriteLine($"Не удалось создать объект SurvivorPerkDomain с ID: {id}. Ошибка: {MessageSurvivorPerk}");
                            continue;
                        }

                        survivorPerksDomain.Add(CreatedSurvivorPerk);
                    }

                    var (CreatedSurvivorWithPerks, Message) = SurvivorWithPerksDomain.Create(
                        survivor.IdSurvivor, 
                        survivor.SurvivorName, 
                        survivor.SurvivorImage, 
                        survivor.SurvivorDescription, 
                        survivorPerksDomain);

                    if (CreatedSurvivorWithPerks ==  null)
                    {
                        Debug.WriteLine($"Не удалось создать объект SurvivorWithPerksDomain. Ошибка: {Message}");
                        continue;
                    }

                    survivorWithPerksDomainList.Add(CreatedSurvivorWithPerks);
                }

                return survivorWithPerksDomainList;
            }
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string survivorName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Survivors.AnyAsync(x => x.SurvivorName == survivorName);
            }
        }

        public async Task<bool> ExistAsync(int idSurvivor)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Survivors.AnyAsync(x => x.IdSurvivor == idSurvivor);
            }
        }

        public bool Exist(string survivorName)
        {
            return Task.Run(() => ExistAsync(survivorName)).Result;
        }

        public bool Exist(int idSurvivor)
        {
            return Task.Run(() => ExistAsync(idSurvivor)).Result;
        }
    }
}