using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class RoleRepository(Func<DBDContext> context) : IRoleRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public async Task<int> CreateAsync(string roleName, string roleDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var roleEntity = new Role
                {
                    RoleName = roleName,
                    RoleDescription = roleDescription
                };

                await _dbContext.Roles.AddAsync(roleEntity);
                await _dbContext.SaveChangesAsync();

                int id = await _dbContext.Roles
                        .Where(r => r.RoleName == roleName)
                            .Select(r => r.IdRole)
                                .FirstOrDefaultAsync();

                return id;
            }
        }

        public async Task<int> UpdateAsync(int idRole, string roleName, string roleDescription)
        {
            using (var _dbContext = _contextFactory())
            {
                var entity = await _dbContext.Roles.FirstOrDefaultAsync(x => x.IdRole == idRole);

                if (entity != null)
                {
                    entity.RoleName = roleName;
                    entity.RoleDescription = roleDescription;

                    _dbContext.Roles.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    return idRole;
                }
                else
                {
                    Debug.WriteLine($"Не удалось обновить Role на уровне базы данных для Id: {idRole}");
                    return -1;
                }
            }
        }

        public async Task<int> DeleteAsync(int idRole)
        {
            using (var _dbContext = _contextFactory())
            {
                var id = await _dbContext.Roles
                            .Where(r => r.IdRole == idRole)
                                .ExecuteDeleteAsync();

                return id;
            }
        }

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<RoleDomain?> GetAsync(int idRole)
        {
            using (var _dbContext = _contextFactory())
            {
                var roleEntity = await _dbContext.Roles.FirstOrDefaultAsync(m => m.IdRole == idRole);

                if (roleEntity == null)
                {
                    Debug.WriteLine($"Не удалось получить Role для Id: {idRole}");
                    return null;
                }

                var (CreatedRole, Message) = RoleDomain.Create(roleEntity.IdRole, roleEntity.RoleName, roleEntity.RoleDescription);

                if (CreatedRole == null)
                {
                    Debug.WriteLine($"Не удалось создать Role для Id: {idRole}. Ошибка: {Message}");
                    return null;
                }

                return CreatedRole;
            }
        }

        public async Task<IEnumerable<RoleDomain>> GetAllAsync()
        {
            using (var _dbContext = _contextFactory())
            {
                var roleEntities = await _dbContext.Roles.ToListAsync();

                var roles = new List<RoleDomain>();

                foreach (var role in roleEntities)
                {
                    if (role == null)
                    {
                        Debug.WriteLine($"Не удалось получить элемент Role из БД");
                        continue;
                    }

                    var id = role.IdRole;

                    var (CreatedRole, Message) = RoleDomain.Create(role.IdRole, role.RoleName, role.RoleDescription);

                    if (CreatedRole == null)
                    {
                        Debug.WriteLine($"Не удалось создать элемент коллекции RoleDomain для Id {id} при получение из БД. Ошибка: {Message}");
                        continue;
                    }

                    roles.Add(CreatedRole);
                }

                return roles;
            }
        }

        public IEnumerable<RoleDomain> GetAll()
        {
            return Task.Run(() => GetAllAsync()).Result;
        }

        /*--Exist-----------------------------------------------------------------------------------------*/

        public async Task<bool> ExistAsync(string roleName)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Roles.AnyAsync(x => x.RoleName == roleName);
            }
        }

        public async Task<bool> ExistAsync(int idRole)
        {
            using (var _dbContext = _contextFactory())
            {
                return await _dbContext.Roles.AnyAsync(x => x.IdRole == idRole);
            }
        }

        public bool Exist(string roleName)
        {
            return Task.Run(() => ExistAsync(roleName)).Result;
        }

        public bool Exist(int idRole)
        {
            return Task.Run(() => ExistAsync(idRole)).Result;
        }
    }
}