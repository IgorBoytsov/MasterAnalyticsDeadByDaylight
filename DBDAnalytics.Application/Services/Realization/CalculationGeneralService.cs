using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Enums;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Domain.Constants;
using System.Globalization;
using System.Linq;

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

        public List<LabeledValue> DayOrWeekActivity(List<DetailsMatchDTO> matches)
        {
            List<LabeledValue> dayOrWeekActivity = [];

            var dayOfWeeks = _calculationTimeService.GetDayOfWeeks();
            CultureInfo russianCulture = new("ru-RU");
            TextInfo russianTextInfo = russianCulture.TextInfo;

            foreach (var week in dayOfWeeks)
            {
                int count = matches.Count(x => x.Date.HasValue && CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(x.Date.Value) == week);

                string lowerCaseDayName = russianCulture.DateTimeFormat.GetDayName(week);

                string titleCaseDayName = russianTextInfo.ToTitleCase(lowerCaseDayName);

                dayOrWeekActivity.Add(new LabeledValue
                {
                    Name = titleCaseDayName,
                    Value = count,
                });
            }

            return dayOrWeekActivity;
        }

        public List<LabeledValue> MonthlyActivity(List<DetailsMatchDTO> matches)
        {
            List<LabeledValue> monthlyActivity = [];

            var months = _calculationTimeService.GetMonths();

            foreach (var month in months)
            {
                int count = matches.Count(x => x.Date.HasValue && x.Date.Value.Month == month.Value);

                monthlyActivity.Add(new LabeledValue
                {
                    Name = month.Key,
                    Value = count,
                });
            }

            return monthlyActivity;
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

                return _creatingApplicationModelsService.CreatedLoadoutPopularity(itemName, itemImage, itemWinMatchesCount, itemMatchesCount, pickRate, winRateContributionToAll, winRateWhenPicked);
            })
            .OrderByDescending(x => x.PickRate)
            .ToList();

            return result;
        }

        public List<DoubleAddonsPopularity<TAddon>> DoubleAddonPopularity<TAddon>(
            List<DetailsMatchDTO> matches, 
            List<TAddon> addons, 
            Func<int, TAddon> addonSelector, 
            Func<DetailsMatchDTO,(int? FirstAddonID, int? SecondAddonID)> idAddonSelector,
            Func<(int FirstAddonID, int SecondAddonID), Func<DetailsMatchDTO, bool>> createCountPredicate,
            Func<DetailsMatchDTO, bool> rulesForFilteringWinRatePredicate) where TAddon : class
        {
            var rawAddonPairs = matches.Select(x => idAddonSelector(x));

            var normalizedPairs = rawAddonPairs.Where(pair => pair.FirstAddonID.HasValue && pair.SecondAddonID.HasValue)
                .Select(pair =>
                {
                    // Ставим '!' перед .Value, означает, что мы сообщаем компилятору, что мы уверенны в том, что тут не будет null. Т.к была использована фильтрация с помощью Where.
                    int id1 = pair.FirstAddonID!.Value;
                    int id2 = pair.SecondAddonID!.Value;

                    //return id1 <= id2 ? (Addon1: id1, Addon2: id2) : (Addon1: id2, Addon2: id1);
                    return (FirstAddonID: Math.Min(id1, id2), SecondAddonID: Math.Max(id1, id2));
                });

            var pairCounts = normalizedPairs.GroupBy(pair => pair).ToDictionary(group => group.Key,group => group.Count());

            List<DoubleAddonsPopularity<TAddon>> doubleAddons = pairCounts.Select(pair =>
            {
                Func<DetailsMatchDTO, bool> countPredicate = createCountPredicate(pair.Key);

                var firstAddon = addonSelector(pair.Key.FirstAddonID);
                var secondAddon = addonSelector(pair.Key.SecondAddonID);
                var count = pair.Value;

                var totalMatchesCount = matches.Count;
                var matchesWithThisPairCount = matches.Count(match => countPredicate(match));
                var wonMatchesWithThisPairCount = matches.Where(x => rulesForFilteringWinRatePredicate(x)).Count(x => countPredicate(x));

                var pickRate = Percentage(count, totalMatchesCount);
                var winRate = Percentage(wonMatchesWithThisPairCount, matchesWithThisPairCount);

                return new DoubleAddonsPopularity<TAddon>
                {
                    FirstAddon = firstAddon,
                    SecondAddon = secondAddon,
                    Count = count,
                    PickRate = pickRate,
                    WinRate = winRate
                };

            }).ToList();

            return doubleAddons;
        }

        public List<QuadruplePerksPopularity<TPerk>> QuadruplePerkPopularity<TPerk>(
            List<DetailsMatchDTO> matches,
            List<TPerk> perks,
            Func<int, TPerk> perkSelector,
            Func<DetailsMatchDTO, (int? FirstPerkID, int? SecondPerkID, int? ThirdPerkID, int? FourthPerkID)> idPerkSelector,
            Func<(int FirstPerkID, int SecondPerkID, int ThirdPerkID, int FourthPerkID), Func<DetailsMatchDTO, bool>> createCountPredicate,
            Func<DetailsMatchDTO, bool> rulesForFilteringWinRatePredicate) where TPerk : class
        {
            var rawPerkCombos = matches.Select(x => idPerkSelector(x));

            var normalizedCombos = rawPerkCombos.Where(combo => combo.FirstPerkID.HasValue && combo.SecondPerkID.HasValue && combo.ThirdPerkID.HasValue && combo.FourthPerkID.HasValue)
                .Select(combo =>
                {
                    int[] ids = 
                    [
                        combo.FirstPerkID!.Value,
                        combo.SecondPerkID!.Value,
                        combo.ThirdPerkID!.Value,
                        combo.FourthPerkID!.Value
                    ];

                    Array.Sort(ids);

                    return (FirstPerkID: ids[0], SecondPerkID: ids[1], ThirdPerkID: ids[2], FourthPerkID: ids[3]);
                });

            var comboCounts = normalizedCombos.GroupBy(combo => combo).ToDictionary(group => group.Key, group => group.Count());

            List<QuadruplePerksPopularity<TPerk>> quadruplePerks = comboCounts.Select(kvp =>
            {
                var comboKey = kvp.Key;
                var count = kvp.Value;

                Func<DetailsMatchDTO, bool> countPredicate = createCountPredicate(comboKey);

                var firstPerk = perkSelector(comboKey.FirstPerkID);
                var secondPerk = perkSelector(comboKey.SecondPerkID);
                var thirdPerk = perkSelector(comboKey.ThirdPerkID);
                var fourthPerk = perkSelector(comboKey.FourthPerkID);

                var totalMatchesCount = matches.Count;
                var matchesWithThisComboCount = matches.Count(match => countPredicate(match)); 
                var wonMatchesWithThisComboCount = matches.Where(x => rulesForFilteringWinRatePredicate(x)).Count(x => countPredicate(x)); 
                var pickRate = Percentage(count, totalMatchesCount); 
                var winRate = Percentage(wonMatchesWithThisComboCount, matchesWithThisComboCount);

                return new QuadruplePerksPopularity<TPerk>
                {
                    FirstPerk = firstPerk,
                    SecondPerk = secondPerk,
                    ThirdPerk = thirdPerk,
                    FourthPerk = fourthPerk,
                    Count = count,
                    PickRate = pickRate,
                    WinRate = winRate
                };

            }).ToList();

            return quadruplePerks;
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