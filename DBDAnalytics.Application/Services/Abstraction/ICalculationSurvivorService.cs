using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ICalculationSurvivorService
    {
        List<LabeledValue> DetailingAnonymous(List<DetailsMatchDTO> matches);
        List<LabeledValue> DetailingDisconnect(List<DetailsMatchDTO> matches);
        List<LabeledValue> DetailingPlatform(List<DetailsMatchDTO> matches, List<PlatformDTO> platforms);
        List<LabeledValue> DetailingTypeDeath(List<DetailsMatchDTO> matches, List<TypeDeathDTO> typeDeaths);
    }
}