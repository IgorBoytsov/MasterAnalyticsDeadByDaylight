using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class SurvivorPerkRepository(Func<DBDContext> context) : ISurvivorPerkRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        public async Task<int> CreateAsync(int idSurvivor, string PerkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var perkEntity = new SurvivorPerk
                {
                    IdSurvivor = idSurvivor,
                    PerkName = PerkName,
                    PerkImage = perkImage,
                    IdCategory = idCategory,
                    PerkDescription = perkDescription
                };

                await _dbContext.SurvivorPerks.AddAsync(perkEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.SurvivorPerks
                    .Where(x => x.IdSurvivor == idSurvivor)
                        .OrderByDescending(x => x.IdSurvivorPerk)
                            .Select(x => x.IdSurvivorPerk)
                                .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idPerk, int idSurvivor, string PerkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.SurvivorPerks.FirstOrDefaultAsync(x => x.IdSurvivorPerk == idPerk);

                entity.IdSurvivor = idSurvivor;
                entity.PerkName = PerkName;
                entity.PerkImage = perkImage;
                entity.IdCategory = idCategory;
                entity.PerkDescription = perkDescription;

                _dbContext.SurvivorPerks.Update(entity);
                _dbContext.SaveChanges();

                return idPerk;
            }
        }

        public async Task<int> DeleteAsync(int idPerk)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.SurvivorPerks
                            .Where(x => x.IdSurvivorPerk == idPerk)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<SurvivorPerkDomain?> GetAsync(int idPerk)
        {
            using (var _dbContext = _contextFactory())
            {
                var perkEntity = await _dbContext.SurvivorPerks.FirstOrDefaultAsync(x => x.IdSurvivorPerk == idPerk);

                if (perkEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить SurvivorPerk для Id: {idPerk}");
                    return null;
                }

                var (CreatedSurvivorPerk, Message) = SurvivorPerkDomain.Create(
                    perkEntity.IdSurvivorPerk, 
                    perkEntity.IdSurvivor, 
                    perkEntity.PerkName, 
                    perkEntity.PerkImage, 
                    perkEntity.IdCategory,
                    perkEntity.PerkDescription);

                if (CreatedSurvivorPerk == null)
                {
                    Debug.WriteLine($"Не удалось создать SurvivorPerkDomain для Id: {idPerk}. Ошибка: {Message}");
                    return null;
                }

                return CreatedSurvivorPerk;
            }
        }

        public async Task<IEnumerable<SurvivorPerkDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var perksEntity = await _dbContext.SurvivorPerks.ToListAsync();
                var perksDomain = new List<SurvivorPerkDomain>();

                foreach (var perkEntity in perksEntity)
                {
                    if (perkEntity == null)
                    {
                        Debug.WriteLine($"Не удалось получить элемент SurvivorPerk из БД");
                        continue;
                    }

                    var id = perkEntity.IdSurvivorPerk;

                    var (CreatedSurvivorPerk, Message) = SurvivorPerkDomain.Create(
                        perkEntity.IdSurvivorPerk, 
                        perkEntity.IdSurvivor, 
                        perkEntity.PerkName,
                        perkEntity.PerkImage,
                        perkEntity.IdCategory, 
                        perkEntity.PerkDescription);

                    if (CreatedSurvivorPerk == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции SurvivorPerkDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    perksDomain.Add(CreatedSurvivorPerk);
                }

                return perksDomain;
            }
        }

        public IEnumerable<SurvivorPerkDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(int idPerk)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.SurvivorPerks.AnyAsync(x => x.IdSurvivorPerk == idPerk);
            }
        }

        public bool Exist(int idPerk)
        {
            return Task.Run(() => ExistAsync(idPerk)).Result;
        }
    }
}