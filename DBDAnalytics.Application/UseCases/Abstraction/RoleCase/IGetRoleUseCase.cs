using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.RoleCase
{
    public interface IGetRoleUseCase
    {
        List<RoleDTO> GetAll();
        Task<List<RoleDTO>> GetAllAsync();
        Task<RoleDTO?> GetAsync(int idRole);
    }
}