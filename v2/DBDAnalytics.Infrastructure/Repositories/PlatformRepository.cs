using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class PlatformRepository(Func<DBDContext> context) : IPlatformRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string platformName, string platformDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var platformEntity = new Platform
                {
                    PlatformName = platformName,
                    PlatformDescription = platformDescription
                };

                await _dbContext.Platforms.AddAsync(platformEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.Platforms
                    .Where(x => x.PlatformName == platformName)
                        .Select(x => x.IdPlatform)
                            .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idPlatform, string platformName, string platformDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.Platforms.FirstOrDefaultAsync(x => x.IdPlatform == idPlatform);

                if (entity != null)
                {
                    entity.PlatformName = platformName;
                    entity.PlatformDescription = platformDescription;

                    _dbContext.Platforms.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    return idPlatform;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить Platform на уровне базы данных для Id: {idPlatform}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idPlatform)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.Platforms
                            .Where(x => x.IdPlatform == idPlatform)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<PlatformDomain?> GetAsync(int idPlatform)
        {
            using (var _dbContext = _contextFactory())
            {
                var platformEntity = await _dbContext.Platforms
                        .FirstOrDefaultAsync(x => x.IdPlatform == idPlatform);

                if (platformEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Killer для Id: {idPlatform}");
                    return null;
                }

                var (CreatedPlatform, Message) = PlatformDomain.Create(
                    platformEntity.IdPlatform, 
                    platformEntity.PlatformName,
                    platformEntity.PlatformDescription);

                if (CreatedPlatform == null)
                {
                    Debug.WriteLine($"Не удалось создать PlatformDomain для Id: {idPlatform}. Ошибка: {Message}");
                    return null;
                }

                return CreatedPlatform;
            }
        }

        public async Task<IEnumerable<PlatformDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var platformEntities = await _dbContext.Platforms.AsNoTracking().ToListAsync();

                var platformsDomain = new List<PlatformDomain>();

                foreach (var platformEntity in platformEntities)
                {
                    if (platformEntity == null)
                    {
                        Debug.WriteLine($"Не удалось получить элемент Platform из БД");
                        continue;
                    }

                    var id = platformEntity.IdPlatform;

                    var (CreatedPlatform, Message) = PlatformDomain.Create(
                        platformEntity.IdPlatform, 
                        platformEntity.PlatformName, 
                        platformEntity.PlatformDescription);

                    if (CreatedPlatform == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции PlatformDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    platformsDomain.Add(CreatedPlatform);
                }

                return platformsDomain;
            }
        }

        public IEnumerable<PlatformDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string platformName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Platforms.AnyAsync(x => x.PlatformName == platformName);
            }  
        }

        public async Task<bool> ExistAsync(int idPlatform)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Platforms.AnyAsync(x => x.IdPlatform == idPlatform);
            }
        }

        public bool Exist(string platformName)
        {
            return Task.Run(() => ExistAsync(platformName)).Result;
        }

        public bool Exist(int idPlatform)
        {
            return Task.Run(() => ExistAsync(idPlatform)).Result;
        }
    }
}