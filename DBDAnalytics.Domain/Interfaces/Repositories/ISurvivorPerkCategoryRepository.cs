using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface ISurvivorPerkCategoryRepository
    {
        Task<int> CreateAsync(string survivorPerkCategoryName);
        Task<int> DeleteAsync(int idSurvivorPerkCategory);
        bool Exist(int idSurvivorPerkCategory);
        bool Exist(string survivorPerkCategoryName);
        Task<bool> ExistAsync(int idSurvivorPerkCategory);
        Task<bool> ExistAsync(string survivorPerkCategoryName);
        IEnumerable<SurvivorPerkCategoryDomain> GetAll();
        Task<IEnumerable<SurvivorPerkCategoryDomain>> GetAllAsync();
        Task<SurvivorPerkCategoryDomain?> GetAsync(int idSurvivorPerkCategory);
        Task<int> UpdateAsync(int idSurvivorPerkCategory, string survivorPerkCategoryName);
    }
}