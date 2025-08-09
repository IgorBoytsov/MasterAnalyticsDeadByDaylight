using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class MeasurementRepository(Func<DBDContext> context) : IMeasurementRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string measurementName, string measurementDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var measurementEntity = new Measurement
                {
                    MeasurementName = measurementName,
                    MeasurementDescription = measurementDescription
                };

                await _dbContext.Measurements.AddAsync(measurementEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.Measurements
                    .Where(x => x.MeasurementName == measurementName)
                        .Select(x => x.IdMeasurement)
                            .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idMeasurement, string measurementName, string measurementDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.Measurements.FirstOrDefaultAsync(x => x.IdMeasurement == idMeasurement);

                if (entity != null)
                {
                    entity.MeasurementName = measurementName;
                    entity.MeasurementDescription = measurementDescription;

                    _dbContext.Measurements.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    return idMeasurement;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить Measurement на уровне базы данных для Id: {idMeasurement}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idMeasurement)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.Measurements
                            .Where(x => x.IdMeasurement == idMeasurement)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<MeasurementDomain?> GetAsync(int idMeasurement)
        {
            using (var _dbContext = _contextFactory())
            {
                var measurementEntity = await _dbContext.Measurements
                        .FirstOrDefaultAsync(x => x.IdMeasurement == idMeasurement);

                if (measurementEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Killer для Id: {idMeasurement}");
                    return null;
                }

                var (CreatedMeasurement, Message) = MeasurementDomain.Create(
                    measurementEntity.IdMeasurement, 
                    measurementEntity.MeasurementName, 
                    measurementEntity.MeasurementDescription);

                if (CreatedMeasurement == null)
                {
                    Debug.WriteLine($"Не удалось создать MeasurementDomain для Id: {idMeasurement}. Ошибка: {Message}");
                    return null;
                }

                return CreatedMeasurement;
            }
        }

        public async Task<IEnumerable<MeasurementDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var measurementEntities = await _dbContext.Measurements.AsNoTracking().ToListAsync();

                var measurementsDomain = new List<MeasurementDomain>();

                foreach (var measurementEntity in measurementEntities)
                {
                    var id = measurementEntity.IdMeasurement;

                    var (CreatedMeasurement, Message) = MeasurementDomain.Create(
                        measurementEntity.IdMeasurement, 
                        measurementEntity.MeasurementName, 
                        measurementEntity.MeasurementDescription);

                    if (CreatedMeasurement == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции MeasurementDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    measurementsDomain.Add(CreatedMeasurement);
                }

                return measurementsDomain;
            }
        }

        public IEnumerable<MeasurementDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        public async Task<MeasurementWithMapsDomain?> GetMeasurementWithMapsAsync(int idMeasurement)
        {
            using (var _dbContext = _contextFactory())
            {
                var measurementEntity = await _dbContext.Measurements.AsNoTracking().FirstOrDefaultAsync(x => x.IdMeasurement == idMeasurement);

                if (measurementEntity == null)
                {
                    Console.WriteLine($"Не удалось получить элемент Measurement из БД с ID: {idMeasurement}.");
                    return null;
                }

                var mapsEntity = await _dbContext.Maps.AsNoTracking().Where(x => x.IdMeasurement == idMeasurement).ToListAsync();
                var mapsDomainList = new List<MapDomain>();

                foreach (var map in mapsEntity)
                {
                    var id = map.IdMap;

                    var (CreatedMap, MessageMap) = MapDomain.Create(
                        map.IdMap, 
                        map.MapName, 
                        map.MapImage, 
                        map.MapDescription, 
                        map.IdMeasurement);

                    if (CreatedMap != null)
                    {
                        mapsDomainList.Add(CreatedMap);
                    }
                    else
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции MapDomain для Id {id} при получение из БД. Ошибка: {MessageMap}");
                    }

                }

                var (CreatedMeasurementWithMaps, MessageMeasurementWithMaps) = MeasurementWithMapsDomain.Create(
                    measurementEntity.IdMeasurement, 
                    measurementEntity.MeasurementName, 
                    measurementEntity.MeasurementDescription, 
                    mapsDomainList);

                if (CreatedMeasurementWithMaps == null)
                {
                    Debug.WriteLine($"Не удалось создать объект MeasurementWithMapsDomain для Id {idMeasurement}. Ошибка: {MessageMeasurementWithMaps}");
                    return null;
                }

                return CreatedMeasurementWithMaps;
            }
        }

        public MeasurementWithMapsDomain? GetMeasurementWithMaps(int idMeasurement)
        {
            return Task.Run(() => GetMeasurementWithMapsAsync(idMeasurement)).Result;
        }

        public async Task<IEnumerable<MeasurementWithMapsDomain>> GetMeasurementsWithMapsAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var measurementsEntity = await _dbContext.Measurements.AsNoTracking().ToListAsync();
                var mapsEntity = await _dbContext.Maps.AsNoTracking().ToListAsync();

                var measurementsWithMApsDomainList = new List<MeasurementWithMapsDomain>();

                foreach (var measurement in measurementsEntity)
                {
                    if (measurement == null)
                    {
                        Debug.WriteLine($"Не удалось получить объект Measurement.");
                        continue;
                    }

                    var mapsDomain = new List<MapDomain>();

                    foreach (var map in mapsEntity.Where(x => x.IdMeasurement == measurement.IdMeasurement))
                    {
                        if (map == null)
                        {
                            Debug.WriteLine($"Не удалось получить объект Map.");
                            continue;
                        }

                        var id = map.IdMap;

                        var (CreatedMap, MessageMap) = MapDomain.Create(
                            map.IdMap, 
                            map.MapName, 
                            map.MapImage, 
                            map.MapDescription, 
                            map.IdMeasurement);

                        if (CreatedMap == null)
                        {
                            Debug.WriteLine($"Не удалось создать объект MapDomain с ID: {id}. Ошибка: {MessageMap}");
                            continue;
                        }

                        mapsDomain.Add(CreatedMap);
                    }

                    var (CreatedMeasurementWithMaps, MessageMeasurementWithMaps) = MeasurementWithMapsDomain.Create(
                        measurement.IdMeasurement, 
                        measurement.MeasurementName, 
                        measurement.MeasurementDescription, 
                        mapsDomain);

                    if (CreatedMeasurementWithMaps != null)
                    {
                        measurementsWithMApsDomainList.Add(CreatedMeasurementWithMaps);
                    }
                    else
                    {
                        Debug.WriteLine($"Не удалось создать объект MeasurementWithMapsDomain. Ошибка {MessageMeasurementWithMaps}");
                    }
                }

                return measurementsWithMApsDomainList;
            }
        }

        public IEnumerable<MeasurementWithMapsDomain> GetMeasurementsWithMaps()
        {
            return Task.Run(() => GetMeasurementsWithMapsAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string measurementName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Measurements.AnyAsync(x => x.MeasurementName == measurementName);
            }
        }

        public async Task<bool> ExistAsync(int idMeasurement)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Measurements.AnyAsync(x => x.IdMeasurement == idMeasurement);
            }
        }

        public bool Exist(string measurementName)
        {
            return Task.Run(() => ExistAsync(measurementName)).Result;
        }

        public bool Exist(int idMeasurement)
        {
            return Task.Run(() => ExistAsync(idMeasurement)).Result;
        }
    }
}