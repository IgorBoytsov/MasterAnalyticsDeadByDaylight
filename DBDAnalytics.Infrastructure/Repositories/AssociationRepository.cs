using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class AssociationRepository(Func<DBDContext> context) : IAssociationRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string playerAssociationName, string playerAssociationDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var playerAssociationEntity = new PlayerAssociation
                {
                    PlayerAssociationName = playerAssociationName,
                    PlayerAssociationDescription = playerAssociationDescription
                };

                await _dbContext.PlayerAssociations.AddAsync(playerAssociationEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.PlayerAssociations
                    .Where(x => x.PlayerAssociationName == playerAssociationName)
                        .Select(x => x.IdPlayerAssociation)
                            .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idPlayerAssociation, string playerAssociationName, string playerAssociationDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.PlayerAssociations.FirstOrDefaultAsync(x => x.IdPlayerAssociation == idPlayerAssociation);

                if (entity != null)
                {
                    entity.PlayerAssociationName = playerAssociationName;
                    entity.PlayerAssociationDescription = playerAssociationDescription;

                    _dbContext.PlayerAssociations.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    return idPlayerAssociation;

                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить PlayerAssociation для Id {idPlayerAssociation}");
                    return -1;
                }
               
            }
        }

        public async Task<int> DeleteAsync(int idPlayerAssociation)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.PlayerAssociations
                    .Where(x => x.IdPlayerAssociation == idPlayerAssociation)
                        .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<PlayerAssociationDomain?> GetAsync(int idPlayerAssociation)
        {
            using (var _dbContext = _contextFactory())
            {
                var playerAssociationEventEntity = await _dbContext.PlayerAssociations
                        .FirstOrDefaultAsync(x => x.IdPlayerAssociation == idPlayerAssociation);

                if (playerAssociationEventEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Killer для Id: {idPlayerAssociation}");
                    return null;
                }

                var (CreatedAssociation, Message) = PlayerAssociationDomain.Create(
                     playerAssociationEventEntity.IdPlayerAssociation,
                     playerAssociationEventEntity.PlayerAssociationName,
                     playerAssociationEventEntity.PlayerAssociationDescription);

                if (CreatedAssociation == null)
                {
                    Debug.WriteLine($"Не удалось создать PlayerAssociation для Id: {idPlayerAssociation}. Ошибка: {Message}");
                    return null;    
                }

                return CreatedAssociation;
            }
        }

        public async Task<IEnumerable<PlayerAssociationDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var playerAssociationEntities = await _dbContext.PlayerAssociations.AsNoTracking().ToListAsync();

                var playerAssociationsDomain = new List<PlayerAssociationDomain>();

                foreach (var playerAssociationEntity in playerAssociationEntities)
                {
                    int id = playerAssociationEntity.IdPlayerAssociation;

                    var (CreatedPlayerAssociation, Message) = PlayerAssociationDomain.Create(
                        playerAssociationEntity.IdPlayerAssociation,
                        playerAssociationEntity.PlayerAssociationName,
                        playerAssociationEntity.PlayerAssociationDescription);

                    if (CreatedPlayerAssociation == null)
                        Debug.WriteLine($"Не удалось создать PlayerAssociationDomain для Id {id}. Ошибка: {Message}");
                    else
                        playerAssociationsDomain.Add(CreatedPlayerAssociation);
                }
                return playerAssociationsDomain;
            }
        }

        public IEnumerable<PlayerAssociationDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string playerAssociationName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.PlayerAssociations.AnyAsync(x => x.PlayerAssociationName == playerAssociationName);
            }   
        }

        public async Task<bool> ExistAsync(int idPlayerAssociation)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.PlayerAssociations.AnyAsync(x => x.IdPlayerAssociation == idPlayerAssociation);
            }
        }

        public bool Exist(string playerAssociationName)
        {
            return Task.Run(() => ExistAsync(playerAssociationName)).Result;
        }

        public bool Exist(int idPlayerAssociation)
        {
            return Task.Run(() => ExistAsync(idPlayerAssociation)).Result;
        }

        #region Вариант реализацйии асинхронного и синхроного метода №1

        private async Task<IEnumerable<PlayerAssociationDomain>> GetAllAsyncPrivate()
        {
            using (var _dbContext = _contextFactory())
            {
                var playerAssociationEntities = await _dbContext.PlayerAssociations.AsNoTracking().ToListAsync();
                return ConvertToDomainPrivate(playerAssociationEntities);
            }
        }

        private IEnumerable<PlayerAssociationDomain> GetAllPrivate()
        {
            using (var _dbContext = _contextFactory())
            {
                var playerAssociationEntities = _dbContext.PlayerAssociations.AsNoTracking().ToList();
                return ConvertToDomainPrivate(playerAssociationEntities);
            }
        }

        private List<PlayerAssociationDomain> ConvertToDomainPrivate(List<PlayerAssociation> entities)
        {
            var playerAssociationsDomain = new List<PlayerAssociationDomain>();

            foreach (var entity in entities)
            {
                int id = entity.IdPlayerAssociation;
                var domain = PlayerAssociationDomain.Create(
                    entity.IdPlayerAssociation,
                    entity.PlayerAssociationName,
                    entity.PlayerAssociationDescription).CreatedPlayerAssociationDomain;

                if (domain == null)
                    Console.WriteLine($"Не удалось создать PlayerAssociationDomain для Id {id}");
                else
                    playerAssociationsDomain.Add(domain);
            }

            return playerAssociationsDomain;
        }

        #endregion 
    }
}

//public async Task<IEnumerable<PlayerAssociationDomain>> GetAllAsync()
//{
//    using (var _dbContext = _contextFactory())
//    {
//        var playerAssociationEntities = await _dbContext.PlayerAssociations.AsNoTracking().ToListAsync();

//        var playerAssociationsDomain = playerAssociationEntities
//            .Select(playerAssociationEntity => PlayerAssociationDomain.Create(
//                playerAssociationEntity.IdPlayerAssociation,
//                playerAssociationEntity.PlayerAssociationName,
//                playerAssociationEntity.PlayerAssociationDescription))
//            .Select(result => result.CreatedPlayerAssociationDomain)
//            .Where(domain => domain != null)
//            .Select(domain => domain!);

//        return playerAssociationsDomain;
//    }
//}