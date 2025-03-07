using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class OfferingCategoryRepository(Func<DBDContext> context) : IOfferingCategoryRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string offeringCategoryName)
        {
            using (var _dbContext = _contextFactory())
            {
                var offeringCategoryEntity = new OfferingCategory
                {
                    CategoryName = offeringCategoryName
                };

                await _dbContext.OfferingCategories.AddAsync(offeringCategoryEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.OfferingCategories
                    .Where(x => x.CategoryName == offeringCategoryName)
                        .Select(x => x.IdCategory)
                            .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idOfferingCategory, string offeringCategoryName)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.OfferingCategories.FirstOrDefaultAsync(x => x.IdCategory == idOfferingCategory);

                if (entity != null)
                {
                    entity.CategoryName = offeringCategoryName;

                    _dbContext.OfferingCategories.Update(entity);
                    _dbContext.SaveChanges();

                    return idOfferingCategory;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить Killer на уровне базы данных для Id: {idOfferingCategory}");
                    return -1;
                }

            }
        }

        public async Task<int> DeleteAsync(int idOfferingCategory)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.OfferingCategories
                            .Where(x => x.IdCategory == idOfferingCategory)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<OfferingCategoryDomain?> GetAsync(int idOfferingCategory)
        {
            using (var _dbContext = _contextFactory())
            {
                var offeringCategoryEntity = await _dbContext.OfferingCategories
                        .FirstOrDefaultAsync(x => x.IdCategory == idOfferingCategory);

                if (offeringCategoryEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Killer для Id: {idOfferingCategory}");
                    return null;
                }

                var (CreatedOfferingCategory, Message) = OfferingCategoryDomain.Create(
                    offeringCategoryEntity.IdCategory, 
                    offeringCategoryEntity.CategoryName);

                return CreatedOfferingCategory ??= null;
            }
        }

        public async Task<IEnumerable<OfferingCategoryDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var offeringCategoryEntities = await _dbContext.OfferingCategories.AsNoTracking().ToListAsync();

                var offeringCategoriesDomain = new List<OfferingCategoryDomain>();

                foreach (var offeringCategoryEntity in offeringCategoryEntities)
                {
                    if (offeringCategoryEntities == null)
                    {
                        Debug.WriteLine($"Запись не была получена из БД");
                        continue;
                    }

                    var id = offeringCategoryEntity.IdCategory;

                    var (CreatedOfferingCategory, Message) = OfferingCategoryDomain.Create(
                        offeringCategoryEntity.IdCategory, 
                        offeringCategoryEntity.CategoryName);

                    if (CreatedOfferingCategory == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции OfferingCategoryDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    offeringCategoriesDomain.Add(CreatedOfferingCategory);
                }

                return offeringCategoriesDomain;
            }
        }

        public IEnumerable<OfferingCategoryDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string offeringCategoryName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.OfferingCategories.AnyAsync(x => x.CategoryName == offeringCategoryName);
            }
        }

        public async Task<bool> ExistAsync(int idOfferingCategory)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.OfferingCategories.AnyAsync(x => x.IdCategory == idOfferingCategory);
            }
        }

        public bool Exist(string offeringCategoryName)
        {
            return Task.Run(() => ExistAsync(offeringCategoryName)).Result;
        }

        public bool Exist(int idOfferingCategory)
        {
            return Task.Run(() => ExistAsync(idOfferingCategory)).Result;
        }
    }
}