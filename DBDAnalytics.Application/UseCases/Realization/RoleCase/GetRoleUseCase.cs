using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.RoleCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.RoleCase
{
    public class GetRoleUseCase(IRoleRepository roleRepository) : IGetRoleUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<List<RoleDTO>> GetAllAsync()
        {
            var domainEntities = await _roleRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<RoleDTO> GetAll()
        {
            var domainEntities = _roleRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<RoleDTO?> GetAsync(int idRole)
        {
            var domainEntity = await _roleRepository.GetAsync(idRole);

            if (domainEntity == null)
            {
                Debug.WriteLine($"Role с ID {idRole} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}