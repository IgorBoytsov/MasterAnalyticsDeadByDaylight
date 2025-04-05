using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Enums;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Domain.Constants;

namespace DBDAnalytics.Application.Services.Realization
{
    public class CalculationGeneralService(ICalculationTimeService calculationTimeService,
                                           ICreatingApplicationModelsService creatingApplicationModelsService) : ICalculationGeneralService
    {
        private readonly ICreatingApplicationModelsService _creatingApplicationModelsService = creatingApplicationModelsService;
        private readonly ICalculationTimeService _calculationTimeService = calculationTimeService;

        /*--Основные методы-------------------------------------------------------------------------------*/

        public double Average(int value, int size, int digits = 2) => IsInvalidForAverageAndPercentage(value, size) ? 0 : Math.Round((double)value / size, digits);

        public double Percentage(int value, int size, int digits = 2) => IsInvalidForAverageAndPercentage(value, size) ? 0 : Math.Round(((double)value / size) * 100, digits);
        
        /*--Детализация-----------------------------------------------------------------------------------*/

        public List<LabeledValue> DetailingRecentGenerators(List<DetailsMatchDTO> matches) => _creatingApplicationModelsService.GenerateLabeledValues(matches, GameRulesConstants.MaxRecentGenerators, i => matches.Count(x => x.RecentGenerator == i), $"Игр с {{0}} генераторами: ");

        /*--Расчеты матча---------------------------------------------------------------------------------*/

        public int Series(List<DetailsMatchDTO> matches, Func<DetailsMatchDTO, bool> condition)
        {
            if (matches == null || matches.Count == 0)
                return 0;

            var sortedMatches = matches.OrderBy(m => m.Date).ToList();

            int currentStreak = 0;
            int longestStreak = 0;

            foreach (var match in sortedMatches)
            {
                if (condition(match))
                {
                    currentStreak++;
                    longestStreak = Math.Max(longestStreak, currentStreak);
                }
                else
                {
                    currentStreak = 0;
                }
            }

            return longestStreak;
        }

        /*--Расчеты за периоды времени--------------------------------------------------------------------*/

        public List<LabeledValue> CountMatchesByTimePeriod(List<DetailsMatchDTO> matches, TimePeriod period)
        {
            if (matches == null || matches.Count == 0)
                return [];

            List<LabeledValue> result = [];

            Action action = period switch
            {
                TimePeriod.Day => () =>
                {
                    if (matches.Count == 0)
                    {
                        result = [];
                        return;
                    }

                    DateTime startDate = matches.Min(m => m.Date.GetValueOrDefault().Date);
                    DateTime endDate = matches.Max(m => m.Date.GetValueOrDefault().Date);

                    List<DateTime> allDates = [];
                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        allDates.Add(date);
                    }

                    var groupedMatches = matches.GroupBy(m => m.Date.GetValueOrDefault().Date);

                    result = allDates.GroupJoin(
                            groupedMatches,
                            date => date,
                            group => group.Key,
                            (date, groups) => new 
                            {
                                Date = date, 
                                Groups = groups 
                            })
                        .SelectMany(
                            joined => joined.Groups.DefaultIfEmpty(),
                            (joined, group) => new LabeledValue
                            {
                                Name = joined.Date.ToString("yyyy-MM-dd"),
                                ConverterValue = joined.Date.ToOADate(),
                                Value = group == null ? Average(0, 0, 0) : group.Count()
                            })
                        .OrderBy(lv => lv.ConverterValue)
                        .ToList();
                }
                ,
                TimePeriod.Week => () =>
                {
                    if (matches.Count == 0)
                    {
                        result = [];
                        return;
                    }

                    DateTime startDate = matches.Min(m => m.Date.GetValueOrDefault().Date);
                    DateTime endDate = matches.Max(m => m.Date.GetValueOrDefault().Date);

                    var allWeeks = new List<(int Year, int Month, int WeekOfMonth)>();
                    DateTime currentDate = startDate;

                    while (currentDate <= endDate)
                    {
                        int year = currentDate.Year;
                        int month = currentDate.Month;
                        int weekOfMonth = _calculationTimeService.GetWeekOfMonth(currentDate);
                        allWeeks.Add((year, month, weekOfMonth));
                        currentDate = currentDate.AddDays(7);
                    }
                    allWeeks = allWeeks.Distinct().ToList();

                    var groupedMatches = matches.GroupBy(m => new
                    {
                        Year = m.Date.GetValueOrDefault().Year,
                        Month = m.Date.GetValueOrDefault().Month,
                        WeekOfMonth = _calculationTimeService.GetWeekOfMonth(m.Date.GetValueOrDefault())
                    });

                    result = allWeeks.GroupJoin(
                        groupedMatches,
                        week => (week.Year, week.Month, week.WeekOfMonth),
                        group => (group.Key.Year, group.Key.Month, group.Key.WeekOfMonth),
                        (week, groups) => new
                        { 
                            Week = week, 
                            Groups = groups 
                        })
                    .SelectMany(
                        joined => joined.Groups.DefaultIfEmpty(),
                        (joined, group) => new LabeledValue
                        {
                            Name = $"{joined.Week.Year}:{joined.Week.Month:D2}: Неделя№ {joined.Week.WeekOfMonth}",
                            ConverterValue = joined.Week.Year * 10000 + joined.Week.Month * 100 + joined.Week.WeekOfMonth,
                            Value = group == null ? 0 : group.Count()
                        })
                    .OrderBy(lv => lv.ConverterValue)
                    .ToList();
                }
                ,
                TimePeriod.Month => () =>
                {
                    if (matches.Count == 0)
                    {
                        result = [];
                        return;
                    }

                    DateTime startDate = matches.Min(m => m.Date.GetValueOrDefault().Date);
                    DateTime endDate = matches.Max(m => m.Date.GetValueOrDefault().Date);

                    var allMonths = new List<DateTime>();
                    DateTime currentMonth = new(startDate.Year, startDate.Month, 1);
                    DateTime endMonth = new(endDate.Year, endDate.Month, 1);

                    while (currentMonth <= endMonth)
                    {
                        allMonths.Add(currentMonth);
                        currentMonth = currentMonth.AddMonths(1);
                    }

                    var groupedMatches = matches.GroupBy(m => new DateTime(m.Date.GetValueOrDefault().Year, m.Date.GetValueOrDefault().Month, 1));

                    result = allMonths.GroupJoin(
                        groupedMatches,
                        month => month,
                        group => group.Key,
                        (month, groups) => new 
                        {
                            Month = month, 
                            Groups = groups 
                        })
                    .SelectMany(
                        joined => joined.Groups.DefaultIfEmpty(),
                        (joined, group) => new LabeledValue
                        {
                            Name = joined.Month.ToString("yyyy-MM"),
                            ConverterValue = joined.Month.ToOADate(),
                            Value = group == null ? 0 : group.Count()
                        })
                    .OrderBy(lv => lv.ConverterValue)
                    .ToList();
                }
                ,
                TimePeriod.Year => () =>
                {
                    if (matches.Count == 0)
                    {
                        result = [];
                        return;
                    }

                    int startYear = matches.Min(m => m.Date.GetValueOrDefault().Year);
                    int endYear = matches.Max(m => m.Date.GetValueOrDefault().Year);

                    var allYears = Enumerable.Range(startYear, endYear - startYear + 1).ToList();

                    var groupedMatches = matches.GroupBy(m => m.Date.GetValueOrDefault().Year);

                    result = allYears.GroupJoin(
                        groupedMatches,
                        year => year,
                        group => group.Key,
                        (year, groups) => new 
                        { 
                            Year = year, 
                            Groups = groups 
                        })
                    .SelectMany(
                        joined => joined.Groups.DefaultIfEmpty(),
                        (joined, group) => new LabeledValue
                        {
                            Name = joined.Year.ToString(),
                            ConverterValue = joined.Year,
                            Value = group == null ? 0 : group.Count()
                        })
                    .OrderBy(lv => lv.ConverterValue)
                    .ToList();
                }
                ,
                _ => () => throw new Exception("Такого типа времени не существует.")
            };

            action?.Invoke();

            return result;
        }

