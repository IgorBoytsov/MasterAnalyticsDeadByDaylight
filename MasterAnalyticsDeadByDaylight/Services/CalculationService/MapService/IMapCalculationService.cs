using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

namespace MasterAnalyticsDeadByDaylight.Services.CalculationService.MapService
{
    public interface IMapCalculationService
    {
        public Task<List<GameStatistic>> GetMatchForMapAsync(int idMap, string typeSortStatValue);
        public Task<List<GameStatistic>> GetMatchForMeasurementAsync(int idMeasurement, string typeSortStatValue);
        public Task<List<MapStat>> CalculatingMapStatAsync(List<GameStatistic> matches);

        public Task<double> CalculatingPickRate(int selectedMatchesOnMap, int countAllMatch);
        public Task<int> CalculatingEscapeSurvivor(int countMatch, int countKill);
        public Task<double> CalculatingEscapeRate(int escapeSurvivor, int countMatch);
        public Task<double> CalculatingWinRateRate(int SelectedMatchesWon, int SelectedAllMatch);
        public Task<int> CalculatingRandomMap(List<GameStatistic> matches);
        public Task<int> CalculatingOfferingMap(List<GameStatistic> matches);
        public Task<double> CalculatingDropMapPercent(int countMatchOnMap, int CountAllMatches);
    }
}
