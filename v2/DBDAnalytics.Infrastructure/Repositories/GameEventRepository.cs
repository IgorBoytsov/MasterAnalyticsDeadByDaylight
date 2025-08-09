using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class GameEventRepository(Func<DBDContext> context) : IGameEventRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string gameEventName, string gameEventDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var gameEventEntity = new GameEvent
                {
                    GameEventName = gameEventName,
                    GameEventDescription = gameEventDescription
                };

                await _dbContext.GameEvents.AddAsync(gameEventEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.GameEvents
                    .Where(x => x.GameEventName == gameEventName)
                        .Select(x => x.IdGameEvent)
                            .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idGameEvent, string gameEventName, string gameEventDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.GameEvents.FirstOrDefaultAsync(x => x.IdGameEvent == idGameEvent);

                if (entity != null)
                {
                    entity.GameEventName = gameEventName;
                    entity.GameEventDescription = gameEventDescription;

                    _dbContext.GameEvents.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    return idGameEvent;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить GameEvent на уровне базы данных для Id: {idGameEvent}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idGameEvent)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.GameEvents
                            .Where(x => x.IdGameEvent == idGameEvent)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<GameEventDomain?> GetAsync(int idGameEvent)
        {
            using (var _dbContext = _contextFactory())
            {
                var gameEventEntity = await _dbContext.GameEvents.FirstOrDefaultAsync(e => e.IdGameEvent == idGameEvent);

                if (gameEventEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить GameEvent для Id: {idGameEvent}");
                    return null;
                }

                var (CreatedGameEvent, Message) = GameEventDomain.Create(
                     gameEventEntity.IdGameEvent,
                     gameEventEntity.GameEventName,
                     gameEventEntity.GameEventDescription);

                if (CreatedGameEvent == null)
                {
                    Debug.WriteLine($"Не удалось создать GameEvent для Id: {idGameEvent}. Ошибка: {Message}");
                    return null;

                }

                return CreatedGameEvent;
            }
        }

        public async Task<IEnumerable<GameEventDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var gameEventEntities = await _dbContext.GameEvents.AsNoTracking().ToListAsync();

                var gameEventDTOs = new List<GameEventDomain>();

                foreach (var gameEventEntity in gameEventEntities)
                {
                    var id = gameEventEntity.IdGameEvent;

                    var (CreatedGameEvent, Message) = GameEventDomain.Create(
                        gameEventEntity.IdGameEvent, 
                        gameEventEntity.GameEventName, 
                        gameEventEntity.GameEventDescription);

                    if (CreatedGameEvent == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции GameEventDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    gameEventDTOs.Add(CreatedGameEvent);
                }

                return gameEventDTOs;
            }
        }

        public IEnumerable<GameEventDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string gameEventName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.GameEvents.AnyAsync(e => e.GameEventName == gameEventName);
            }
        }

        public async Task<bool> ExistAsync(int idGameEvent)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.GameEvents.AnyAsync(e => e.IdGameEvent == idGameEvent);
            }
        }

        public bool Exist(string gameEventName)
        {
            return Task.Run(() => ExistAsync(gameEventName)).Result;
        }

        public bool Exist(int idGameEvent)
        {
            return Task.Run(() => ExistAsync(idGameEvent)).Result;
        }
    }
}