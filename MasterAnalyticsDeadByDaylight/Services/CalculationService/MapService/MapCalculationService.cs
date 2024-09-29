using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using Microsoft.EntityFrameworkCore;

namespace MasterAnalyticsDeadByDaylight.Services.CalculationService.MapService
{
    public class MapCalculationService(Func<MasterAnalyticsDeadByDaylightDbContext> contextFactory) : IMapCalculationService
    {
        private readonly DataService _dataService = new(contextFactory);

        public async Task<List<GameStatistic>> GetGameStatisticsAsync()
        {
            return await _dataService.GetAllDataInListAsync<GameStatistic>(x => x
            .Include(x => x.IdMapNavigation)
                .Include(x => x.IdMapNavigation).ThenInclude(x => x.IdMeasurementNavigation)
                    .Include(x => x.IdKillerNavigation)
                        .Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdAssociationNavigation)
                            .Include(x => x.IdSurvivors1Navigation)
                                .Include(x => x.IdSurvivors2Navigation)
                                    .Include(x => x.IdSurvivors3Navigation)
                                        .Include(x => x.IdSurvivors4Navigation));
        }


        #region Количество сыгранных матчей на картах ( W\R, K\R, без подношений, с подношениями)

        public async Task<List<MapStat>> CalculatingMapStatAsync(List<GameStatistic> matches)
        {
            List<MapStat> mapStatList = [];
            List<Map> MapList = await _dataService.GetAllDataInListAsync<Map>(x => x.Include(x => x.IdMeasurementNavigation));

            foreach (var item in MapList)
            {
                double countGameMap = matches.Where(x => x.IdMap == item.IdMap).Count();
                double winRateMap = Math.Round((double)matches.Where(x => x.IdMap == item.IdMap).Where(x => x.CountKills == 3 | x.CountKills == 4).Count() / matches.Where(x => x.IdMap == item.IdMap).Count() * 100, 2);
                winRateMap = double.IsNaN(winRateMap) ? 0 : winRateMap;

                double pickRateMap = Math.Round((double)matches.Where(x => x.IdMap == item.IdMap).Count() / matches.Count * 100, 2);
                pickRateMap = double.IsNaN(pickRateMap) ? 0 : pickRateMap;

                var mapstat = new MapStat
                {
                    MapName = item.MapName,
                    MapImage = item.MapImage,
                    MapMeasurement = item.IdMeasurementNavigation.MeasurementName,
                    CountGame = countGameMap,
                    WinRateMap = winRateMap,
                    PickRateMap = pickRateMap,
                };

                mapStatList.Add(mapstat);
            }
            return mapStatList;
        }

        #endregion
    }
}
