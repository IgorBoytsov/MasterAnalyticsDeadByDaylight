using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IRarityRepository
    {
        Task<int> CreateAsync(string rarityName);
        Task<int> DeleteAsync(int idRarity);
        bool Exist(int idRarity);
        bool Exist(string rarityName);
        Task<bool> ExistAsync(int idRarity);
        Task<bool> ExistAsync(string rarityName);
        IEnumerable<RarityDomain> GetAll();
        Task<IEnumerable<RarityDomain>> GetAllAsync();
        Task<RarityDomain?> GetAsync(int idRarity);
        Task<int> UpdateAsync(int idRarity, string rarityName);
    }
}