        public List<LabeledValue> AvgScoreByTimePeriod(List<DetailsMatchDTO> matches, TimePeriod period, Func<DetailsMatchDTO, int> selector)
        {
            if (matches == null || matches.Count == 0)
                return [];

            List<LabeledValue> result = [];

            Action action = period switch
            {
                TimePeriod.Day => () =>
                {
                    if (matches.Count == 0)
                    {
                        result = [];
                        return;
                    }

                    DateTime startDate = matches.Min(m => m.Date.GetValueOrDefault().Date);
                    DateTime endDate = matches.Max(m => m.Date.GetValueOrDefault().Date);

                    List<DateTime> allDates = [];
                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        allDates.Add(date);
                    }

                    var groupedMatches = matches.GroupBy(m => m.Date.GetValueOrDefault().Date);

                    result = allDates.GroupJoin(
                            groupedMatches,
                            date => date,
                            group => group.Key,
                            (date, groups) => new
                            {
                                Date = date,
                                Groups = groups
                            })
                        .SelectMany(
                            joined => joined.Groups.DefaultIfEmpty(),
                            (joined, group) => new LabeledValue
                            {
                                Name = joined.Date.ToString("yyyy-MM-dd"),
                                ConverterValue = joined.Date.ToOADate(),
                                Value = group == null ? Average(0, 0, 0) : Average(group.Sum(s => selector(s)), group.Count(), 0)
                            })
                        .OrderBy(lv => lv.ConverterValue)
                        .ToList();
                }
                ,
                TimePeriod.Week => () =>
                {
                    if (matches.Count == 0)
                    {
                        result = [];
                        return;
                    }

                    DateTime startDate = matches.Min(m => m.Date.GetValueOrDefault().Date);
                    DateTime endDate = matches.Max(m => m.Date.GetValueOrDefault().Date);

                    var allWeeks = new List<(int Year, int Month, int WeekOfMonth)>();

                    DateTime currentDate = startDate;
                    while (currentDate <= endDate)
                    {
                        int year = currentDate.Year;
                        int month = currentDate.Month;
                        int weekOfMonth = _calculationTimeService.GetWeekOfMonth(currentDate);
                        allWeeks.Add((year, month, weekOfMonth));
                        currentDate = currentDate.AddDays(7);
                    }

                    allWeeks = allWeeks.Distinct().ToList();

                    var groupedMatches = matches.GroupBy(m => new
                    {
                        Year = m.Date.GetValueOrDefault().Year,
                        Month = m.Date.GetValueOrDefault().Month,
                        WeekOfMonth = _calculationTimeService.GetWeekOfMonth(m.Date.GetValueOrDefault())
                    });

                    result = allWeeks.GroupJoin(
                        groupedMatches,
                        week => (week.Year, week.Month, week.WeekOfMonth),
                        group => (group.Key.Year, group.Key.Month, group.Key.WeekOfMonth),
                        (week, groups) => new
                        {
                            Week = week,
                            Groups = groups
                        })
                    .SelectMany(
                        joined => joined.Groups.DefaultIfEmpty(),
                        (joined, group) => new LabeledValue
                        {
                            Name = $"{joined.Week.Year}:{joined.Week.Month:D2}: Неделя№ {joined.Week.WeekOfMonth}",
                            ConverterValue = joined.Week.Year * 10000 + joined.Week.Month * 100 + joined.Week.WeekOfMonth,
                            Value = group == null ? Average(0, 0, 0) : Average(group.Sum(s => selector(s)), group.Count(), 0)
                        })
                    .OrderBy(lv => lv.ConverterValue)
                    .ToList();
                }
                ,
                TimePeriod.Month => () =>
                {
                    if (matches.Count == 0)
                    {
                        result = [];
                        return;
                    }

                    DateTime startDate = matches.Min(m => m.Date.GetValueOrDefault().Date);
                    DateTime endDate = matches.Max(m => m.Date.GetValueOrDefault().Date);

                    var allMonths = new List<DateTime>();

                    DateTime currentMonth = new(startDate.Year, startDate.Month, 1);
                    DateTime endMonth = new(endDate.Year, endDate.Month, 1);

                    while (currentMonth <= endMonth)
                    {
                        allMonths.Add(currentMonth);
                        currentMonth = currentMonth.AddMonths(1);
                    }

                    var groupedMatches = matches.GroupBy(m => new DateTime(m.Date.GetValueOrDefault().Year, m.Date.GetValueOrDefault().Month, 1));

                    result = allMonths.GroupJoin(
                        groupedMatches,
                        month => month,
                        group => group.Key,
                        (month, groups) => new
                        {
                            Month = month,
                            Groups = groups
                        })
                    .SelectMany(
                        joined => joined.Groups.DefaultIfEmpty(),
                        (joined, group) => new LabeledValue
                        {
                            Name = joined.Month.ToString("yyyy-MM"),
                            ConverterValue = joined.Month.ToOADate(),
                            Value = group == null ? Average(0, 0, 0) : Average(group.Sum(s => selector(s)), group.Count(), 0)
                        })
                    .OrderBy(lv => lv.ConverterValue)
                    .ToList();
                }
                ,
                TimePeriod.Year => () =>
                {
                    if (matches.Count == 0)
                    {
                        result = [];
                        return;
                    }

                    int startYear = matches.Min(m => m.Date.GetValueOrDefault().Year);
                    int endYear = matches.Max(m => m.Date.GetValueOrDefault().Year);

                    var allYears = Enumerable.Range(startYear, endYear - startYear + 1).ToList();

                    var groupedMatches = matches.GroupBy(m => m.Date.GetValueOrDefault().Year);

                    result = allYears.GroupJoin(
                        groupedMatches,
                        year => year,
                        group => group.Key,
                        (year, groups) => new
                        {
                            Year = year,
                            Groups = groups
                        })
                    .SelectMany(
                        joined => joined.Groups.DefaultIfEmpty(),
                        (joined, group) => new LabeledValue
                        {
                            Name = joined.Year.ToString(),
                            ConverterValue = joined.Year,
                            Value = group == null ? Average(0, 0, 0) : Average(group.Sum(s => selector(s)), group.Count(), 0)
                        })
                    .OrderBy(lv => lv.ConverterValue)
                    .ToList();
                }
                ,
                _ => () => throw new Exception("Такого типа времени не существует.")
            };

