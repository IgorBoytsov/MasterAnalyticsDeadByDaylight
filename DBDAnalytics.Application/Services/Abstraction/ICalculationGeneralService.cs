using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Enums;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ICalculationGeneralService
    {
        double Average(int value, int size, int digits = 2);
        double Percentage(int value, int size, int digits = 2);
        public List<LabeledValue> DetailingRecentGenerators(List<DetailsMatchDTO> matches);
        public int Series(List<DetailsMatchDTO> matches, Func<DetailsMatchDTO, bool> condition);
        public List<LabeledValue> CountMatchesByTimePeriod(List<DetailsMatchDTO> matches, TimePeriod period);
        public List<LabeledValue> AvgScoreByTimePeriod(List<DetailsMatchDTO> matches, TimePeriod period, Func<DetailsMatchDTO, int> selector);
        public List<LabeledValue> HourlyActivity(List<DetailsMatchDTO> killerDetailsList);
        public List<LabeledValue> DayOrWeekActivity(List<DetailsMatchDTO> matches);
        public List<LabeledValue> MonthlyActivity(List<DetailsMatchDTO> matches);

        List<LoadoutPopularity> CalculatePopularity<TCollectionItem>(
            List<DetailsMatchDTO> matches,
            List<TCollectionItem> items,
            Func<DetailsMatchDTO, TCollectionItem, bool> matchPredicate,
            Func<TCollectionItem, string> nameSelector,
            Func<TCollectionItem, byte[]?> imageSelector,
            Func<DetailsMatchDTO, bool> countPredicate);

        public List<DoubleAddonsPopularity<TAddon>> DoubleAddonPopularity<TAddon>(
            List<DetailsMatchDTO> matches,
            List<TAddon> addons,
            Func<int, TAddon> addonSelector,
            Func<DetailsMatchDTO, (int? FirstAddonID, int? SecondAddonID)> idAddonSelector,
            Func<DetailsMatchDTO, bool> rulesForWinPredicate) where TAddon : class;
    }
}