using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Domain.Enums;

namespace DBDAnalytics.Application.UseCases.Abstraction.StatisticCase
{
    public interface IGetDetailsMatchUseCase
    {
        Task<(List<DetailsMatchDTO> KillerDetails, int TotalMatches)> GetDetailsMatch(int idEntity, Associations associations, FilterParameter filterParameter);
        Task<(List<DetailsMatchDTO> KillerDetails, int TotalMatches)> GetDetailsMatch(List<int> idsMatches);
    }
}