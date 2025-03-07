using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.RoleCase
{
    public interface ICreateRoleUseCase
    {
        Task<(RoleDTO? RoleDTO, string? Message)> CreateAsync(string roleName, string roleDescription);
    }
}