            action?.Invoke();

            return result;
        }

        public List<LabeledValue> HourlyActivity(List<DetailsMatchDTO> matches)
        {
            List<LabeledValue> hourlyActivity = [];

            for (int hour = 0; hour < 24; hour++)
            {
                int matchCount = matches
                    .Where(x => x.Date.HasValue && x.Date.Value.Hour == hour)
                    .Count();

                hourlyActivity.Add(new LabeledValue
                {
                    Name = $"{hour:D2}:00 - {(hour + 1) % 24:D2}:00",
                    Value = matchCount
                });
            }

            return hourlyActivity;
        }

        /*--Расчеты популярности--------------------------------------------------------------------------*/

        public List<LoadoutPopularity> CalculatePopularity<TCollectionItem>(
            List<DetailsMatchDTO> matches,
            List<TCollectionItem> items,
            Func<DetailsMatchDTO, TCollectionItem, bool> matchPredicate,
            Func<TCollectionItem, string> nameSelector,
            Func<TCollectionItem, byte[]?> imageSelector,
            Func<DetailsMatchDTO, bool> countPredicate)
        {
            if (matches == null || items == null)
                return [];

            var totalMatchesCount = matches.Count;
            if (totalMatchesCount == 0)
                return [];

            var result = items.Select(item =>
            {
                var itemMatches = matches
                    .Where(match => matchPredicate(match, item))
                    .ToList();

                var itemWinMatchesCount = itemMatches.Count(match => countPredicate(match));
                var itemMatchesCount = itemMatches.Count;

                double pickRate = Percentage(itemMatchesCount, totalMatchesCount);
                double winRateContributionToAll = Percentage(itemWinMatchesCount, totalMatchesCount);
                double winRateWhenPicked = itemMatchesCount > 0 ? Percentage(itemWinMatchesCount, itemMatchesCount) : 0.0;

                string itemName = nameSelector(item);
                byte[]? itemImage = imageSelector(item);

                return _creatingApplicationModelsService.CreatedLoadoutPopularity(itemName, itemImage, totalMatchesCount, itemMatchesCount, pickRate, winRateContributionToAll, winRateWhenPicked);
            })
            .OrderByDescending(x => x.PickRate)
            .ToList();

            return result;
        }

        /*--Проверки--------------------------------------------------------------------------------------*/

        private static bool IsInvalidForAverageAndPercentage(int value, int size) => value <= 0 || size <= 0;
        
    }
}

