using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Enums;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Domain.Constants;
using System.Diagnostics;
using System.Globalization;

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

        /// <summary>
        /// Рассчитывает популярность отдельных элементов (например, перков, аддонов, предметов) на основе списка данных.
        /// </summary>
        /// <typeparam name="TDataSource">Тип элемента в исходном списке данных (например, DetailsMatchDTO, DetailsMatchSurvivorDTO).</typeparam>
        /// <typeparam name="TCollectionItem">Тип анализируемого элемента (например, KillerPerk, SurvivorAddon).</typeparam>
        /// <param name="dataSourceItems">Список исходных данных для расчетов.</param>
        /// <param name="itemsToAnalyze">Список элементов, популярность которых нужно рассчитать.</param>
        /// <param name="itemUsagePredicate">Функция, определяющая, используется ли данный элемент TCollectionItem в данном элементе TDataSource.</param>
        /// <param name="nameSelector">Селектор для получения имени элемента TCollectionItem.</param>
        /// <param name="imageSelector">Селектор для получения изображения элемента TCollectionItem.</param>
        /// <param name="winPredicate">Предикат для определения "победы" для элемента TDataSource (используется для расчета WinRate).</param>
        /// <returns>Список объектов LoadoutPopularity с рассчитанной статистикой для каждого элемента TCollectionItem.</returns>
        public List<LoadoutPopularity> CalculatePopularity<TDataSource, TCollectionItem>(
            List<TDataSource> dataSourceItems,
            List<TCollectionItem> itemsToAnalyze,
            Func<TDataSource, TCollectionItem, bool> itemUsagePredicate,
            Func<TCollectionItem, string> nameSelector,
            Func<TCollectionItem, byte[]?> imageSelector,
            Func<TDataSource, bool> winPredicate)
        {
            if (dataSourceItems == null || itemsToAnalyze == null)
                return [];

            var totalDataSourceItemsCount = dataSourceItems.Count;
            if (totalDataSourceItemsCount == 0)
                return [];

            var result = itemsToAnalyze.Select(item =>
            {
                var dataSourceItemsWithThisItem = dataSourceItems
                    .Where(dataItem => itemUsagePredicate(dataItem, item))
                    .ToList();

                var winCountWithThisItem = dataSourceItemsWithThisItem.Count(dataItem => winPredicate(dataItem));
                var pickCountForItem = dataSourceItemsWithThisItem.Count;
                double pickRate = Percentage(pickCountForItem, totalDataSourceItemsCount);
                double winRateContributionToAll = Percentage(winCountWithThisItem, totalDataSourceItemsCount);
                double winRateWhenPicked = pickCountForItem > 0 ? Percentage(winCountWithThisItem, pickCountForItem) : 0.0;

                string itemName = nameSelector(item);
                byte[]? itemImage = imageSelector(item);

                return _creatingApplicationModelsService.CreatedLoadoutPopularity(itemName, itemImage, winCountWithThisItem, pickCountForItem, pickRate, winRateContributionToAll, winRateWhenPicked);
            })
            .OrderByDescending(x => x.PickRate)
            .ToList();

            return result;
        }

        /// <summary>
        /// Рассчитывает популярность комбинаций из двух "аддонов" (или других парных элементов).
        /// </summary>
        /// <typeparam name="TDataSource">Тип элемента в исходном списке данных (например, DetailsMatchDTO).</typeparam>
        /// <typeparam name="TItem">Тип объекта (или парного элемента).</typeparam>
        /// <param name="dataSourceItems">Список исходных данных для анализа.</param>
        /// <param name="allItems">Полный список всех возможных предметов данного типа. (К примеру : Аддоны аптечки, улучшения киллера)</param>
        /// <param name="itemSelectorById">Селектор для получения объекта предмета по его ID.</param>
        /// <param name="idPairSelectorFromDataSource">Функция для извлечения кортежа состоящего из двух ID предмета из элемента TDataSource.</param>
        /// <param name="createCountPredicate">Предикат, принимающий отсортированный кортеж ID предметов и возвращает предикат для подсчета совпадений в TDataSource.</param>
        /// <param name="winPredicate">Предикат для определения "победы" для элемента TDataSource (используется для расчета WinRate).</param>
        /// <returns>Список объектов DoubleAddonsPopularity с рассчитанной парой и их краткой статистикой.</returns>
        public List<DoubleAddonsPopularity<TItem>> DoubleItemPopularity<TDataSource, TItem>(
            List<TDataSource> dataSourceItems,
            List<TItem> allItems,
            Func<int, TItem?> itemSelectorById,
            Func<TDataSource, (int? FirstItemID, int? SecondItemID)> idPairSelectorFromDataSource,
            Func<(int FirstItemID, int SecondItemID), Func<TDataSource, bool>> createCountPredicate,
            Func<TDataSource, bool> winPredicate) where TItem : class
        {
            if (dataSourceItems == null || dataSourceItems.Count == 0 || allItems == null)
                return [];

            var rawItemPairs = dataSourceItems.Select(item => idPairSelectorFromDataSource(item));

            var normalizedPairs = rawItemPairs
                .Where(pair => pair.FirstItemID.HasValue && pair.SecondItemID.HasValue)
                .Select(pair =>
                {
                    int id1 = pair.FirstItemID!.Value;
                    int id2 = pair.SecondItemID!.Value;

                    return (FirstItemID: Math.Min(id1, id2), SecondItemID: Math.Max(id1, id2));
                });

            var pairCounts = normalizedPairs
                .GroupBy(pair => pair)
                .ToDictionary(group => group.Key, group => group.Count());

            List<DoubleAddonsPopularity<TItem>> doubleItemStats = pairCounts.Select(kvp =>
            {
                var pairKey = kvp.Key;
                var countForThisPair = kvp.Value;

                Func<TDataSource, bool> countPredicate = createCountPredicate(pairKey);

                var firstItem = itemSelectorById(pairKey.FirstItemID);
                var secondItem = itemSelectorById(pairKey.SecondItemID);

                if (firstItem == null || secondItem == null)
                {
                    Console.WriteLine($"Предупреждение: Не удалось найти все улучшение для комбинированных идентификаторов {pairKey.FirstItemID}, {pairKey.SecondItemID}. Пропущенная пара.");
                    return null;
                }

                var totalDataSourceItemsCount = dataSourceItems.Count;
                var matchesWithThisPairCount = dataSourceItems.Count(dataItem => countPredicate(dataItem));
                var wonMatchesWithThisPairCount = dataSourceItems.Where(dataItem => winPredicate(dataItem)).Count(dataItem => countPredicate(dataItem));
                var pickRate = Percentage(countForThisPair, totalDataSourceItemsCount);
                var winRate = Percentage(wonMatchesWithThisPairCount, matchesWithThisPairCount);

                return new DoubleAddonsPopularity<TItem>
                {
                    FirstAddon = firstItem, 
                    SecondAddon = secondItem,
                    Count = countForThisPair,
                    PickRate = pickRate,
                    WinRate = winRate
                };
            })
            .Where(result => result != null)
            .Select(result => result!)  
            .OrderByDescending(x => x.Count)
            .ToList();

            return doubleItemStats;
        }

        /// <summary>
        /// Рассчитывает популярность комбинаций из четырех перков.
        /// </summary>
        /// <typeparam name="TDataSource">Исходный тип данных списка (например, DetailsMatchDTO или DetailsMatchSurvivorDTO).</typeparam>
        /// <typeparam name="TItem">Тип объекта перка.</typeparam>
        /// <param name="dataSourceItems">Список исходных данных для расчета.</param>
        /// <param name="allItems">Полный список всех возможных предметов данного типа. (К примеру : Перки киллера, выжившего)</param>
        /// <param name="itemSelectorById">Селектор для получения объекта предмета по его ID.</param>
        /// <param name="idItemSelectorFromDataSource">Функция для извлечения кортежа состоящего из четырех ID предметов из TDataSource.</param>
        /// <param name="createCountPredicate">Предикат, принимающий отсортированный кортеж ID предметов и проверяет подсчет совпадений в TDataSource.</param>
        /// <param name="winPredicate">Предикат для определения "победы" для элемента TDataSource.</param>
        /// <returns>Список объектов QuadruplePerksPopularity с рассчитанными комбинациями билдов и их краткой статистиков.</returns>
        public List<QuadruplePerksPopularity<TItem>> QuadrupleItemPopularity<TDataSource, TItem>(
            List<TDataSource> dataSourceItems,
            List<TItem> allItems,
            Func<int, TItem> itemSelectorById,
            Func<TDataSource, (int? FirstItemID, int? SecondItemID, int? ThirdItemID, int? FourthItemID)> idItemSelectorFromDataSource,
            Func<(int FirstItemID, int SecondItemID, int ThirdItemID, int FourthItemID), Func<TDataSource, bool>> createCountPredicate,
            Func<TDataSource, bool> winPredicate) where TItem : class
        {
            if (dataSourceItems == null || dataSourceItems.Count == 0)
                return [];

            var rawPerkCombos = dataSourceItems.Select(item => idItemSelectorFromDataSource(item));

            var normalizedCombos = rawPerkCombos
                .Where(combo => combo.FirstItemID.HasValue && combo.SecondItemID.HasValue && combo.ThirdItemID.HasValue && combo.FourthItemID.HasValue)
                .Select(combo =>
                {
                    int[] ids =
                    [
                        combo.FirstItemID!.Value,
                        combo.SecondItemID!.Value,
                        combo.ThirdItemID!.Value,
                        combo.FourthItemID!.Value
                    ];

                    Array.Sort(ids);

                    return (FirstItemID: ids[0], SecondItemID: ids[1], ThirdItemID: ids[2], FourthItemID: ids[3]);
                });

            var comboCounts = normalizedCombos
                .GroupBy(combo => combo)
                .ToDictionary(group => group.Key, group => group.Count());

            List<QuadruplePerksPopularity<TItem>> quadrupleItems = comboCounts.Select(kvp =>
            {
                var comboKey = kvp.Key;
                var countForThisCombo = kvp.Value;

                Func<TDataSource, bool> countPredicate = createCountPredicate(comboKey);

                var firstItem = itemSelectorById(comboKey.FirstItemID);
                var secondItem = itemSelectorById(comboKey.SecondItemID);
                var thirdItem = itemSelectorById(comboKey.ThirdItemID);
                var fourthItem = itemSelectorById(comboKey.FourthItemID);

                if (firstItem == null || secondItem == null || thirdItem == null || fourthItem == null)
                {
                    Debug.WriteLine($"Предупреждение: Не удалось найти все перки для комбинированных идентификаторов {comboKey.FirstItemID}, {comboKey.SecondItemID}, {comboKey.ThirdItemID}, {comboKey.FourthItemID}. Пропущенное комбо.");
                    return null;
                }

                var totalDataSourceItemsCount = dataSourceItems.Count;
                var itemsWithThisComboCount = dataSourceItems.Count(item => countPredicate(item));
                var wonItemsWithThisComboCount = dataSourceItems.Where(item => winPredicate(item)).Count(item => countPredicate(item));
                var pickRate = Percentage(countForThisCombo, totalDataSourceItemsCount);
                var winRate = Percentage(wonItemsWithThisComboCount, itemsWithThisComboCount);

                return new QuadruplePerksPopularity<TItem>
                {
                    FirstPerk = firstItem,
                    SecondPerk = secondItem,
                    ThirdPerk = thirdItem,
                    FourthPerk = fourthItem,
                    Count = countForThisCombo,
                    PickRate = pickRate,
                    WinRate = winRate
                };

            })
           .Where(result => result != null)
           .Select(result => result!)
           .ToList();

            return quadrupleItems;
        }

        /*--Проверки--------------------------------------------------------------------------------------*/

        private static bool IsInvalidForAverageAndPercentage(int value, int size) => value <= 0 || size <= 0;
        
    }
}