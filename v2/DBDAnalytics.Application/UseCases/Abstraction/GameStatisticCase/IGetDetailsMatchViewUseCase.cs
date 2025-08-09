using DBDAnalytics.Application.DTOs.DetailsViewDTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase
{
    public interface IGetDetailsMatchViewUseCase
    {
        Task<DetailsMatchViewDTO> GetDetailsViewMatch(int idGameStatistics);
    }
}