using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.RoleCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class RoleService(ICreateRoleUseCase createRoleUseCase,
                             IDeleteRoleUseCase deleteRoleUseCase,
                             IGetRoleUseCase getRoleUseCase,
                             IUpdateRoleUseCase updateRoleUseCase) : IRoleService
    {
        private readonly ICreateRoleUseCase _createRoleUseCase = createRoleUseCase;
        private readonly IDeleteRoleUseCase _deleteRoleUseCase = deleteRoleUseCase;
        private readonly IGetRoleUseCase _getRoleUseCase = getRoleUseCase;
        private readonly IUpdateRoleUseCase _updateRoleUseCase = updateRoleUseCase;

        public async Task<(RoleDTO? RoleDTO, string Message)> CreateAsync(string roleName, string roleDescription)
        {
            return await _createRoleUseCase.CreateAsync(roleName, roleDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idRole)
        {
            return await _deleteRoleUseCase.DeleteAsync(idRole);
        }

        public List<RoleDTO> GetAll()
        {
            return _getRoleUseCase.GetAll();
        }

        public async Task<List<RoleDTO>> GetAllAsync()
        {
            return await _getRoleUseCase.GetAllAsync();
        }

        public async Task<RoleDTO> GetAsync(int idRole)
        {
            return await _getRoleUseCase.GetAsync(idRole);
        }

        public async Task<(RoleDTO? RoleDTO, string? Message)> UpdateAsync(int idRole, string roleName, string roleDescription)
        {
            return await _updateRoleUseCase.UpdateAsync(idRole, roleName, roleDescription);
        }

        public async Task<RoleDTO> ForcedUpdateAsync(int idRole, string roleName, string roleDescription)
        {
            return await _updateRoleUseCase.ForcedUpdateAsync(idRole, roleName, roleDescription);
        }
    }
}
