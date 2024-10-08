using MasterAnalyticsDeadByDaylight.MVVM.Model.ChartModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Utils.Enum;

namespace MasterAnalyticsDeadByDaylight.Services.CalculationService.KillerService
{
    public interface IKillerCalculationService
    {
        public Task<List<GameStatistic>> GetMatchForKillerAsync(int idKiller, int IdPlayerAssociation);
        public Task<(DateTime First, DateTime Last)> FirstAndLastDateMatch();

        public Task<int> GetAllKillerCountMatch(int IdPlayerAssociation);
        public Task<List<GameStatistic>> GetSelectedKillerMatch(int idKiller, int IdPlayerAssociation);
        public Task<double> CalculatingCountKill(List<GameStatistic> killerMatches);
        public Task<double> CalculatingAVGKillRate(List<GameStatistic> killerMatches, double countKill);
        public Task<double> CalculatingAVGKillRatePercentage(double killRate);
        public Task<double> CalculatingKillRatePercentage(int countMatches, double countKills);
        public Task<double> CalculatingPickRate(int selectedCharacterMatches, int countAllMatch);
        public Task<double> CalculatingKillerCountMatchWin(List<GameStatistic> selectedKillerMatches);
        public Task<double> CalculatingWinRate(int selectedCharacterMatchesWin, int countAllMatch);
        public Task<(double KillingZero, double KillingOne, double KillingTwo, double KillingThree, double KillingFour)> CalculatingKillerKillDistribution(List<GameStatistic> selectedKillerMatches);

        public Task<List<KillerAverageScoreTracker>> AverageKillerScoreAsync(List<GameStatistic> matches, TypeTime typeTime);
        public Task<List<CountMatchTracker>> CountMatchAsync(List<GameStatistic> matches, TypeTime typeTime);
        public Task<List<ActivityByHoursTracker>> ActivityByHourAsync(List<GameStatistic> matches);
        public Task<List<KillerKillRateTracker>> KillRateAsync(List<GameStatistic> matches, TypeTime typeTime);
        public Task<List<KillerWinRateTracker>> WinRateAsync(List<GameStatistic> matches, TypeTime typeTime);
        public Task<List<PlayerPlatformTracker>> CalculatingPlayersByPlatformsAsync(List<GameStatistic> matches);
        public Task<List<SurvivorBotTracker>> CalculatingPlayersBotAsync(List<GameStatistic> matches);
        public Task<List<SurvivorAnonymousTracker>> CalculatingAnonymousPlayerAsync(List<GameStatistic> matches);
        public Task<List<KillerHooksTracker>> CalculatingCountHooksAsync(List<GameStatistic> matches);
        public Task<List<RecentGeneratorsTracker>> CalculatingRecentGeneratorsAsync(List<GameStatistic> matches);
        public Task<List<SurvivorTypeDeathTracker>> CalculatingTypeDeathSurvivorAsync(List<GameStatistic> matches);

        public Task<List<GameStatistic>> MatchForPeriodTime(List<GameStatistic> allMatch, TypeTime typeTime, DateTime periodTime);
        public Task<List<GameStatistic>> MatchForPeriodTime(List<GameStatistic> allMatch, DateTime startingDate, DateTime endDate);
    }
}