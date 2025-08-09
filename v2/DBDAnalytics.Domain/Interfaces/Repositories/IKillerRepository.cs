using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IKillerRepository
    {
        Task<int> CreateAsync(string killerName, byte[]? killerImage, byte[]? killerAbilityImage);
        Task<int> DeleteAsync(int idKiller);
        bool Exist(int idKiller);
        bool Exist(string killerName);
        Task<bool> ExistAsync(int idKiller);
        Task<bool> ExistAsync(string killerName);
        IEnumerable<KillerDomain> GetAll();
        Task<IEnumerable<KillerDomain>> GetAllAsync();
        Task<KillerDomain?> GetAsync(int idKiller);
        Task<IEnumerable<KillerLoadoutDomain>> GetKillersWithAddonsAndPerksAsync();
        Task<KillerLoadoutDomain?> GetKillerWithAddonsAndPerksAsync(int idKiller);
        Task<int> UpdateAsync(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage);
    }
}