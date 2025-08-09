using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IGameModeRepository
    {
        Task<int> CreateAsync(string gameModeName, string gameModeDescription);
        Task<int> DeleteAsync(int idGameMode);
        Task<GameModeDomain?> GetAsync(int idGameMode);
        Task<IEnumerable<GameModeDomain>> GetAllAsync();
        IEnumerable<GameModeDomain> GetAll();
        Task<int> UpdateAsync(int idGameMode, string gameModeName, string gameModeDescription);
        bool Exist(string gameModeName);
        bool Exist(int idGameMode);
        Task<bool> ExistAsync(string gameModeName);
        Task<bool> ExistAsync(int idGameMode);
    }
}