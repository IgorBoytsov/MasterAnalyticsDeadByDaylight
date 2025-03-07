using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IPlatformRepository
    {
        Task<int> CreateAsync(string platformName, string platformDescription);
        Task<int> DeleteAsync(int idPlatform);
        bool Exist(int idPlatform);
        bool Exist(string platformName);
        Task<bool> ExistAsync(int idPlatform);
        Task<bool> ExistAsync(string platformName);
        IEnumerable<PlatformDomain> GetAll();
        Task<IEnumerable<PlatformDomain>> GetAllAsync();
        Task<PlatformDomain?> GetAsync(int idPlatform);
        Task<int> UpdateAsync(int idPlatform, string platformName, string platformDescription);
    }
}