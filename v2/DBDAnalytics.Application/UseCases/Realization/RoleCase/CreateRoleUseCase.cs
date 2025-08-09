using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.RoleCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.RoleCase
{
    public class CreateRoleUseCase(IRoleRepository roleRepository) : ICreateRoleUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<(RoleDTO? RoleDTO, string? Message)> CreateAsync(string roleName, string roleDescription)
        {
            string message = string.Empty;

            var (CreatedRole, Message) = RoleDomain.Create(0, roleName, roleDescription);

            if (CreatedRole is null)
            {
                return (null, Message);
            }

            bool exist = await _roleRepository.ExistAsync(roleName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _roleRepository.CreateAsync(roleName, roleDescription);

            var domainEntity = await _roleRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}