using DBDAnalytics.Application.DTOs.DetailsDTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.StatisticCase
{
    public interface IGetDetailsMatchUseCase
    {
        Task<(List<DetailsMatchDTO> KillerDetails, int TotalMatches)> GetDetailsMatch(int idKiller, int idAssociation);
    }
}