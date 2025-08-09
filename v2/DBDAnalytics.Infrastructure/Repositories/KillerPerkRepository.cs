using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class KillerPerkRepository(Func<DBDContext> context) : IKillerPerkRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(int idKiller, string PerkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var perkEntity = new KillerPerk
                {
                    IdKiller = idKiller,
                    PerkName = PerkName,
                    PerkImage = perkImage,
                    IdCategory = idCategory,
                    PerkDescription = perkDescription
                };

                await _dbContext.KillerPerks.AddAsync(perkEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.KillerPerks
                    .Where(x => x.IdKiller == idKiller)
                        .OrderByDescending(x => x.IdKillerPerk)
                            .Select(x => x.IdKillerPerk)
                                .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idPerk, int idKiller, string PerkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.KillerPerks.FirstOrDefaultAsync(x => x.IdKillerPerk == idPerk);

                if (entity != null)
                {
                    entity.PerkName = PerkName;
                    entity.PerkImage = perkImage;
                    entity.IdCategory = idCategory;
                    entity.PerkDescription = perkDescription;

                    _dbContext.KillerPerks.Update(entity);
                    _dbContext.SaveChanges();

                    return idPerk;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить KillerPerk на уровне базы данных для Id: {idPerk}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idPerk)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.KillerPerks
                            .Where(x => x.IdKillerPerk == idPerk)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<KillerPerkDomain?> GetAsync(int idPerk)
        {
            using (var _dbContext = _contextFactory())
            {
                var perkEntity = await _dbContext.KillerPerks.FirstOrDefaultAsync(x => x.IdKillerPerk == idPerk);

                if (perkEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить KillerPerk для Id: {idPerk}");
                    return null;
                }

                var (CreatedKillerPerk, Message) = KillerPerkDomain.Create(
                    perkEntity.IdKillerPerk, 
                    perkEntity.IdKiller, 
                    perkEntity.PerkName, 
                    perkEntity.PerkImage, 
                    perkEntity.IdCategory, 
                    perkEntity.PerkDescription);

                if (CreatedKillerPerk == null)
                {
                    Debug.WriteLine($"Не удалось создать KillerPerkDomain для Id: {idPerk}. Ошибка: {Message}");
                    return null;
                }

                return CreatedKillerPerk;
            }
        }

        public async Task<IEnumerable<KillerPerkDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var perksEntity = await _dbContext.KillerPerks.ToListAsync();
                var perksDomain = new List<KillerPerkDomain>();

                foreach (var perkEntity in perksEntity)
                {
                    var id = perkEntity.IdKillerPerk;

                    var (CreatedKillerPerk, Message) = KillerPerkDomain.Create(
                        perkEntity.IdKillerPerk,
                        perkEntity.IdKiller,
                        perkEntity.PerkName,
                        perkEntity.PerkImage,
                        perkEntity.IdCategory,
                        perkEntity.PerkDescription);

                    if (CreatedKillerPerk == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции KillerPerkDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    perksDomain.Add(CreatedKillerPerk);
                }

                return perksDomain;
            }
        }

        public IEnumerable<KillerPerkDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(int idPerk)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.KillerPerks.AnyAsync(x => x.IdKillerPerk == idPerk);
            }
        }

        public bool Exist(int idPerk)
        {
            return Task.Run(() => ExistAsync(idPerk)).Result;
        }
    }
}