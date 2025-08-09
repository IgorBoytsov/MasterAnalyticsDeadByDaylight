using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Services.Abstraction;

namespace DBDAnalytics.Application.Services.Realization
{
    public class CreatingApplicationModelsService(Lazy<ICalculationGeneralService> calculatorGeneralService) : ICreatingApplicationModelsService
    {
        private readonly Lazy<ICalculationGeneralService> _calculationGeneralService = calculatorGeneralService;

        public List<LabeledValue> GenerateLabeledValues(
            List<DetailsMatchDTO> matches,
            int maxValue,
            Func<int, int> countMatchesWithValue,
            string displayNameFormat)
        {
            return Enumerable.Range(0, maxValue + 1)
                .Select(i =>
                {
                    int count = countMatchesWithValue(i);
                    return CreateLabeledValue(i, count, matches.Count, displayNameFormat);
                })
                .ToList();
        }

        private LabeledValue CreateLabeledValue(int i, int count, int totalMatches, string displayNameFormat)
        {
            return new LabeledValue
            {
                Name = string.Format(displayNameFormat, i),
                ConverterValue = i,
                Value = _calculationGeneralService.Value.Percentage(count, totalMatches)
            };
        }

        public LoadoutPopularity CreatedLoadoutPopularity(
            string name,
            byte[]? image,
            int countWinMatchWithItemLoadout,
            int countMatchWithItemLoadout,
            double pickRate,
            double winRateAllMatch,
            double winRateWithItemLoadoutMatch)
        {
            return new LoadoutPopularity
            {
                Name = name,
                Image = image,
                CountWinMatchWithItemLoadout = countWinMatchWithItemLoadout,
                CountMatchWithItemLoadout = countMatchWithItemLoadout,
                PickRate = pickRate,
                WinRateAllMatch = winRateAllMatch,
                WinRateWithItemLoadoutMatch = winRateWithItemLoadoutMatch
            };
        }
    }
}