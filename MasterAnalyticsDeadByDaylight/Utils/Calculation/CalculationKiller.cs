using MasterAnalyticsDeadByDaylight.MVVM.Model.ChartModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Utils.Enum;

namespace MasterAnalyticsDeadByDaylight.Utils.Calculation
{
    public static class CalculationKiller
    {
       
        #region Расчеты для отображение списка статистики по киллерам, на странице KillerPage

        public static async Task<double> CountKillAsync(List<GameStatistic> Matches)
        {
            return await Task.Run(() =>
            {
                if (Matches.Count == 0)
                {
                    return 0;
                }
                else
                {
                    double CountKill = 0;
                    foreach (var item in Matches)
                    {
                        CountKill += item.CountKills;
                    }
                    return CountKill;
                }
            });
        }

        public static async Task<double> AVGKillRateAsync(List<GameStatistic> Matches)
        {
            return await Task.Run(() =>
            {
                if (Matches.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Round(CountKillAsync(Matches).Result / Matches.Count, 1);
                }
            });
        }

        public static async Task<double> AVGKillRatePercentageAsync(List<GameStatistic> Matches)
        {
            return await Task.Run(() =>
            {
                return Math.Round(AVGKillRateAsync(Matches).Result / 4 * 100, 2);
            });
        }

        public static async Task<double> PickRateAsync(int selectedMatches, int countAllMatch)
        {
            return await Task.Run(() =>
            {
                double PickRate = Math.Round((double)selectedMatches / countAllMatch * 100, 2);
                return PickRate;
            });
        }

        public static async Task<int> CountMatchWinAsync(List<GameStatistic> Matches)
        {
            return await Task.Run(() =>
            {
                return Matches.Where(gs => gs.CountKills == 3 | gs.CountKills == 4).Count();
            });
        }

        public static async Task<double> WinRateAsync(double selectedCharacterMatchesWin, int countAllMatch)
        {
            return await Task.Run(() =>
            {
                return Math.Round((double)selectedCharacterMatchesWin / countAllMatch * 100, 2);
            });
        }

        public static async Task<(double KillingZero, double KillingOne, double KillingTwo, double KillingThree, double KillingFour)> KillDistributionAsync(List<GameStatistic> Matches)
        {
            return await Task.Run(() =>
            {
                double Zero = Math.Round((double)Matches.Where(gs => gs.CountKills == 0).Count() / Matches.Count * 100, 2);
                double One = Math.Round((double)Matches.Where(gs => gs.CountKills == 1).Count() / Matches.Count * 100, 2);
                double Two = Math.Round((double)Matches.Where(gs => gs.CountKills == 2).Count() / Matches.Count * 100, 2);
                double Three = Math.Round((double)Matches.Where(gs => gs.CountKills == 3).Count() / Matches.Count * 100, 2);
                double Four = Math.Round((double)Matches.Where(gs => gs.CountKills == 4).Count() / Matches.Count * 100, 2);

                return (Zero, One, Two, Three, Four);
            });
        }

        #endregion

        #region Расширение расчеты ( При выборе конкретного киллера на странице KillerPage )

        #region Средний счет за промежуток времени

        public static async Task<List<AverageScoreTracker>> AverageScoreForPeriodTimeAsyncAsync(List<GameStatistic> matches, TypeTime typeTime)
        {
            List<AverageScoreTracker> averageScoreTrackers = new();

            (DateTime FirstDate, DateTime LastDate) = await CalculationTime.FirstAndLastDateMatchAsync(matches);

            return await Task.Run(() =>
            {
                //Определение шага для цикла в зависимости от типа времени
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

                for (var date = FirstDate; date <= LastDate; date = Increment(date))
                {
                    var matchByDate = matches.Where(x =>
                        x.DateTimeMatch.HasValue &&
                        (typeTime == TypeTime.Year ? x.DateTimeMatch.Value.Year == date.Year :
                         typeTime == TypeTime.Month ? x.DateTimeMatch.Value.Year == date.Year && x.DateTimeMatch.Value.Month == date.Month :
                         x.DateTimeMatch.Value.Date == date.Date))
                        .ToList();

                    if (matchByDate.Count == 0)
                    {
                        averageScoreTrackers.Add(new AverageScoreTracker
                        {
                            AvgScore = 0,
                            DateTime = date.ToString(dateFormat)
                        });
                    }
                    else
                    {
                        int account = matchByDate.Sum(x => x.IdKillerNavigation.KillerAccount);
                        double avg = matchByDate.Count > 0 ? Math.Round(account / (double)matchByDate.Count, 0) : 0;

                        averageScoreTrackers.Add(new AverageScoreTracker
                        {
                            AvgScore = avg,
                            DateTime = date.ToString(dateFormat)
                        });
                    }
                }
                return averageScoreTrackers;
            });
        }

        #endregion
       
        #region Киллрейт за промежуток времени 

        public static async Task<List<KillerKillRateTracker>> KillRateForPeriodTimeAsync(List<GameStatistic> matches, TypeTime typeTime)
        {
            List<KillerKillRateTracker> killRateTracker = [];

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
                        killRateTracker.Add(new KillerKillRateTracker
                        {
                            KillRate = 0,
                            DateTime = date.ToString(dateFormat)
                        });
                    }
                    else
                    {
                        killRateTracker.Add(new KillerKillRateTracker
                        {
                            KillRate = await AVGKillRateAsync(matchByDate),
                            DateTime = date.ToString(dateFormat),
                        });
                    }
                }
                return killRateTracker;
            });
        }

        #endregion

        #region Винрейт за промежуток времени

        public static async Task<List<KillerWinRateTracker>> WinRateForPeriodTimeAsync(List<GameStatistic> matches, TypeTime typeTime)
        {
            List<KillerWinRateTracker> winRateTracker = [];

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
                        winRateTracker.Add(new KillerWinRateTracker
                        {
                            WinRate = 0,
                            DateTime = date.ToString(dateFormat)
                        });
                    }
                    else
                    {
                        winRateTracker.Add(new KillerWinRateTracker
                        {
                            WinRate = await WinRateAsync(await CountMatchWinAsync(matches), matchByDate.Count),
                            DateTime = date.ToString(dateFormat),
                        });
                    }
                }
                return winRateTracker;
            });
        }

        #endregion

        #region Статистика по повесам ( 0 повесов - 12 повесов )

        public static async Task<List<KillerHooksTracker>> CountHooksAsync(List<GameStatistic> matches)
        {
            List<KillerHooksTracker> killerHooksTrackers = [];

            return await Task.Run(() =>
            {
                for (int i = 0; i <= 12; i++)
                {
                    double HookPercentages = Math.Round((double)matches.Count(x => x.CountHooks == i) / matches.Count * 100, 2);
                    HookPercentages = double.IsNaN(HookPercentages) ? 0 : HookPercentages;

                    var killerHook = new KillerHooksTracker
                    {
                        CountHookName = CalculationsCollections.Names[i],
                        CountHookPercentages = HookPercentages,
                        CountGame = matches.Where(x => x.CountHooks == i).Count(),
                    };

                    killerHooksTrackers.Add(killerHook);
                }
                return killerHooksTrackers;
            });
        }

        #endregion

        #endregion
    }
}