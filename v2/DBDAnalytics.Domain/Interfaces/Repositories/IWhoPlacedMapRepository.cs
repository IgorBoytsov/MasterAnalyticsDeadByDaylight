using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IWhoPlacedMapRepository
    {
        Task<int> CreateAsync(string whoPlacedMapName, string? description);
        Task<int> DeleteAsync(int idWhoPlacedMap);
        bool Exist(int idWhoPlacedMap);
        bool Exist(string whoPlacedMapName);
        Task<bool> ExistAsync(int idWhoPlacedMap);
        Task<bool> ExistAsync(string whoPlacedMapName);
        IEnumerable<WhoPlacedMapDomain> GetAll();
        Task<IEnumerable<WhoPlacedMapDomain>> GetAllAsync();
        Task<WhoPlacedMapDomain?> GetAsync(int idWhoPlacedMap);
        Task<int> UpdateAsync(int idWhoPlacedMap, string whoPlacedMapName, string? description);
    }
}