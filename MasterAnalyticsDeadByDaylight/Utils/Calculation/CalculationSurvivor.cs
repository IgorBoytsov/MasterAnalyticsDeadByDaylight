using MasterAnalyticsDeadByDaylight.MVVM.Model.ChartModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;

namespace MasterAnalyticsDeadByDaylight.Utils.Calculation
{
    public static class CalculationSurvivor
    {
        #region Статистика по типам смертей выижвших

        public static async Task<List<SurvivorTypeDeathTracker>> TypeDeathSurvivorsAsync(List<GameStatistic> matches)
        {
            List<SurvivorTypeDeathTracker> survivorTypeDeathTrackers = [];

            //TODO: Убрать вызов DataService, передавать его сюда из вне.
            Func<MasterAnalyticsDeadByDaylightDbContext> _contextFactory = () => new MasterAnalyticsDeadByDaylightDbContext();
            var _dataService = new DataService(_contextFactory);
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

        #region Количество анонимных игроков

        public static async Task<List<SurvivorAnonymousTracker>> AnonymousPlayersAsync(List<GameStatistic> matches)
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

        #region Количество ливающих игроков

        public static async Task<List<SurvivorBotTracker>> PlayersBotAsync(List<GameStatistic> matches)
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

        #region Количество игроков по платформам

        public static async Task<List<PlayerPlatformTracker>> PlayersByPlatformsAsync(List<GameStatistic> matches)
        {
            List<PlayerPlatformTracker> playerPlatformTracker = [];

            //TODO: Убрать вызов DataService, передавать его сюда из вне.
            Func<MasterAnalyticsDeadByDaylightDbContext> _contextFactory = () => new MasterAnalyticsDeadByDaylightDbContext();
            var _dataService = new DataService(_contextFactory);
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
    }
}