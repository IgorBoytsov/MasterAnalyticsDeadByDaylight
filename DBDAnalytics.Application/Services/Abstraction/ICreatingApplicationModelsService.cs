using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs.DetailsDTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ICreatingApplicationModelsService
    {
        List<LabeledValue> GenerateLabeledValues(List<DetailsMatchDTO> matches, int maxValue, Func<int, int> countMatchesWithValue, string displayNameFormat);
        LoadoutPopularity CreatedLoadoutPopularity(
            string name,
            byte[]? image,
            int countWinMatchWithItemLoadout,
            int countMatchWithItemLoadout,
            double pickRate,
            double winRateAllMatch,
            double winRateWithItemLoadoutMatch);
    }
}