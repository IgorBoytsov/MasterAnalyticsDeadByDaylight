using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class SurvivorPerkCategoryRepository(Func<DBDContext> context) : ISurvivorPerkCategoryRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        public async Task<int> CreateAsync(string survivorPerkCategoryName, string? description)
        {
            using (var _dbContext = _contextFactory())
            {
                var survivorPerkCategoryEntity = new SurvivorPerkCategory
                {
                    CategoryName = survivorPerkCategoryName,
                    Description = description
                };

                await _dbContext.SurvivorPerkCategories.AddAsync(survivorPerkCategoryEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.SurvivorPerkCategories
                    .Where(x => x.CategoryName == survivorPerkCategoryName)
                        .Select(x => x.IdSurvivorPerkCategory)
                            .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idSurvivorPerkCategory, string survivorPerkCategoryName, string? description)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.SurvivorPerkCategories.FirstOrDefaultAsync(x => x.IdSurvivorPerkCategory == idSurvivorPerkCategory);

                if (entity != null)
                {
                    entity.CategoryName = survivorPerkCategoryName;
                    entity.Description = description;

                    _dbContext.SurvivorPerkCategories.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    return idSurvivorPerkCategory;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить SurvivorPerkCategory на уровне базы данных для Id: {idSurvivorPerkCategory}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idSurvivorPerkCategory)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.SurvivorPerkCategories
                            .Where(x => x.IdSurvivorPerkCategory == idSurvivorPerkCategory)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<SurvivorPerkCategoryDomain?> GetAsync(int idSurvivorPerkCategory)
        {
            using (var _dbContext = _contextFactory())
            {
                var survivorPerkCategoryEntity = await _dbContext.SurvivorPerkCategories
                        .FirstOrDefaultAsync(x => x.IdSurvivorPerkCategory == idSurvivorPerkCategory);

                if (survivorPerkCategoryEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Killer для Id: {idSurvivorPerkCategory}");
                    return null;
                }

                var (CreatedSurvivorPerkCategory, Message) = SurvivorPerkCategoryDomain.Create(
                    survivorPerkCategoryEntity.IdSurvivorPerkCategory, 
                    survivorPerkCategoryEntity.CategoryName,
                    survivorPerkCategoryEntity.Description);

                if (CreatedSurvivorPerkCategory == null)
                {
                    Debug.WriteLine($"Не удалось создать KillerDomain для Id: {idSurvivorPerkCategory}. Ошибка: {Message}");
                    return null;
                }

                return CreatedSurvivorPerkCategory;
            }
        }

        public async Task<IEnumerable<SurvivorPerkCategoryDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var survivorPerkCategoryEntities = await _dbContext.SurvivorPerkCategories.AsNoTracking().ToListAsync();

                var survivorPerkCategoriesDomain = new List<SurvivorPerkCategoryDomain>();

                foreach (var survivorPerkCategoryEntity in survivorPerkCategoryEntities)
                {
                    if (survivorPerkCategoryEntity == null)
                    {
                        Debug.WriteLine($"Не удалось получить элемент SurvivorPerkCategory из БД");
                        continue;
                    }

                    var id = survivorPerkCategoryEntity.IdSurvivorPerkCategory;

                    var (CreatedSurvivorPerkCategory, Message) = SurvivorPerkCategoryDomain.Create(
                        survivorPerkCategoryEntity.IdSurvivorPerkCategory, 
                        survivorPerkCategoryEntity.CategoryName,
                        survivorPerkCategoryEntity.Description);
                   
                    if (CreatedSurvivorPerkCategory == null)
                    {
                        Debug.WriteLine($"Не удалось создать KillerDomain для Id: {id}. Ошибка: {Message}");
                        continue;
                    }

                    survivorPerkCategoriesDomain.Add(CreatedSurvivorPerkCategory);
                }

                return survivorPerkCategoriesDomain;
            }
        }

        public IEnumerable<SurvivorPerkCategoryDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string survivorPerkCategoryName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.SurvivorPerkCategories.AnyAsync(x => x.CategoryName == survivorPerkCategoryName);
            }  
        }

        public async Task<bool> ExistAsync(int idSurvivorPerkCategory)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.SurvivorPerkCategories.AnyAsync(x => x.IdSurvivorPerkCategory == idSurvivorPerkCategory);
            }
        }

        public bool Exist(string survivorPerkCategoryName)
        {
            return Task.Run(() => ExistAsync(survivorPerkCategoryName)).Result;
        }

        public bool Exist(int idSurvivorPerkCategory)
        {
            return Task.Run(() => ExistAsync(idSurvivorPerkCategory)).Result;
        }
    }
}