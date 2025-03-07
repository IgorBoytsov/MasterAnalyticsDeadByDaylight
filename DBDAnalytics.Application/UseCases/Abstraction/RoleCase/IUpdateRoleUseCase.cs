using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.RoleCase
{
    public interface IUpdateRoleUseCase
    {
        Task<(RoleDTO? RoleDTO, string? Message)> UpdateAsync(int idRole, string roleName, string roleDescription);
        Task<RoleDTO?> ForcedUpdateAsync(int idRole, string roleName, string roleDescription);
    }
}