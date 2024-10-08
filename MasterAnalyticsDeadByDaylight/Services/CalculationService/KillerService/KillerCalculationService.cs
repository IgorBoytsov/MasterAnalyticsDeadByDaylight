using MasterAnalyticsDeadByDaylight.MVVM.Model.ChartModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace MasterAnalyticsDeadByDaylight.Services.CalculationService.KillerService
{
    public class KillerCalculationService(Func<MasterAnalyticsDeadByDaylightDbContext> contextFactory) : IKillerCalculationService
    {
        private readonly DataService _dataService = new(contextFactory);

        private static ObservableCollection<string> Names { get; set; } = ["Ноль", "Один", "Два", "Три", "Четыре", "Пять", "Шесть", "Семь", "Восемь", "Девять", "Десять", "Одиннадцать", "Двенадцать"];

        #region Вспомогательные методы

        public async Task<List<GameStatistic>> GetMatchForKillerAsync(int idKiller, int IdPlayerAssociation)
        {
            return await _dataService.GetAllDataInListAsync<GameStatistic>(x => x
                    .Include(x => x.IdMapNavigation).ThenInclude(x => x.IdMeasurementNavigation)
                        .Include(x => x.IdKillerNavigation)
                            .Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdAssociationNavigation)
                                .Include(x => x.IdSurvivors1Navigation)
                                    .Include(x => x.IdSurvivors2Navigation)
                                        .Include(x => x.IdSurvivors3Navigation)
                                            .Include(x => x.IdSurvivors4Navigation)
                    .Where(x => x.IdKillerNavigation.IdKiller == idKiller && x.IdKillerNavigation.IdAssociation == IdPlayerAssociation));
        }

        public async Task<(DateTime First, DateTime Last)> FirstAndLastDateMatch()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var First = await context.GameStatistics.OrderBy(x => x.DateTimeMatch).FirstOrDefaultAsync();
                var Last = await context.GameStatistics.OrderBy(x => x.DateTimeMatch).LastOrDefaultAsync();

                DateTime FirstMatch = First.DateTimeMatch.Value;
                DateTime LastMatch = Last.DateTimeMatch.Value;

                return (FirstMatch, LastMatch);
            }
        }

        #endregion

        #region Общие расчеты

        /// <summary>
        /// Считает количество всех сыгранных игр на всех киллерах.
        /// </summary>
        /// <param name="playerAssociation">
        /// Указывает сторону для вывода статистики. Возможные значения:
        /// - Я: Личная статистика.
        /// - Противник: Статистика по играм игроков, которые попадались против вас.
        /// </param>
        /// <returns>Возвращает общее количество сыгранных игр.</returns>
        public async Task<int> GetAllKillerCountMatch(int IdPlayerAssociation)
        {
            var matches = await _dataService.GetAllDataAsync<GameStatistic>(x => x
                .Where(x => x.IdKillerNavigation.IdAssociation == IdPlayerAssociation));

            return matches.Count();
        }

        /// <summary>
        /// Выводит количество всех сыгранных игр на конкретном киллере.
        /// </summary>
        /// <param name="killer"> Выбранный киллер. </param>
        /// <param name="playerAssociation"> Выбранная игровая ассоциация. </param>
        /// <returns>Возвращает общее количество сыгранных игр на конкретном киллере.</returns>
        public async Task<List<GameStatistic>> GetSelectedKillerMatch(int idKiller, int IdPlayerAssociation)
        {
            List<GameStatistic> matches = await _dataService.GetAllDataInListAsync<GameStatistic>(x => x
                                .Where(gs => gs.IdKillerNavigation.IdAssociation == IdPlayerAssociation)
                                    .Where(gs => gs.IdKillerNavigation.IdKillerNavigation.IdKiller == idKiller));

            return matches;
        }

        /// <summary>
        /// Считает количество количество килов на киллере. 
        /// <para>Требует передачу списка матчей.</para>
        /// <para>Рекомендуется использоваться метод <see cref="GetSelectedKillerMatch"/>, если нужно посчитать количество килов на выбранном киллере.</para>
        /// Подробнее в комментариях к методу.
        /// </summary>
        /// <param name="killerMatches"> Количество матчей. </param>
        /// <returns>Возвращает общее количество килов.</returns>
        public async Task<double> CalculatingCountKill(List<GameStatistic> killerMatches)
        {
            return await Task.Run(() =>
            {
                if (killerMatches.Count == 0)
                {
                    return 0;
                }
                else
                {
                    double CountKill = 0;
                    foreach (var item in killerMatches)
                    {
                        CountKill += item.CountKills;
                    }
                    return CountKill;
                }
            });
        }

        /// <summary>
        /// Считает K\R на киллере. 
        /// <para>Требует передачу списка матчей по конкретному киллеру.</para>
        /// <para>Рекомендуется использоваться метод <see cref="GetSelectedKillerMatch"/>.</para>
        /// Подробнее в комментариях к методу.
        /// </summary>
        /// <param name="killerMatches"> Количество матчей на конкретном персонаже. </param>
        /// <returns>Возвращает K\R на персонаже.</returns>
        public async Task<double> CalculatingAVGKillRate(List<GameStatistic> killerMatches, double countKill)
        {
            return await Task.Run(() =>
            {
                if (killerMatches.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Round(countKill / killerMatches.Count, 1);
                }
            });
        }

        /// <summary>
        /// Считает K\R (% килов на персонажа) на киллере.
        /// Подробнее в комментариях к методу.
        /// </summary>
        /// <param name="killRate"> Килрейт на персонаже. </param>
        /// <returns>Возвращает K\R на персонаже в %.</returns>
        public async Task<double> CalculatingAVGKillRatePercentage(double killRate)
        {
            return await Task.Run(() =>
            {
                return Math.Round(killRate / 4 * 100, 2);
            });
        }

        public async Task<double> CalculatingKillRatePercentage(int countMatches ,double countKills)
        {
            return await Task.Run(() =>
            {
                return Math.Round(countKills / (countMatches * 4) * 100, 2);
            });
        }

        /// <summary>
        /// Считает PickRate (Популярность персонажа, как часто на нем играют) на персонаже.
        /// <para><paramref name="selectedCharacterMatches"/> - требует количество матчей на выбранном персонаже.Рекомендуется использовать метод <see cref="GetSelectedKillerMatch"/> для киллера и <see cref="GetSelectedSurvivorMatch"/> для выжившего .</para>
        /// <para><paramref name="countAllMatch"/> - требует количество матчей на всех персонажах. Рекомендуется использовать метод <see cref="GetAllKillerCountMatch"/>.</para>
        /// Подробнее в комментариях к методу.
        /// </summary>
        /// <param name="selectedCharacterMatches"> Количество матчей на выбранном персонаже. </param>
        /// <param name="countAllMatch"> Количество матчей на всех персонажах. </param>
        /// <returns>Возвращает K\R на персонаже в %.</returns>
        public async Task<double> CalculatingPickRate(int selectedCharacterMatches, int countAllMatch)
        {
            return await Task.Run(() =>
            {
                double PickRate = Math.Round((double)selectedCharacterMatches / countAllMatch * 100, 2);
                return PickRate;
            });
        }

        /// <summary>
        /// Считает PickRate (Популярность персонажа, как часто на нем играют) на персонаже.
        /// <para><paramref name="selectedKillerMatches"/> - требует количество матчей на выбранном персонаже.Рекомендуется использовать метод <see cref="GetSelectedKillerMatch"/>.</para>
        /// Подробнее в комментариях к методу.
        /// </summary>
        /// <param name="selectedKillerMatches"> Список матчей на выбранном киллере. </param>
        /// <returns>Возвращает количество выигранных матчей на киллере.</returns>
        public async Task<double> CalculatingKillerCountMatchWin(List<GameStatistic> selectedKillerMatches)
        {
            return await Task.Run(() =>
            {
                double PickRate = selectedKillerMatches.Where(gs => gs.CountKills == 3 | gs.CountKills == 4).Count();
                return PickRate;
            });
        }

        /// <summary>
        /// Считает WinRate (Количество выигранных игр) на персонаже.
        /// <para><paramref name="selectedCharacterMatchesWin"/> - требует количество выигранных матчей на выбранном персонаже.Рекомендуется использовать метод <see cref="GetSelectedKillerMatch"/> для киллера и <see cref="GetSelectedSurvivorMatch"/> для выжившего .</para>  
        /// <para><paramref name="countAllMatch"/> - требует количество матчей на всех персонажах. Рекомендуется использовать метод <see cref="GetAllKillerCountMatch"/> для киллера и <see cref="GetAllSurvivorCountMatch"/> для выживших.</para>
        /// Подробнее в комментариях к методу.
        /// </summary>
        /// <param name="selectedCharacterMatchesWin"> Список выигранных матчей на выбранном персонаже. </param>
        /// <param name="countAllMatch"> Список всех матчей на выбранном персонаже. </param>
        /// <returns>Возвращает W\R на персонаже в %.</returns>
        public async Task<double> CalculatingWinRate(int selectedCharacterMatchesWin, int countAllMatch)
        {
            return await Task.Run(() =>
            {
                double WinRate = Math.Round((double)selectedCharacterMatchesWin / countAllMatch * 100, 2);
                return WinRate;
            });
        }

        /// <summary>
        /// Считает подробный % убийств по килам от 0 до 4.
        /// <para><paramref name="selectedKillerMatches"/> - требует количество матчей на конкретном киллере. Рекомендуется использовать метод <see cref="GetSelectedKillerMatch"/>.</para>  
        /// Подробнее в комментариях к методу.
        /// </summary>
        /// <returns> Возвращает кортеж чисел.</returns>
        public async Task<(double KillingZero, double KillingOne, double KillingTwo, double KillingThree, double KillingFour)> CalculatingKillerKillDistribution(List<GameStatistic> selectedKillerMatches)
        {
            return await Task.Run(() =>
            {
                double Zero = Math.Round((double)selectedKillerMatches.Where(gs => gs.CountKills == 0).Count() / selectedKillerMatches.Count * 100, 2);
                double One = Math.Round((double)selectedKillerMatches.Where(gs => gs.CountKills == 1).Count() / selectedKillerMatches.Count * 100, 2);
                double Two = Math.Round((double)selectedKillerMatches.Where(gs => gs.CountKills == 2).Count() / selectedKillerMatches.Count * 100, 2);
                double Three = Math.Round((double)selectedKillerMatches.Where(gs => gs.CountKills == 3).Count() / selectedKillerMatches.Count * 100, 2);
                double Four = Math.Round((double)selectedKillerMatches.Where(gs => gs.CountKills == 4).Count() / selectedKillerMatches.Count * 100, 2);

                return (Zero, One, Two, Three, Four);
            });
        }

        #endregion

        #region Расширеные расчеты

        #region Средний счет за промежуток времени

        /// <summary>
        /// Выполняет подсчет среднего счета за разные периоды времени : Год, Месяц, День.
        /// <para><paramref name="matches"/> - Требует передачу матчей, включающих в себя <see cref="Include"/> c <see cref=".Include(x => x.IdKillerNavigation)"/>, <see cref=".Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdAssociationNavigation)"/>.</para>
        /// </summary>
        /// <param name="matches"> Список матчей, по которым будет идти подсчет.</param>
        /// <param name="typeTime"> Участок времени, на который будут делится записи. </param>
        /// <returns> Список среднего счета за определённый срок.</returns>
        public async Task<List<KillerAverageScoreTracker>> AverageKillerScoreAsync(List<GameStatistic> matches, TypeTime typeTime)
        {
            List<KillerAverageScoreTracker> averageScoreTrackers = new();

            (DateTime FirstDate, DateTime LastDate) = await FirstAndLastDateMatch();

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
                        averageScoreTrackers.Add(new KillerAverageScoreTracker
                        {
                            AvgScore = 0,
                            DateTime = date.ToString(dateFormat)
                        });
                    }
                    else
                    {
                        int account = matchByDate.Sum(x => x.IdKillerNavigation.KillerAccount);
                        double avg = matchByDate.Count > 0 ? Math.Round(account / (double)matchByDate.Count, 0) : 0;

                        averageScoreTrackers.Add(new KillerAverageScoreTracker
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

        #region Количество сыграных за промежуток времени

        /// <summary>
        /// Выполняет подсчет количества сыгранных матчей за разные отрезки времени : Год, Месяц, День.
        /// <para><paramref name="matches"/> - Требует передачу матчей, включающих в себя <see cref="Include"/> c <see cref=".Include(x => x.IdKillerNavigation)"/>, <see cref=".Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdAssociationNavigation)"/>.</para>
        /// <para> Если расчет нужно произвести по выжившим, тогда нужно вложить сующие данные:  <see cref="Include"/> c <see cref=".Include(x => x.IdSurvivors1Navigation)"/>, <see cref=".Include(x => x.IdSurvivors1Navigation).ThenInclude(x => x.IdSurvivorNavigation)"/>. Аналогично сделать для всех 4-ёх выживших.</para>
        /// </summary>
        /// <param name="matches"> Список матчей, по которым будет идти подсчет.</param>
        /// <param name="typeTime"> Участок времени, на который будут делится записи. </param>
        /// <returns> Список среднего счета за определённый срок.</returns>
        public async Task<List<CountMatchTracker>> CountMatchAsync(List<GameStatistic> matches, TypeTime typeTime)
        {
            List<CountMatchTracker> countMatchTracker = [];

            (DateTime First, DateTime Last) = await FirstAndLastDateMatch();

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
                    var matchByDate = await MatchForPeriodTime(matches, typeTime, date);

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

        #region Активность по часам

        public async Task<List<ActivityByHoursTracker>> ActivityByHourAsync(List<GameStatistic> matches)
        {
            List<ActivityByHoursTracker> activityByHoursTracker = [];

            List<DateTime> hours = await GetHourList();

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

        #region Киллрейт за промежуток времени 

        /// <summary>
        /// Выполняет подсчет среднего киллрейта за разные отрезки времени : Год, Месяц, День.
        /// <para><paramref name="matches"/> - Требует передачу матчей, включающих в себя <see cref="Include"/> c <see cref=".Include(x => x.IdKillerNavigation)"/>, <see cref=".Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdAssociationNavigation)"/>.</para>
        /// </summary>
        /// <param name="matches"> Список матчей, по которым будет идти подсчет.</param>
        /// <param name="typeTime"> Участок времени, на который будут делится записи. </param>
        /// <returns> Список среднего счета за определённый срок.</returns>
        public async Task<List<KillerKillRateTracker>> KillRateAsync(List<GameStatistic> matches, TypeTime typeTime)
        {
            List<KillerKillRateTracker> killRateTracker = [];

            (DateTime First, DateTime Last) = await FirstAndLastDateMatch();

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
                    var matchByDate = await MatchForPeriodTime(matches, typeTime, date);

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
                            KillRate = await CalculatingAVGKillRate(matchByDate, await CalculatingCountKill(matchByDate)),
                            DateTime = date.ToString(dateFormat),
                        });
                    }
                }
                return killRateTracker;
            });
        }

        #endregion

        #region Винрейт за промежуток времени

        public async Task<List<KillerWinRateTracker>> WinRateAsync(List<GameStatistic> matches, TypeTime typeTime)
        {
            List<KillerWinRateTracker> winRateTracker = [];

            (DateTime First, DateTime Last) = await FirstAndLastDateMatch();

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
                    var matchByDate = await MatchForPeriodTime(matches, typeTime, date);

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
                            WinRate = await CalculatingWinRate((int)await CalculatingKillerCountMatchWin(matchByDate), matchByDate.Count),
                            DateTime = date.ToString(dateFormat),
                        });
                    }
                }
                return winRateTracker;
            });
        }

        #endregion

        #region Количество игроков по платформам

        public async Task<List<PlayerPlatformTracker>> CalculatingPlayersByPlatformsAsync(List<GameStatistic> matches)
        {
            List<PlayerPlatformTracker> playerPlatformTracker = [];

            List<Platform> platforms = await _dataService.GetAllDataInListAsync<Platform>();

            return await Task.Run(() =>
            {
                foreach (var item in platforms)
                {
                    int first = matches.Count(x => x.IdSurvivors1Navigation.IdPlatform == item.IdPlatform);
                    int second = matches.Count(x => x.IdSurvivors2Navigation.IdPlatform == item.IdPlatform);
                    int third = matches.Count(x => x.IdSurvivors3Navigation.IdPlatform == item.IdPlatform);
                    int fourth = matches.Count(x => x.IdSurvivors4Navigation.IdPlatform == item.IdPlatform);

                    int platformCount = first + second + third + fourth;

                    double platformPercentages = Math.Round((double)platformCount / (matches.Count * 4) * 100, 2);
                    platformPercentages = double.IsNaN(platformPercentages) ? 0 : platformPercentages;

                    playerPlatformTracker.Add(new PlayerPlatformTracker
                    {
                        PlatformName = item.PlatformName,
                        PlayerCount = platformCount,
                        PlatformPercentages = platformPercentages
                    });
                }
                return playerPlatformTracker;
            });
        }

        #endregion

        #region Количество ливающих игроков

        public async Task<List<SurvivorBotTracker>> CalculatingPlayersBotAsync(List<GameStatistic> matches)
        {
            List<SurvivorBotTracker> survivorBotTrackers = [];

            return await Task.Run(() =>
            {
                int firstBot = matches.Count(x => x.IdSurvivors1Navigation.Bot == true);
                int secondBot = matches.Count(x => x.IdSurvivors2Navigation.Bot == true);
                int thirdBot = matches.Count(x => x.IdSurvivors3Navigation.Bot == true);
                int fourthBot = matches.Count(x => x.IdSurvivors4Navigation.Bot == true);

                int playerBot = firstBot + secondBot + thirdBot + fourthBot;

                double playerBotPercentages = Math.Round((double)playerBot / (matches.Count * 4) * 100, 2);
                playerBotPercentages = double.IsNaN(playerBotPercentages) ? 0 : playerBotPercentages;

                var botTracker = new SurvivorBotTracker
                {
                    PlayerBot = playerBotPercentages,
                    CountPlayerBot = playerBot,
                    CountPlayer = matches.Count * 4,
                };

                survivorBotTrackers.Add(botTracker);

                return survivorBotTrackers;
            });
        }

        #endregion

        #region Количество анонимных игроков

        public async Task<List<SurvivorAnonymousTracker>> CalculatingAnonymousPlayerAsync(List<GameStatistic> matches)
        {
            List<SurvivorAnonymousTracker> survivorAnonymous = [];

            return await Task.Run(() =>
            {
                int firstAnonymous = matches.Count(x => x.IdSurvivors1Navigation.AnonymousMode == true);
                int secondAnonymous = matches.Count(x => x.IdSurvivors2Navigation.AnonymousMode == true);
                int thirdAnonymous = matches.Count(x => x.IdSurvivors3Navigation.AnonymousMode == true);
                int fourthAnonymous = matches.Count(x => x.IdSurvivors4Navigation.AnonymousMode == true);

                int playerAnonymous = firstAnonymous + secondAnonymous + thirdAnonymous + fourthAnonymous;

                double playerAnonymousPercentages = Math.Round((double)playerAnonymous / (matches.Count * 4) * 100, 2);
                playerAnonymousPercentages = double.IsNaN(playerAnonymousPercentages) ? 0 : playerAnonymousPercentages;

                var anonymousTracker = new SurvivorAnonymousTracker
                {
                    PlayerAnonymous = playerAnonymousPercentages,
                    CountPlayerAnonymous = playerAnonymous,
                    CountPlayer = matches.Count * 4,
                };

                survivorAnonymous.Add(anonymousTracker);

                return survivorAnonymous;
            });
        }

        #endregion

        #region Статистика по повесам ( 0 повесов - 12 повесов )

        public async Task<List<KillerHooksTracker>> CalculatingCountHooksAsync(List<GameStatistic> matches)
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
                        CountHookName = Names[i],
                        CountHookPercentages = HookPercentages,
                        CountGame = matches.Where(x => x.CountHooks == i).Count(),
                    };

                    killerHooksTrackers.Add(killerHook);
                }
                return killerHooksTrackers;
            });
        }

        #endregion

        #region Статистика по оставшимся генераторам 

        public async Task<List<RecentGeneratorsTracker>> CalculatingRecentGeneratorsAsync(List<GameStatistic> matches)
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
                        CountRecentGeneratorsName = Names[i],
                        CountRecentGeneratorsPercentages = RecentGeneratorsPercentages,
                        CountGame = matches.Where(x => x.NumberRecentGenerators == i).Count(),
                    };
                    recentGeneratorsTrackers.Add(recentGenerators);
                }
                return recentGeneratorsTrackers;
            });
        }

        #endregion

        #region Статистика по типам смертей выижвших

        public async Task<List<SurvivorTypeDeathTracker>> CalculatingTypeDeathSurvivorAsync(List<GameStatistic> matches)
        {
            List<SurvivorTypeDeathTracker> survivorTypeDeathTrackers = [];
            List<TypeDeath> typeDeath = await _dataService.GetAllDataInListAsync<TypeDeath>();

            return await Task.Run(() =>
            {
                foreach (var item in typeDeath)
                {
                    int first = matches.Count(x => x.IdSurvivors1Navigation.IdTypeDeath == item.IdTypeDeath);
                    int second = matches.Count(x => x.IdSurvivors2Navigation.IdTypeDeath == item.IdTypeDeath);
                    int third = matches.Count(x => x.IdSurvivors3Navigation.IdTypeDeath == item.IdTypeDeath);
                    int fourth = matches.Count(x => x.IdSurvivors4Navigation.IdTypeDeath == item.IdTypeDeath);

                    int countSurvivorDeath = first + second + third + fourth;

                    double countSurvivorTypeDeathPercentages = Math.Round((double)countSurvivorDeath / (matches.Count * 4) * 100, 2);

                    var SurvivorTypeDeath = new SurvivorTypeDeathTracker
                    {
                        TypeDeathName = item.TypeDeathName,
                        TypeDeathPercentages = countSurvivorTypeDeathPercentages,
                        CountGame = countSurvivorDeath
                    };
                    survivorTypeDeathTrackers.Add(SurvivorTypeDeath);
                }
                return survivorTypeDeathTrackers;
            });
        }

        #endregion

        #endregion

        #region Расчеты по времени  

        /// <summary>
        /// Вспомогательный метод для расчетов статистики за период времени. Выводит список матчей за определённый период времени.
        /// </summary>
        /// <param name="allMatch">Список матчей, из которых нужно вывести матчи за определенный промежуток времени.</param>
        /// <param name="typeTime">Перечисления типа времени : Год, месяц, неделя, день.</param>
        /// <param name="periodTime">Точное дата, за которую нужно вывести матчи.</param>
        /// <returns>Список матчей за определенную дату - месяц/год/день.</returns>
        public async Task<List<GameStatistic>> MatchForPeriodTime(List<GameStatistic> allMatch, TypeTime typeTime, DateTime periodTime)
        {
            return await Task.Run(() =>
            {
                var matchByDate = allMatch.Where(x =>
                        x.DateTimeMatch.HasValue &&
                        (typeTime == TypeTime.Year ? x.DateTimeMatch.Value.Year == periodTime.Year :
                        typeTime == TypeTime.Month ? x.DateTimeMatch.Value.Year == periodTime.Year && x.DateTimeMatch.Value.Month == periodTime.Month :
                        x.DateTimeMatch.Value.Date == periodTime.Date))
                        .ToList();

                return matchByDate;
            });
        }

        // TODO: Реализовать вывод матчей по периоду времени, между 1 и 2 датой.
        public async Task<List<GameStatistic>> MatchForPeriodTime(List<GameStatistic> allMatch, DateTime startingDate, DateTime endDate)
        {
            return await Task.Run(() =>
            {
                var matchByDate = allMatch;
                return matchByDate;
            });
        }

        private async Task<List<DateTime>> GetHourList()
        {
            return await Task.Run(() =>
            {
                List<DateTime> hoursList = new List<DateTime>();
                DateTime startDate = DateTime.MinValue;

                for (int i = 0; i < 24; i++)
                {
                    hoursList.Add(startDate.AddHours(i));
                }
                return hoursList;
            });
        }

        #endregion
    }
}