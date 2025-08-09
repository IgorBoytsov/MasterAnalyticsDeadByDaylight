using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Domain.Enums;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ICalculationItemService
    {
        List<DetailsMatchSurvivorDTO> GetSurvivors(List<DetailsMatchDTO> matches, int idItem, Associations association);
    }
}