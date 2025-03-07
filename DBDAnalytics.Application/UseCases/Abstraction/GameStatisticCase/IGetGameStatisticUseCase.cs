using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase
{
    public interface IGetGameStatisticUseCase
    {
        Task<GameStatisticViewingDTO> GetAsync(int idGameStatistic);
        GameStatisticViewingDTO? Get(int idGameStatistic);
        Task<List<GameStatisticViewingDTO>> GetViewsAsync();
    }
}