//public List<LoadoutPopularity> PopularityKillerAddon(List<DetailsMatchDTO> matches, List<KillerAddonDTO> addons)
//{
//    var result = addons.Select(addon =>
//    {
//        var addonMatches = matches
//            .Where(x => x.KillerDTO.FirstAddonID == addon.IdKillerAddon || x.KillerDTO.SecondAddonID == addon.IdKillerAddon)
//            .ToList();

//        var addonWinMatchesCount = addonMatches.Count(x => x.CountKill > 2);
//        var totalMatchesCount = matches.Count;
//        var addonMatchesCount = addonMatches.Count;

//        double pickRate = _calculatorGeneralService.Percentage(addonMatchesCount, totalMatchesCount);
//        double winRateAllMatch = _calculatorGeneralService.Percentage(addonWinMatchesCount, totalMatchesCount);
//        double winRateWithItemLoadoutMatch = _calculatorGeneralService.Percentage(addonWinMatchesCount, addonMatchesCount);

//        return CreatedLoadoutPopularity(addon.AddonName, addon.AddonImage, totalMatchesCount, addonMatchesCount, pickRate, winRateAllMatch, winRateWithItemLoadoutMatch);
//    })
//    .OrderByDescending(x => x.PickRate)
//    .ToList();

//    return result;
//}