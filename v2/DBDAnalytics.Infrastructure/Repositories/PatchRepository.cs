using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class PatchRepository(Func<DBDContext> context) : IPatchRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        public async Task<int> CreateAsync(string patchNumber, DateOnly patchDateRelease, string? description)
        {
            using (var _dbContext = _contextFactory())
            {
                var patchEntity = new Patch
                {
                    PatchNumber = patchNumber,
                    PatchDateRelease = patchDateRelease,
                    Description = description
                };

                await _dbContext.Patches.AddAsync(patchEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.Patches
                    .Where(x => x.PatchNumber == patchNumber)
                        .Select(x => x.IdPatch)
                            .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idPatch, string patchNumber, DateOnly patchDateRelease, string? description)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.Patches.FirstOrDefaultAsync(x => x.IdPatch == idPatch);

                if (entity != null)
                {
                    entity.PatchNumber = patchNumber;
                    entity.PatchDateRelease = patchDateRelease;
                    entity.Description = description;

                    _dbContext.Patches.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    return idPatch;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить Killer на уровне базы данных для Id: {idPatch}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idPatch)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.Patches
                    .Where(x => x.IdPatch == idPatch)
                        .ExecuteDeleteAsync();

                return id;
            }

        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<PatchDomain?> GetAsync(int idPatch)
        {
            using (var _dbContext = _contextFactory())
            {
                var patchEntity = await _dbContext.Patches
                        .FirstOrDefaultAsync(x => x.IdPatch == idPatch);

                if (patchEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Killer для Id: {idPatch}");
                    return null;
                }

                var (CreatedPath, Message) = PatchDomain.Create(
                    patchEntity.IdPatch, 
                    patchEntity.PatchNumber, 
                    patchEntity.PatchDateRelease,
                    patchEntity.Description);

                if (CreatedPath == null)
                {
                    Debug.WriteLine($"Не удалось создать KillerDomain для Id: {idPatch}. Ошибка: {Message}");
                    return null;
                }

                return CreatedPath;
            }
        }

        public async Task<IEnumerable<PatchDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var patchesEntities = await _dbContext.Patches.AsNoTracking().ToListAsync();

                var patchesDomain = new List<PatchDomain>();

                foreach (var patchEntity in patchesEntities)
                {
                    if (patchEntity == null)
                    {
                        Debug.WriteLine($"Не удалось получить элемент Patch из БД");
                        continue;
                    }

                    var id = patchEntity.IdPatch;

                    var (CreatedPath, Message) = PatchDomain.Create(
                        patchEntity.IdPatch, 
                        patchEntity.PatchNumber, 
                        patchEntity.PatchDateRelease,
                        patchEntity.Description);

                    if (CreatedPath == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции PatchDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    patchesDomain.Add(CreatedPath);
                }

                return patchesDomain;
            }
        }

        public IEnumerable<PatchDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string patchNumber)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Patches.AnyAsync(x => x.PatchNumber == patchNumber);
            }
        }

        public async Task<bool> ExistAsync(int idPatch)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Patches.AnyAsync(x => x.IdPatch == idPatch);
            }
        }

        public bool Exist(string patchNumber)
        {
            using (var _dbContext = _contextFactory())
            {
                return Task.Run(() => ExistAsync(patchNumber)).Result;
            }
        }

        public bool Exist(int idPatch)
        {
            using (var _dbContext = _contextFactory())
            {
                return Task.Run(() => ExistAsync(idPatch)).Result;
            }
        }
    }
}