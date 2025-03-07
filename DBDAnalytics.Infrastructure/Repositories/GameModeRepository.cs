using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class GameModeRepository(Func<DBDContext> context) : IGameModeRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string gameModeName, string gameModeDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var gameModeEntity = new GameMode
                {
                    GameModeName = gameModeName,
                    GameModeDescription = gameModeDescription
                };

                await _dbContext.GameModes.AddAsync(gameModeEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.GameModes
                        .Where(m => m.GameModeName == gameModeName)
                            .Select(m => m.IdGameMode)
                                .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idGameMode, string gameModeName, string gameModeDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.GameModes.FirstOrDefaultAsync(x => x.IdGameMode == idGameMode);

                if (entity != null)
                {
                    entity.GameModeName = gameModeName;
                    entity.GameModeDescription = gameModeDescription;

                    _dbContext.GameModes.Update(entity);
                    _dbContext.SaveChanges();

                    return idGameMode;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить GameMode на уровне базы данных для Id: {idGameMode}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idGameMode)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.GameModes
                            .Where(m => m.IdGameMode == idGameMode)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<GameModeDomain?> GetAsync(int idGameMode)
        {
            using (var _dbContext = _contextFactory())
            {
                var gameModeEntity = await _dbContext.GameModes.FirstOrDefaultAsync(m => m.IdGameMode == idGameMode);

                if (gameModeEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить GameMode для Id: {idGameMode}");
                    return null;
                }

                var (CreatedGameMode, Message) = GameModeDomain.Create(
                    gameModeEntity.IdGameMode,
                    gameModeEntity.GameModeName,
                    gameModeEntity.GameModeDescription);

                if (CreatedGameMode == null)
                {
                    Debug.WriteLine($"Не удалось создать KillerDomain для Id: {idGameMode}. Ошибка: {Message}");
                    return null;
                }

                return CreatedGameMode;
            }
        }

        public async Task<IEnumerable<GameModeDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var gameModeEntities = await _dbContext.GameModes.ToListAsync();

                var gameModes = new List<GameModeDomain>();

                foreach (var gameModeEntity in gameModeEntities)
                {
                    var id = gameModeEntity.IdGameMode;

                    var (CreatedGameMode, Message) = GameModeDomain.Create(
                        gameModeEntity.IdGameMode, 
                        gameModeEntity.GameModeName, 
                        gameModeEntity.GameModeDescription);

                    if (CreatedGameMode == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции GameModeDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    gameModes.Add(CreatedGameMode);
                }

                return gameModes;
            }
        }

        public IEnumerable<GameModeDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string gameModeName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.GameModes.AnyAsync(e => e.GameModeName == gameModeName);
            }
        }

        public async Task<bool> ExistAsync(int idGameMode)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.GameModes.AnyAsync(e => e.IdGameMode == idGameMode);
            }
        }

        public bool Exist(string gameModeName)
        {
            return Task.Run(() => ExistAsync(gameModeName)).Result;
        }

        public bool Exist(int idGameMode)
        {
            return Task.Run(() => ExistAsync(idGameMode)).Result;
        }
    }
}