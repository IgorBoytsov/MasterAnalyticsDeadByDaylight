using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class MatchAttributeRepository(Func<DBDContext> context) : IMatchAttributeRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string attributeName, string? AttributeDescription, DateTime CreatedAt, bool isHide)
        {
            using (var _dbContext = _contextFactory())
            {
                var matchAttribute = new MatchAttribute
                {
                    AttributeName = attributeName,
                    AttributeDescription = AttributeDescription,
                    CreatedAt = CreatedAt,
                    IsHide = isHide
                };

                await _dbContext.MatchAttributes.AddAsync(matchAttribute);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.MatchAttributes
                        .OrderByDescending(x => x.IdMatchAttribute)
                            .Select(x => x.IdMatchAttribute)
                                .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idMatchAttribute, string attributeName, string? AttributeDescription, bool isHide)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.MatchAttributes.FirstOrDefaultAsync(x => x.IdMatchAttribute == idMatchAttribute);

                if (entity != null)
                {
                    entity.AttributeName = attributeName;
                    entity.AttributeDescription = AttributeDescription;
                    entity.IsHide = isHide;

                    _dbContext.MatchAttributes.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    return idMatchAttribute;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить MatchAttribute на уровне базы данных для Id: {idMatchAttribute}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idMatchAttribute)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.MatchAttributes
                            .Where(x => x.IdMatchAttribute == idMatchAttribute)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<MatchAttributeDomain?> GetAsync(int idMatchAttribute)
        {
            using (var _dbContext = _contextFactory())
            {
                var matchAttribute = await _dbContext.MatchAttributes
                       .FirstOrDefaultAsync(x => x.IdMatchAttribute == idMatchAttribute);

                if (matchAttribute == null)
                {
                    Debug.WriteLine($"Не удалось получить MatchAttribute для Id: {idMatchAttribute}");
                    return null;
                }

                var (CreatedMatchAttribute, Message) = MatchAttributeDomain.Create(
                    matchAttribute.IdMatchAttribute, 
                    matchAttribute.AttributeName,
                    matchAttribute.AttributeDescription, 
                    matchAttribute.CreatedAt, 
                    matchAttribute.IsHide);

                if (CreatedMatchAttribute == null)
                {
                    Debug.WriteLine($"Не удалось создать MatchAttributeDomain для Id: {idMatchAttribute}. Ошибка: {Message}");
                    return null;
                }

                return CreatedMatchAttribute;
            }
        }

        public async Task<IEnumerable<MatchAttributeDomain>> GetAllAsync(bool isHide)
        {
            using (var _dbContext = _contextFactory())
            {
                var matchAttributeEntities = await _dbContext.MatchAttributes.Where(x => x.IsHide == isHide).AsNoTracking().ToListAsync();

                var matchAttributeDomain = new List<MatchAttributeDomain>();

                foreach (var matchAttribute in matchAttributeEntities)
                {
                    var id = matchAttribute.IdMatchAttribute;

                    var (CreatedMatchAttribute, Message) = MatchAttributeDomain.Create(
                        matchAttribute.IdMatchAttribute,
                        matchAttribute.AttributeName,
                        matchAttribute.AttributeDescription,
                        matchAttribute.CreatedAt,
                        matchAttribute.IsHide);

                    if (CreatedMatchAttribute == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции MatchAttributeDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    matchAttributeDomain.Add(CreatedMatchAttribute);
                }

                return matchAttributeDomain;
            }
        }

        public IEnumerable<MatchAttributeDomain> GetAll(bool isHide)
        {
            return Task.Run(() => GetAllAsync(isHide)).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string attributeName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.MatchAttributes.AnyAsync(x => x.AttributeName == attributeName);
            }
        }

        public async Task<bool> ExistAsync(int idMatchAttribute)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.MatchAttributes.AnyAsync(x => x.IdMatchAttribute == idMatchAttribute);
            }
        }

        public bool Exist(string attributeName)
        {
            return Task.Run(() => ExistAsync(attributeName)).Result;
        }

        public bool Exist(int idMatchAttribute)
        {
            return Task.Run(() => ExistAsync(idMatchAttribute)).Result;
        }
    }
}