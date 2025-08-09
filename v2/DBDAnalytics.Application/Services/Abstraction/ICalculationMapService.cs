using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ICalculationMapService
    {
        List<LabeledValue> DetailingWhoPlaceMap(List<DetailsMatchDTO> matches, List<WhoPlacedMapDTO> whoPlacedMaps, bool isWin = false);
    }
}