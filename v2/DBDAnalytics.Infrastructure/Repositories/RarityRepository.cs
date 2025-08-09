using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class RarityRepository(Func<DBDContext> context) : IRarityRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string rarityName, string? description)
        {
            using (var _dbContext = _contextFactory())
            {
                var rarityEntity = new Rarity
                {
                    RarityName = rarityName,
                    RarityDescription = description
                };

                await _dbContext.Rarities.AddAsync(rarityEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.Rarities
                    .Where(x => x.RarityName == rarityName)
                        .Select(x => x.IdRarity)
                            .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idRarity, string rarityName, string? description)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.Rarities.FirstOrDefaultAsync(x => x.IdRarity == idRarity);

                if (entity != null)
                {
                    entity.RarityName = rarityName;
                    entity.RarityDescription = description;

                    _dbContext.Rarities.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    return idRarity;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить Rarity на уровне базы данных для Id: {idRarity}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idRarity)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.Rarities
                            .Where(x => x.IdRarity == idRarity)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--Get-------------------------------------------------------------------------------------------*/

        public async Task<RarityDomain?> GetAsync(int idRarity)
        {
            using (var _dbContext = _contextFactory())
            {
                var rarityEntity = await _dbContext.Rarities
                        .FirstOrDefaultAsync(x => x.IdRarity == idRarity);

                if (rarityEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Rarity для Id: {idRarity}");
                    return null;
                }

                var (CreatedRarity, Message) = RarityDomain.Create(
                    rarityEntity.IdRarity, 
                    rarityEntity.RarityName,
                    rarityEntity.RarityDescription);

                if (CreatedRarity == null)
                {
                    Debug.WriteLine($"Не удалось создать KillerDomain для Id: {idRarity}. Ошибка: {Message}");
                    return null;
                }

                return CreatedRarity;
            }
        }

        public async Task<IEnumerable<RarityDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var rarityEntities = await _dbContext.Rarities.AsNoTracking().ToListAsync();

                var raritiesDomain = new List<RarityDomain>();

                foreach (var rarityEntity in rarityEntities)
                {
                    if (rarityEntities == null)
                    {
                        Debug.WriteLine($"Не удалось получить объект Killer.");
                        continue;
                    }

                    var (CreatedRarity, Message) = RarityDomain.Create(
                        rarityEntity.IdRarity, 
                        rarityEntity.RarityName,
                        rarityEntity.RarityDescription);

                    if (CreatedRarity == null)
                    {
                        Debug.WriteLine($"Не удалось создать объект коллекции RarityDomain. Ошибка: {Message}");
                        return null;
                    }

                    raritiesDomain.Add(CreatedRarity);
                }

                return raritiesDomain;
            }
        }

        public IEnumerable<RarityDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/ 

        public async Task<bool> ExistAsync(string rarityName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Rarities.AnyAsync(x => x.RarityName == rarityName);
            }
        }

        public async Task<bool> ExistAsync(int idRarity)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Rarities.AnyAsync(x => x.IdRarity == idRarity);
            }
        }

        public bool Exist(string rarityName)
        {
            return Task.Run(() => ExistAsync(rarityName)).Result;
        }

        public bool Exist(int idRarity)
        {
            return Task.Run(() => ExistAsync(idRarity)).Result;
        }
    }
}