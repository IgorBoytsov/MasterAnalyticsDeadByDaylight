using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Services.Abstraction;

namespace DBDAnalytics.Application.Services.Realization
{
    public class CalculationMapService(ICalculationGeneralService calculationGeneralService) : ICalculationMapService
    {
        private readonly ICalculationGeneralService _calculationGeneralService = calculationGeneralService;

        public List<LabeledValue> DetailingWhoPlaceMap(List<DetailsMatchDTO> matches, List<WhoPlacedMapDTO> whoPlacedMaps, bool isWin = false)
        {
            var values = new List<LabeledValue>();

            if (isWin)
            {
                values = whoPlacedMaps.Select(wpm => new LabeledValue
                {
                    Name = wpm.WhoPlacedMapName,
                    Value = _calculationGeneralService.Percentage(matches.Count(x => x.IdWhoPlaceMapWin == wpm.IdWhoPlacedMap), matches.Count),
                })
                .ToList();
            }
            else
            {
                values = whoPlacedMaps.Select(wpm => new LabeledValue
                {
                    Name = wpm.WhoPlacedMapName,
                    Value = _calculationGeneralService.Percentage(matches.Count(x => x.IdWhoPlaceMap == wpm.IdWhoPlacedMap), matches.Count),
                })
                .ToList();
            }

            return values;
        }
    }
}