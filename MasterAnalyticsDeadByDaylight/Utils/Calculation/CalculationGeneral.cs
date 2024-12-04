﻿using MasterAnalyticsDeadByDaylight.MVVM.Model.ChartModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Utils.Enum;

namespace MasterAnalyticsDeadByDaylight.Utils.Calculation
{
    public static class CalculationGeneral
    {
        public static int CountMatch(IEnumerable<GameStatistic> matches)
        {
            return matches.Count();
        }

        #region Статистика по оставшимся генераторам 

        public static async Task<List<RecentGeneratorsTracker>> RecentGeneratorsAsync(List<GameStatistic> matches)
        {
            List<RecentGeneratorsTracker> recentGeneratorsTrackers = [];

            return await Task.Run(() =>
            {
                for (int i = 0; i <= 5; i++)
                {
                    double RecentGeneratorsPercentages = Math.Round((double)matches.Where(x => x.NumberRecentGenerators == i).Count() / matches.Count * 100, 2);
                    RecentGeneratorsPercentages = double.IsNaN(RecentGeneratorsPercentages) ? 0 : RecentGeneratorsPercentages;

                    var recentGenerators = new RecentGeneratorsTracker
                    {
                        CountRecentGeneratorsName = CalculationsCollections.Names[i],
                        CountRecentGeneratorsPercentages = RecentGeneratorsPercentages,
                        CountGame = matches.Where(x => x.NumberRecentGenerators == i).Count(),
                    };
                    recentGeneratorsTrackers.Add(recentGenerators);
                }
                return recentGeneratorsTrackers;
            });
        }

        #endregion

        #region Активность по часам

        public static async Task<List<ActivityByHoursTracker>> CountMatchesPlayedInEachHourAsync(List<GameStatistic> matches)
        {
            List<ActivityByHoursTracker> activityByHoursTracker = [];

            List<DateTime> hours = await CalculationTime.GetHourListAsync();

            return await Task.Run(() =>
            {
                foreach (var item in hours)
                {
                    int countMatchesByHours = matches.Count(x => x.DateTimeMatch.Value.Hour == item.Hour);

                    activityByHoursTracker.Add(new ActivityByHoursTracker()
                    {
                        Hours = item,
                        CountMatch = countMatchesByHours,
                    });
                }
                return activityByHoursTracker;
            });
        }

        #endregion

        #region Количество сыграных матчей за промежуток времени

        public static async Task<List<CountMatchTracker>> CountMatchForPeriodTimeAsync(List<GameStatistic> matches, TypeTime typeTime)
        {
            List<CountMatchTracker> countMatchTracker = [];

            (DateTime First, DateTime Last) = await CalculationTime.FirstAndLastDateMatchAsync(matches);

            Func<DateTime, DateTime> Increment = typeTime switch
            {
                TypeTime.Year => date => date.AddYears(1),
                TypeTime.Month => date => date.AddMonths(1),
                TypeTime.Day => date => date.AddDays(1),
                _ => throw new ArgumentOutOfRangeException(nameof(typeTime), "Неверный тип времени")
            };

            string dateFormat = typeTime switch
            {
                TypeTime.Year => "yyyy",
                TypeTime.Month => "Y",
                TypeTime.Day => "D",
                _ => throw new ArgumentOutOfRangeException(nameof(typeTime), "Неверный тип времени")
            };

            return await Task.Run(async () =>
            {
                for (DateTime date = First; date <= Last; date = Increment(date))
                {
                    var matchByDate = await CalculationTime.MatchForPeriodTimeAsync(matches, typeTime, date);

                    if (matchByDate.Count == 0)
                    {
                        countMatchTracker.Add(new CountMatchTracker
                        {
                            CountMatch = 0,
                            DateTime = date.ToString(dateFormat)
                        });
                    }
                    else
                    {
                        countMatchTracker.Add(new CountMatchTracker
                        {
                            CountMatch = matchByDate.Count,
                            DateTime = date.ToString(dateFormat)
                        });
                    }
                }
                return countMatchTracker;
            });
        }

        #endregion

    }
}