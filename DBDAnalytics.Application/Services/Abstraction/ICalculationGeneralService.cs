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

        public List<LoadoutPopularity> CalculatePopularity<TDataSource, TCollectionItem>(
            List<TDataSource> dataSourceItems,
            List<TCollectionItem> itemsToAnalyze,
            Func<TDataSource, TCollectionItem, bool> itemUsagePredicate,
            Func<TCollectionItem, string> nameSelector,
            Func<TCollectionItem, byte[]?> imageSelector,
            Func<TDataSource, bool> winPredicate);

        public List<DoubleAddonsPopularity<TItem>> DoubleItemPopularity<TDataSource, TItem>(
          List<TDataSource> dataSourceItems,
          List<TItem> allItems,
          Func<int, TItem?> itemSelectorById,
          Func<TDataSource, (int? FirstItemID, int? SecondItemID)> idPairSelectorFromDataSource, 
          Func<(int FirstItemID, int SecondItemID), Func<TDataSource, bool>> createCountPredicate,
          Func<TDataSource, bool> winPredicate)
          where TItem : class;

        public List<QuadruplePerksPopularity<TItem>> QuadrupleItemPopularity<TDataSource, TItem>(
            List<TDataSource> dataSourceItems,
            List<TItem> allPerks,
            Func<int, TItem> itemSelectorById,
            Func<TDataSource, (int? FirstItemID, int? SecondItemID, int? ThirdItemID, int? FourthItemID)> idItemSelectorFromDataSource,
            Func<(int FirstItemID, int SecondItemID, int ThirdItemID, int FourthItemID), Func<TDataSource, bool>> createCountPredicate,
            Func<TDataSource, bool> winPredicate) where TItem : class;
    }
}