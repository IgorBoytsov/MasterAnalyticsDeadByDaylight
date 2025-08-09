using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IOfferingRepository
    {
        Task<int> CreateAsync(int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription);
        Task<int> DeleteAsync(int idOffering);
        bool Exist(int idOffering);
        bool Exist(string offeringName);
        Task<bool> ExistAsync(int idPOffering);
        Task<bool> ExistAsync(string offeringName);
        IEnumerable<OfferingDomain> GetAll();
        Task<IEnumerable<OfferingDomain>> GetAllAsync();
        Task<OfferingDomain?> GetAsync(int idOffering);
        Task<int> UpdateAsync(int idOffering, int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription);
    }
}