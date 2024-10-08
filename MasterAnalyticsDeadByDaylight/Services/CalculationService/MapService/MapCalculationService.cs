using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace MasterAnalyticsDeadByDaylight.Services.CalculationService.MapService
{
    public class MapCalculationService(Func<MasterAnalyticsDeadByDaylightDbContext> contextFactory) : IMapCalculationService
    {
        private readonly DataService _dataService = new(contextFactory);

        public async Task<List<GameStatistic>> GetMatchForMapAsync(int idMap, string typeSortStatValue)
        {

            return typeSortStatValue switch
            {
                "Общая" => await _dataService.GetAllDataInListAsync<GameStatistic>(x => x
                                 .Include(x => x.IdMapNavigation)
                                     .Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdKillerNavigation)
                                        .Include(x => x.IdMapNavigation).ThenInclude(x => x.IdMeasurementNavigation)
                                             .Include(x => x.IdWhoPlacedMapNavigation)
                                                 .Include(x => x.IdWhoPlacedMapWinNavigation)
                                                     .Where(x => x.IdMapNavigation.IdMap == idMap)),
                "Личная за Убийц" => await _dataService.GetAllDataInListAsync<GameStatistic>(x => x
                                           .Include(x => x.IdMapNavigation)
                                                .Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdKillerNavigation)
                                                    .Include(x => x.IdMapNavigation).ThenInclude(x => x.IdMeasurementNavigation)
                                                        .Include(x => x.IdWhoPlacedMapNavigation)
                                                            .Include(x => x.IdWhoPlacedMapWinNavigation)
                                                                .Where(x => x.IdKillerNavigation.IdAssociation == 1)
                                                                    .Where(x => x.IdMapNavigation.IdMap == idMap)),
                "Личная за Выживших" => await _dataService.GetAllDataInListAsync<GameStatistic>(x => x
                                              .Include(x => x.IdMapNavigation)
                                                   .Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdKillerNavigation)
                                                        .Include(x => x.IdMapNavigation).ThenInclude(x => x.IdMeasurementNavigation)
                                                            .Include(x => x.IdWhoPlacedMapNavigation).Include(x => x.IdWhoPlacedMapWinNavigation)
                                                                .Where(x => x.IdKillerNavigation.IdAssociation == 3)
                                                                    .Where(x => x.IdMapNavigation.IdMap == idMap)),
                _ => throw new Exception("Такого типа статистик нету")
            };
        }

        public async Task<List<GameStatistic>> GetMatchForMeasurementAsync(int idMeasurement, string typeSortStatValue)
        {
            return typeSortStatValue switch
            {
                "Общая" => await _dataService.GetAllDataInListAsync<GameStatistic>(x => x
                                 .Include(x => x.IdMapNavigation)
                                    .Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdKillerNavigation)
                                        .Include(x => x.IdMapNavigation).ThenInclude(x => x.IdMeasurementNavigation)
                                            .Include(x => x.IdWhoPlacedMapNavigation)
                                                .Include(x => x.IdWhoPlacedMapWinNavigation)
                                                    .Where(x => x.IdMapNavigation.IdMeasurementNavigation.IdMeasurement == idMeasurement)),
                "Личная за Убийц" => await _dataService.GetAllDataInListAsync<GameStatistic>(x => x
                                           .Include(x => x.IdMapNavigation)
                                                .Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdKillerNavigation)
                                                    .Include(x => x.IdMapNavigation).ThenInclude(x => x.IdMeasurementNavigation)
                                                        .Include(x => x.IdWhoPlacedMapNavigation)
                                                            .Include(x => x.IdWhoPlacedMapWinNavigation)
                                                                .Where(x => x.IdKillerNavigation.IdAssociation == 1)
                                                                    .Where(x => x.IdMapNavigation.IdMeasurementNavigation.IdMeasurement == idMeasurement)),
                "Личная за Выживших" => await _dataService.GetAllDataInListAsync<GameStatistic>(x => x
                                              .Include(x => x.IdMapNavigation)
                                                   .Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdKillerNavigation)
                                                        .Include(x => x.IdMapNavigation).ThenInclude(x => x.IdMeasurementNavigation)
                                                            .Include(x => x.IdWhoPlacedMapNavigation)
                                                                .Include(x => x.IdWhoPlacedMapWinNavigation)
                                                                    .Where(x => x.IdKillerNavigation.IdAssociation == 3)
                                                                        .Where(x => x.IdMapNavigation.IdMeasurementNavigation.IdMeasurement == idMeasurement)),
                _ => throw new Exception("Такого типа статистик нету")
            };
        }

        #region Расчеты

        public async Task<double> CalculatingPickRate(int selectedMatchesOnMap, int countAllMatch)
        {
            return await Task.Run(() =>
            {
                return Math.Round((double)selectedMatchesOnMap / countAllMatch * 100, 2);
            });
        }

        public async Task<int>  CalculatingEscapeSurvivor(int countMatch, int countKill)
        {
            return await Task.Run(() => 
            {
                return countMatch * 4 - (int)countKill;
            });
        }

        public async Task<double> CalculatingEscapeRate(int escapeSurvivor, int countMatch)
        {
            return await Task.Run(() =>
            {
                return Math.Round((double)escapeSurvivor / (countMatch * 4) * 100, 2);
            }); 
        }

        public async Task<double> CalculatingWinRateRate(int SelectedMatchesWon, int SelectedAllMatch)
        {
            return await Task.Run(() =>
            {
                return Math.Round((double)SelectedMatchesWon / SelectedAllMatch * 100, 2);
            });
        }

        public async Task<int> CalculatingRandomMap(List<GameStatistic> matches)
        {
            return await Task.Run(() =>
            {
                return matches.Where(x => x.IdWhoPlacedMapWin == 1).Count();
            });
        }

        public async Task<int> CalculatingOfferingMap(List<GameStatistic> matches)
        {
            return await Task.Run(() =>
            {
                return matches.Where(x => x.IdWhoPlacedMapWin == 2 | x.IdWhoPlacedMapWin == 3 | x.IdWhoPlacedMapWin == 4).Count();
            });
        }

        public async Task<double> CalculatingDropMapPercent(int countMatchOnMap, int CountAllMatches)
        {
            return await Task.Run(() =>
            {
                return Math.Round((double)countMatchOnMap / CountAllMatches * 100, 2);
            });
        }

        #endregion

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
                    WinRateMapPercent = winRateMap,
                    PickRateMap = pickRateMap,
                };

                mapStatList.Add(mapstat);
            }
            return mapStatList;
        }

        #endregion
    }
}