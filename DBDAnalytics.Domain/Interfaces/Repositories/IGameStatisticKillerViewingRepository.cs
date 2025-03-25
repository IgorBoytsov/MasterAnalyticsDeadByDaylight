using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IGameStatisticKillerViewingRepository
    {
        GameStatisticKillerViewingDomain GetKillerView(int idGameStatistic);
        Task<GameStatisticKillerViewingDomain> GetKillerViewAsync(int idGameStatistic);
        Task<List<GameStatisticKillerViewingDomain>> GetKillerViewsFilteredAsync(GameStatisticKillerFilterDomain filter);
    }
}