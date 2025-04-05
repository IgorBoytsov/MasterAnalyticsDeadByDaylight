using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs.DetailsDTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ICalculationKillerService
    {
        List<LabeledValue> DetailingHooks(List<DetailsMatchDTO> matches, string displayNameFormat);
        List<LabeledValue> DetailingKills(List<DetailsMatchDTO> matches, string displayNameFormat);
    }
}