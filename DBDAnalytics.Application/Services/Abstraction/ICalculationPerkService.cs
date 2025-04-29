using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Domain.Enums;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ICalculationPerkService
    {
        List<DetailsMatchKillerDTO> GetKillers(List<DetailsMatchDTO> matches, int idPerk, Associations association);
        List<DetailsMatchSurvivorDTO> GetSurvivors(List<DetailsMatchDTO> matches, int idPerk, Associations association);
    }
}