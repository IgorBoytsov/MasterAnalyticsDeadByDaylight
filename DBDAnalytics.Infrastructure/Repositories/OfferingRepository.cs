using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class OfferingRepository(Func<DBDContext> context) : IOfferingRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var offeringEntity = new Offering
                {
                    IdRole = idRole,
                    IdCategory = idCategory,
                    IdRarity = idRarity,
                    OfferingName = offeringName,
                    OfferingImage = offeringImage,
                    OfferingDescription = offeringDescription
                };

                await _dbContext.Offerings.AddAsync(offeringEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.Offerings
                    .OrderByDescending(x => x.IdOffering)
                            .Select(x => x.IdOffering)
                                .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idOffering, int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.Offerings.FirstOrDefaultAsync(x => x.IdOffering == idOffering);

                if (entity != null)
                {
                    entity.IdRole = idRole;
                    entity.IdCategory = idCategory;
                    entity.IdRarity = idRarity;
                    entity.OfferingName = offeringName;
                    entity.OfferingImage = offeringImage;
                    entity.OfferingDescription = offeringDescription;

                    _dbContext.Offerings.Update(entity);
                    _dbContext.SaveChanges();

                    return idOffering;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить Offering на уровне базы данных для Id: {idOffering}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idOffering)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.Offerings
                            .Where(x => x.IdOffering == idOffering)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<OfferingDomain?> GetAsync(int idOffering)
        {
            using (var _dbContext = _contextFactory())
            {
                var offeringEntity = await _dbContext.Offerings
                        .FirstOrDefaultAsync(x => x.IdOffering == idOffering);

                if (offeringEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Killer для Id: {idOffering}");
                    return null;
                }

                var (CreatedOffering, Message) = OfferingDomain.Create(
                    offeringEntity.IdOffering, 
                    offeringEntity.IdRole, 
                    offeringEntity.IdCategory,
                    offeringEntity.IdRarity,
                    offeringEntity.OfferingName, 
                    offeringEntity.OfferingImage, 
                    offeringEntity.OfferingDescription);

                if (CreatedOffering == null)
                {
                    Debug.WriteLine($"Не удалось создать KillerDomain для Id: {idOffering}. Ошибка: {Message}");
                    return null;
                }

                return CreatedOffering;
            }
        }

        public async Task<IEnumerable<OfferingDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var offeringEntities = await _dbContext.Offerings.AsNoTracking().ToListAsync();

                var offeringsDomain = new List<OfferingDomain>();

                foreach (var offeringEntity in offeringEntities)
                {
                    if (offeringEntity == null)
                    {
                        Debug.WriteLine($"Не удалось получить элемент Offering из БД");
                        continue;
                    }

                    var id = offeringEntity.IdOffering;

                    var (CreatedOffering, Message) = OfferingDomain.Create(
                        offeringEntity.IdOffering,
                        offeringEntity.IdRole, 
                        offeringEntity.IdCategory, 
                        offeringEntity.IdRarity,
                        offeringEntity.OfferingName,
                        offeringEntity.OfferingImage, 
                        offeringEntity.OfferingDescription);

                    if (CreatedOffering == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции OfferingDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    offeringsDomain.Add(CreatedOffering);
                }

                return offeringsDomain;
            }
        }

        public IEnumerable<OfferingDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string offeringName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Offerings.AnyAsync(x => x.OfferingName == offeringName);
            }
        }

        public async Task<bool> ExistAsync(int idPOffering)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Offerings.AnyAsync(x => x.IdOffering == idPOffering);
            }
        }

        public bool Exist(string offeringName)
        {
            return Task.Run(() => ExistAsync(offeringName)).Result;
        }

        public bool Exist(int idOffering)
        {
            return Task.Run(() => ExistAsync(idOffering)).Result;
        }
    }
}