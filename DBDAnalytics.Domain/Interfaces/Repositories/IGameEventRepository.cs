using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IGameEventRepository
    {
        Task<int> CreateAsync(string gameEventName, string gameEventDescription);
        Task<int> DeleteAsync(int idGameEvent);
        Task<GameEventDomain?> GetAsync(int idGameEvent);
        IEnumerable<GameEventDomain> GetAll();
        Task<IEnumerable<GameEventDomain>> GetAllAsync();
        Task<int> UpdateAsync(int idGameEvent, string gameEventName, string gameEventDescription);
        bool Exist(string gameEventName);
        bool Exist(int idGameEvent);
        Task<bool> ExistAsync(string gameEventName);
        Task<bool> ExistAsync(int idGameEvent);
    }
}