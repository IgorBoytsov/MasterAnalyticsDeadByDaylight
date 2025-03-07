using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IMatchAttributeRepository
    {
        Task<int> CreateAsync(string attributeName, string? AttributeDescription, DateTime CreatedAt, bool isHide);
        Task<int> DeleteAsync(int idMatchAttribute);
        bool Exist(int idMatchAttribute);
        bool Exist(string attributeName);
        Task<bool> ExistAsync(int idMatchAttribute);
        Task<bool> ExistAsync(string attributeName);
        IEnumerable<MatchAttributeDomain> GetAll(bool isHide);
        Task<IEnumerable<MatchAttributeDomain>> GetAllAsync(bool isHide);
        Task<MatchAttributeDomain?> GetAsync(int idMatchAttribute);
        Task<int> UpdateAsync(int idMatchAttribute, string attributeName, string? AttributeDescription, bool isHide);
    }
}