using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class KillerPerkCategoryRepository(Func<DBDContext> context) : IKillerPerkCategoryRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string killerPerkCategoryName, string? description)
        {
            using (var _dbContext = _contextFactory())
            {
                var killerPerkCategoryEntity = new KillerPerkCategory
                {
                    CategoryName = killerPerkCategoryName,
                    Description = description
                };

                await _dbContext.KillerPerkCategories.AddAsync(killerPerkCategoryEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.KillerPerkCategories
                    .Where(x => x.CategoryName == killerPerkCategoryName)
                        .Select(x => x.IdKillerPerkCategory)
                            .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idKillerPerkCategory, string killerPerkCategoryName, string? description)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.KillerPerkCategories.FirstOrDefaultAsync(x => x.IdKillerPerkCategory == idKillerPerkCategory);

                if (entity != null)
                {

                    entity.CategoryName = killerPerkCategoryName;
                    entity.Description = description;

                    _dbContext.KillerPerkCategories.Update(entity);
                    _dbContext.SaveChanges();

                    return idKillerPerkCategory;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить KillerPerkCategory на уровне базы данных для Id: {idKillerPerkCategory}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idKillerPerkCategory)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.KillerPerkCategories
                            .Where(x => x.IdKillerPerkCategory == idKillerPerkCategory)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<KillerPerkCategoryDomain?> GetAsync(int idKillerPerkCategory)
        {
            using (var _dbContext = _contextFactory())
            {
                var killerPerkCategoryEntity = await _dbContext.KillerPerkCategories
                        .FirstOrDefaultAsync(x => x.IdKillerPerkCategory == idKillerPerkCategory);

                if (killerPerkCategoryEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить killerPerkCategory для Id: {idKillerPerkCategory}");
                    return null;
                }

                var (CreatedKillerPerkCategory, Message) = KillerPerkCategoryDomain.Create(
                    killerPerkCategoryEntity.IdKillerPerkCategory, 
                    killerPerkCategoryEntity.CategoryName,
                    killerPerkCategoryEntity.Description);

                if (CreatedKillerPerkCategory == null)
                {
                    Debug.WriteLine($"Не удалось создать KillerPerkCategoryDomain для Id: {idKillerPerkCategory}. Ошибка: {Message}");
                    return null;
                }

                return CreatedKillerPerkCategory;
            }
        }

        public async Task<IEnumerable<KillerPerkCategoryDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var killerPerkCategoryEntities = await _dbContext.KillerPerkCategories.AsNoTracking().ToListAsync();

                var killerPerkCategoriesDomain = new List<KillerPerkCategoryDomain>();

                foreach (var killerPerkCategoryEntity in killerPerkCategoryEntities)
                {
                    var id = killerPerkCategoryEntity.IdKillerPerkCategory;

                    var (CreatedKillerPerkCategory, Message) = KillerPerkCategoryDomain.Create(
                         killerPerkCategoryEntity.IdKillerPerkCategory,
                         killerPerkCategoryEntity.CategoryName, 
                         killerPerkCategoryEntity.Description);

                    if (CreatedKillerPerkCategory == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции KillerPerkCategoryDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    killerPerkCategoriesDomain.Add(CreatedKillerPerkCategory);
                }

                return killerPerkCategoriesDomain;
            }
        }

        public IEnumerable<KillerPerkCategoryDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string killerPerkCategoryName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.KillerPerkCategories.AnyAsync(x => x.CategoryName == killerPerkCategoryName);
            }
        }

        public async Task<bool> ExistAsync(int idKillerPerkCategory)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.KillerPerkCategories.AnyAsync(x => x.IdKillerPerkCategory == idKillerPerkCategory);
            }
        }

        public bool Exist(string killerPerkCategoryName)
        {
            return Task.Run(() => ExistAsync(killerPerkCategoryName)).Result;
        }

        public bool Exist(int idKillerPerkCategory)
        {
            return Task.Run(() => ExistAsync(idKillerPerkCategory)).Result;
        }
    }
}