using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class MapRepository(Func<DBDContext> context) : IMapRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(int idMeasurement, string mapName, byte[] mapImage, string mapDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var mapEntity = new Map
                {
                    IdMeasurement = idMeasurement,
                    MapName = mapName,
                    MapImage = mapImage,
                    MapDescription = mapDescription
                };

                await _dbContext.Maps.AddAsync(mapEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.Maps
                    .Where(x => x.IdMeasurement == idMeasurement)
                        .OrderByDescending(x => x.IdMap)
                            .Select(x => x.IdMap)
                                .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idMap, int idMeasurement, string mapName, byte[] mapImage, string mapDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.Maps.FirstOrDefaultAsync(x => x.IdMap == idMap);

                if (entity != null)
                {

                    entity.IdMeasurement = idMeasurement;
                    entity.MapName = mapName;
                    entity.MapImage = mapImage;
                    entity.MapDescription = mapDescription;

                    _dbContext.Maps.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    return idMap;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить Map на уровне базы данных для Id: {idMap}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idMap)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.Maps
                            .Where(x => x.IdMap == idMap)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<MapDomain?> GetAsync(int idMap)
        {
            using (var _dbContext = _contextFactory())
            {
                var mapEntity = await _dbContext.Maps
                        .FirstOrDefaultAsync(x => x.IdMap == idMap);

                if (mapEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Map для Id: {idMap}");
                    return null;
                }

                var (CreatedMap, Message) = MapDomain.Create(
                    mapEntity.IdMap, 
                    mapEntity.MapName, 
                    mapEntity.MapImage, 
                    mapEntity.MapDescription, 
                    mapEntity.IdMeasurement);

                if (CreatedMap == null)
                {
                    Debug.WriteLine($"Не удалось создать MapDomain для Id: {idMap}. Ошибка: {Message}");
                    return null;
                }

                return CreatedMap;
            }
        }

        public async Task<IEnumerable<MapDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var mapsEntities = await _dbContext.Maps.AsNoTracking().ToListAsync();

                var mapsDomain = new List<MapDomain>();

                foreach (var mapEntity in mapsEntities)
                {
                    var id = mapEntity.IdMap;

                    var (CreatedMap, Message) = MapDomain.Create(
                        mapEntity.IdMap,
                        mapEntity.MapName,
                        mapEntity.MapImage,
                        mapEntity.MapDescription,
                        mapEntity.IdMeasurement);

                    if (CreatedMap == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции MapDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    mapsDomain.Add(CreatedMap);
                }

                return mapsDomain;
            }
        }

        public IEnumerable<MapDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string mapName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Maps.AnyAsync(x => x.MapName == mapName);
            }
        }

        public async Task<bool> ExistAsync(int idMap)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Maps.AnyAsync(x => x.IdMap == idMap);
            }
        }

        public bool Exist(string mapName)
        {
            return Task.Run(() => ExistAsync(mapName)).Result;
        }

        public bool Exist(int idMap)
        {
            return Task.Run(() => ExistAsync(idMap)).Result;
        }
    }
}