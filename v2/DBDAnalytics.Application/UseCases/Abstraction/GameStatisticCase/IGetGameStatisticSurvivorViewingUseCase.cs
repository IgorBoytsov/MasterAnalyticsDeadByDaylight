using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase
{
    public interface IGetGameStatisticSurvivorViewingUseCase
    {
        GameStatisticSurvivorViewingDTO GetSurvivorView(int idGameStatistic);
        Task<GameStatisticSurvivorViewingDTO> GetSurvivorViewAsync(int idGameStatistic);
        Task<List<GameStatisticSurvivorViewingDTO>> GetSurvivorViewsAsync(GameStatisticSurvivorFilterDTO filter);
        public DateTime? GetLastDateMatch();
    }
}