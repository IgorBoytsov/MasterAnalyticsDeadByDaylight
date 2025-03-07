using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IMapRepository
    {
        Task<int> CreateAsync(int idMeasurement, string mapName, byte[] mapImage, string mapDescription);
        Task<int> DeleteAsync(int idMap);
        bool Exist(int idMap);
        bool Exist(string mapName);
        Task<bool> ExistAsync(int idMap);
        Task<bool> ExistAsync(string mapName);
        IEnumerable<MapDomain> GetAll();
        Task<IEnumerable<MapDomain>> GetAllAsync();
        Task<MapDomain?> GetAsync(int idMap);
        Task<int> UpdateAsync(int idMap, int idMeasurement, string mapName, byte[] mapImage, string mapDescription);
    }
}