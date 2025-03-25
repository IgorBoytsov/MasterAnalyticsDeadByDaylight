using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IGameStatisticSurvivorViewingRepository
    {
        GameStatisticSurvivorViewingDomain GetSurvivorView(int idGameStatistic);
        Task<GameStatisticSurvivorViewingDomain> GetSurvivorViewAsync(int idGameStatistic);
        Task<List<GameStatisticSurvivorViewingDomain>> GetSurvivorViewsAsync(GameStatisticSurvivorFilterDomain filter);
    }
}