using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<int> CreateAsync(string roleName, string roleDescription);
        Task<int> DeleteAsync(int idRole);
        Task<RoleDomain?> GetAsync(int idRole);
        Task<IEnumerable<RoleDomain>> GetAllAsync();
        IEnumerable<RoleDomain> GetAll();
        Task<int> UpdateAsync(int idRole, string roleName, string roleDescription);
        bool Exist(string roleName);
        bool Exist(int idRole);
        Task<bool> ExistAsync(string roleName);
        Task<bool> ExistAsync(int idRole);
    }
}