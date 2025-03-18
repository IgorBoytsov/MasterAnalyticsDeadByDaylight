using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase
{
    public interface IGetGameStatisticUseCase
    {
        Task<List<GameStatisticKillerViewingDTO>> GetKillerViewsAsync();
        Task<GameStatisticKillerViewingDTO> GetKillerViewAsync(int idGameStatistic);
        GameStatisticKillerViewingDTO GetKillerView(int idGameStatistic);
        Task<List<GameStatisticSurvivorViewingDTO>> GetSurvivorViewsAsync();
        Task<GameStatisticSurvivorViewingDTO> GetSurvivorViewAsync(int idGameStatistic);
        GameStatisticSurvivorViewingDTO GetSurvivorView(int idGameStatistic);
    }
}