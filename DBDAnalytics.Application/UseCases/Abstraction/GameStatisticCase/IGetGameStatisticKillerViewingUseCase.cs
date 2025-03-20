using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase
{
    public interface IGetGameStatisticKillerViewingUseCase
    {
        Task<GameStatisticKillerViewingDTO> GetKillerViewAsync(int idGameStatistic);
        GameStatisticKillerViewingDTO GetKillerView(int idGameStatistic);
        public DateTime? GetLastDateMatch();
        Task<List<GameStatisticKillerViewingDTO>> GetKillerViewsFilteredAsync(GameStatisticKillerFilterDTO filter);
    }
}