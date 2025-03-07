using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IOfferingCategoryRepository
    {
        Task<int> CreateAsync(string offeringCategoryName);
        Task<int> DeleteAsync(int idOfferingCategory);
        bool Exist(int idOfferingCategory);
        bool Exist(string offeringCategoryName);
        Task<bool> ExistAsync(int idOfferingCategory);
        Task<bool> ExistAsync(string offeringCategoryName);
        IEnumerable<OfferingCategoryDomain> GetAll();
        Task<IEnumerable<OfferingCategoryDomain>> GetAllAsync();
        Task<OfferingCategoryDomain?> GetAsync(int idOfferingCategory);
        Task<int> UpdateAsync(int idOfferingCategory, string offeringCategoryName);
    }
}