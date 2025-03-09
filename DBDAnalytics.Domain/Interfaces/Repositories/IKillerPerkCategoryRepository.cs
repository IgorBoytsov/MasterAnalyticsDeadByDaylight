using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IKillerPerkCategoryRepository
    {
        Task<int> CreateAsync(string killerPerkCategoryName, string? description);
        Task<int> DeleteAsync(int idKillerPerkCategory);
        bool Exist(int idKillerPerkCategory);
        bool Exist(string killerPerkCategoryName);
        Task<bool> ExistAsync(int idKillerPerkCategory);
        Task<bool> ExistAsync(string killerPerkCategoryName);
        IEnumerable<KillerPerkCategoryDomain> GetAll();
        Task<IEnumerable<KillerPerkCategoryDomain>> GetAllAsync();
        Task<KillerPerkCategoryDomain?> GetAsync(int idKillerPerkCategory);
        Task<int> UpdateAsync(int idKillerPerkCategory, string killerPerkCategoryName, string? description);
    }
}