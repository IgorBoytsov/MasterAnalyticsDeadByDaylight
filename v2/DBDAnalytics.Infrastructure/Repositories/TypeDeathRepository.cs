using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class TypeDeathRepository(Func<DBDContext> context) : ITypeDeathRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string typeDeathName, string typeDeathDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var typeDeathEntity = new TypeDeath
                {
                    TypeDeathName = typeDeathName,
                    TypeDeathDescription = typeDeathDescription
                };

                await _dbContext.TypeDeaths.AddAsync(typeDeathEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.TypeDeaths
                    .Where(x => x.TypeDeathName == typeDeathName)
                        .Select(x => x.IdTypeDeath)
                            .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idTypeDeath, string typeDeathName, string typeDeathDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.TypeDeaths.FirstOrDefaultAsync(x => x.IdTypeDeath == idTypeDeath);

                if (entity != null)
                {
                    entity.TypeDeathName = typeDeathName;
                    entity.TypeDeathDescription = typeDeathDescription;

                    _dbContext.TypeDeaths.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    return idTypeDeath;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить TypeDeath на уровне базы данных для Id: {idTypeDeath}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idTypeDeath)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.TypeDeaths
                            .Where(x => x.IdTypeDeath == idTypeDeath)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<TypeDeathDomain?> GetAsync(int idTypeDeath)
        {
            using (var _dbContext = _contextFactory())
            {
                var typeDeathEntity = await _dbContext.TypeDeaths
                        .FirstOrDefaultAsync(x => x.IdTypeDeath == idTypeDeath);

                if (typeDeathEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Killer для Id: {idTypeDeath}");
                    return null;
                }

                var (CreatedTypeDeath, Message) = TypeDeathDomain.Create(
                    typeDeathEntity.IdTypeDeath, 
                    typeDeathEntity.TypeDeathName, 
                    typeDeathEntity.TypeDeathDescription);

                if (CreatedTypeDeath == null)
                {
                    Debug.WriteLine($"Не удалось создать TypeDeathDomain для Id: {idTypeDeath}. Ошибка: {Message}");
                    return null;
                }

                return CreatedTypeDeath;
            }
        }

        public async Task<IEnumerable<TypeDeathDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var typeDeathsEntities = await _dbContext.TypeDeaths.AsNoTracking().ToListAsync();

                var typeDeathsDomain = new List<TypeDeathDomain>();

                foreach (var typeDeathEntity in typeDeathsEntities)
                {
                    if (typeDeathEntity == null)
                    {
                        Debug.WriteLine($"Не удалось получить элемент TypeDeath из БД");
                        continue;
                    }

                    var id = typeDeathEntity.IdTypeDeath;

                    var (CreateTypeDeath, Message) = TypeDeathDomain.Create(typeDeathEntity.IdTypeDeath, typeDeathEntity.TypeDeathName, typeDeathEntity.TypeDeathDescription);

                    if (CreateTypeDeath == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции TypeDeathDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    typeDeathsDomain.Add(CreateTypeDeath);
                }

                return typeDeathsDomain;
            }
        }

        public IEnumerable<TypeDeathDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string typeDeathName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.TypeDeaths.AnyAsync(x => x.TypeDeathName == typeDeathName);
            }
        }

        public async Task<bool> ExistAsync(int idTypeDeath)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.TypeDeaths.AnyAsync(x => x.IdTypeDeath == idTypeDeath);
            }
        }

        public bool Exist(string typeDeathName)
        {
            return Task.Run(() => ExistAsync(typeDeathName)).Result;
        }

        public bool Exist(int idTypeDeath)
        {
            return Task.Run(() => ExistAsync(idTypeDeath)).Result;
        }
    }
}