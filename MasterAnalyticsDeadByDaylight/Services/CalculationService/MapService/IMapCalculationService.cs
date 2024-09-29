using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

namespace MasterAnalyticsDeadByDaylight.Services.CalculationService.MapService
{
    public interface IMapCalculationService
    {
        public Task<List<GameStatistic>> GetGameStatisticsAsync();
        public Task<List<MapStat>> CalculatingMapStatAsync(List<GameStatistic> matches);
    }
}
