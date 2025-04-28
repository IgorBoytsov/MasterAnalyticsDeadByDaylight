using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Domain.Enums;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ICalculationOfferingService
    {
        List<DetailsMatchKillerDTO> GetKillers(List<DetailsMatchDTO> matches, int idOffering, Associations association);
        List<DetailsMatchSurvivorDTO> GetSurvivors(List<DetailsMatchDTO> matches, int idOffering, Associations association);
    }
}