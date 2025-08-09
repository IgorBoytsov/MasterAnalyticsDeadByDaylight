namespace DBDAnalytics.Application.UseCases.Abstraction.RoleCase
{
    public interface IDeleteRoleUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idRole);
    }
}