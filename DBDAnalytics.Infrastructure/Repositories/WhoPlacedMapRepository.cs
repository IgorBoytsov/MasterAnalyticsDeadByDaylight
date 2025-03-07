using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class WhoPlacedMapRepository(Func<DBDContext> context) : IWhoPlacedMapRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string whoPlacedMapName)
        {
            using (var _dbContext = _contextFactory())
            {
                var whoPlacedMapEntity = new WhoPlacedMap
                {
                    WhoPlacedMapName = whoPlacedMapName
                };

                await _dbContext.WhoPlacedMaps.AddAsync(whoPlacedMapEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.WhoPlacedMaps
                    .Where(x => x.WhoPlacedMapName == whoPlacedMapName)
                        .Select(x => x.IdWhoPlacedMap)
                            .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idWhoPlacedMap, string whoPlacedMapName)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.WhoPlacedMaps.FirstOrDefaultAsync(x => x.IdWhoPlacedMap == idWhoPlacedMap);

                if (entity != null)
                {
                    entity.WhoPlacedMapName = whoPlacedMapName;

                    _dbContext.WhoPlacedMaps.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    return idWhoPlacedMap;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить WhoPlacedMap на уровне базы данных для Id: {whoPlacedMapName}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idWhoPlacedMap)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.WhoPlacedMaps
                            .Where(x => x.IdWhoPlacedMap == idWhoPlacedMap)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<WhoPlacedMapDomain?> GetAsync(int idWhoPlacedMap)
        {
            using (var _dbContext = _contextFactory())
            {
                var whoPlacedMapEntity = await _dbContext.WhoPlacedMaps
                        .FirstOrDefaultAsync(x => x.IdWhoPlacedMap == idWhoPlacedMap);

                if (whoPlacedMapEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить WhoPlacedMap для Id: {idWhoPlacedMap}");
                    return null;
                }

                var (CreatedWhoPlacedMap, Message) = WhoPlacedMapDomain.Create(whoPlacedMapEntity.IdWhoPlacedMap, whoPlacedMapEntity.WhoPlacedMapName);

                if (CreatedWhoPlacedMap == null)
                {
                    Debug.WriteLine($"Не удалось получить WhoPlacedMap для Id: {idWhoPlacedMap}");
                    return null;
                }

                return CreatedWhoPlacedMap;
            }
        }

        public async Task<IEnumerable<WhoPlacedMapDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var whoPlacedMapEntities = await _dbContext.WhoPlacedMaps.AsNoTracking().ToListAsync();

                var whoPlacedMapsDomain = new List<WhoPlacedMapDomain>();

                foreach (var whoPlacedMapEntity in whoPlacedMapEntities)
                {
                    if (whoPlacedMapEntity == null)
                    {
                        Debug.WriteLine($"Не удалось получить элемент WhoPlacedMap из БД");
                        continue;
                    }

                    var id = whoPlacedMapEntity.IdWhoPlacedMap;

                    var (CreatedWhoPlacedMap, Message) = WhoPlacedMapDomain.Create(whoPlacedMapEntity.IdWhoPlacedMap, whoPlacedMapEntity.WhoPlacedMapName);

                    if (CreatedWhoPlacedMap == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции WhoPlacedMapDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    whoPlacedMapsDomain.Add(CreatedWhoPlacedMap);
                }

                return whoPlacedMapsDomain;
            }
        }

        public IEnumerable<WhoPlacedMapDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string whoPlacedMapName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.WhoPlacedMaps.AnyAsync(x => x.WhoPlacedMapName == whoPlacedMapName);
            }
        }

        public async Task<bool> ExistAsync(int idWhoPlacedMap)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.WhoPlacedMaps.AnyAsync(x => x.IdWhoPlacedMap == idWhoPlacedMap);
            }
        }

        public bool Exist(string whoPlacedMapName)
        {
            return Task.Run(() => ExistAsync(whoPlacedMapName)).Result;
        }

        public bool Exist(int idWhoPlacedMap)
        {
            return Task.Run(() => ExistAsync(idWhoPlacedMap)).Result;
        }
    }
}