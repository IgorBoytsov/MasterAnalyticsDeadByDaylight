using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IRoleService
    {
        Task<(RoleDTO? RoleDTO, string Message)> CreateAsync(string roleName, string roleDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idRole);
        List<RoleDTO> GetAll();
        Task<List<RoleDTO>> GetAllAsync();
        Task<RoleDTO> GetAsync(int idRole);
        Task<(RoleDTO? RoleDTO, string? Message)> UpdateAsync(int idRole, string roleName, string roleDescription);
        Task<RoleDTO> ForcedUpdateAsync(int idRole, string roleName, string roleDescription);
    }
}