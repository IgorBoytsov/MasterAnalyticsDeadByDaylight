using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.RoleCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.RoleCase
{
    public class UpdateRoleUseCase(IRoleRepository roleRepository) : IUpdateRoleUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<(RoleDTO? RoleDTO, string? Message)> UpdateAsync(int idRole, string roleName, string roleDescription)
        {
            string message = string.Empty;

            if (idRole == 0 || string.IsNullOrWhiteSpace(roleName))
                return (null, "Такой записи не существует");

            var exist = await _roleRepository.ExistAsync(roleName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _roleRepository.UpdateAsync(idRole, roleName, roleDescription);

            var domainEntity = await _roleRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<RoleDTO?> ForcedUpdateAsync(int idRole, string roleName, string roleDescription)
        {
            int id = await _roleRepository.UpdateAsync(idRole, roleName, roleDescription);

            var domainEntity = await _roleRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}