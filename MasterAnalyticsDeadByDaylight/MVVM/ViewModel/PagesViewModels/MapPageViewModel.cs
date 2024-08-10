using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.View.Pages;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    class MapPageViewModel : BaseViewModel
    {

        #region Колекции 

        public ObservableCollection<MapStat> MapStatList { get; set; }

        public ObservableCollection<MapStat> MapStatSortedList { get; set; }

        public ObservableCollection<PlayerAssociation> PlayerAssociationList { get; set; }

        public ObservableCollection<string> SortingList { get; set; } =
            [
            "Измерению (Убыв.)", "Измерению (Возр.)",
            "Дате выхода (Убыв.)", "Дате выхода (Возр.)",
            "Алфавит (Я-А)", "Алфавит (А-Я)",
            "Пикрейт (Убыв.)", "Пикрейт (Возр.)",
            "Винрейт (Убыв.)","Винрейт (Возр.)",
            "Киллрейт (Убыв.)","Киллрейт (Возр.)",
            "Количеству сыгранных игр (Убыв.)", "Количеству сыгранных игр (Возр.)",
            "Количеству игр с подношениями (Убыв.)", "Количеству игр с подношениями (Возр.)",
            "Количеству игр без подношений (Убыв.)", "Количеству игр без подношений (Возр.)",
            ];

        public ObservableCollection<string> Association { get; set; } = [ "Общая", "Личная за Убийц", "Личная за Выживших"];

        #endregion

        #region Свойства

        private string _selectedMapStatSortItem;
        public string SelectedMapStatSortItem
        {
            get => _selectedMapStatSortItem;
            set
            {
                _selectedMapStatSortItem = value;
                SortMapStatList();
                OnPropertyChanged();
            }
        }

        private string _selectedAssociation; // ВЫБОР ЭЛЕМЕНТА ИЗ СПИСКА
        public string SelectedAssociation
        {
            get => _selectedAssociation;
            set
            {
                _selectedAssociation = value;
                SortMapStatList();
                OnPropertyChanged();
            }
        }

        private PlayerAssociation _selectedTypePlayerItem;
        public PlayerAssociation SelectedTypePlayerItem
        {
            get => _selectedTypePlayerItem;
            set
            {
                _selectedTypePlayerItem = value;
                OnPropertyChanged();
            }
        }

        private string _searchTextBox;
        public string SearchTextBox
        {
            get => _searchTextBox;
            set
            {
                _searchTextBox = value;
                SearchMapName();
                OnPropertyChanged();
            }
        }

        #endregion

        public MapPageViewModel()
        {
            PlayerAssociationList = [];
            MapStatList = [];
            MapStatSortedList = [];

            GetPlayerAssociation();

            SelectedAssociation = Association[1];
            SelectedMapStatSortItem = SortingList[1];
            SelectedTypePlayerItem = PlayerAssociationList.First();
        }

        #region Команды

        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { GetMapStatisticData(); }); }

        #endregion

        #region Методы

        //private void GetMapStatisticData()
        //{
        //    MapStatList.Clear();
        //    using (MasterAnalyticsDeadByDaylightDbContext context = new())
        //    {
        //        List<Map> Maps = context.Maps.Include(mapMeasurement => mapMeasurement.IdMeasurementNavigation).ToList();

        //        int CountMatch = context.GameStatistics
        //            .Include(gs => gs.IdKillerNavigation)
        //            .ThenInclude(killerInfo => killerInfo.IdAssociationNavigation)
        //            .Where(gs => gs.IdKillerNavigation.IdAssociation == 1)
        //            .Count();

        //        var idRandomMap = context.WhoPlacedMaps.FirstOrDefault(WPM => WPM.WhoPlacedMapName == "Рандом");
        //        var idOfferingMap = context.WhoPlacedMaps.FirstOrDefault(WPM => WPM.WhoPlacedMapName == "Выживший" | WPM.WhoPlacedMapName == "Я");

        //        foreach (var item in Maps)
        //        {
        //            List<GameStatistic> GameStat = context.GameStatistics
        //                .Include(gs => gs.IdMapNavigation)
        //                .Include(gs => gs.IdKillerNavigation)
        //                .ThenInclude(gs => gs.IdKillerNavigation)
        //                .Include(gs => gs.IdMapNavigation)
        //                .ThenInclude(Map => Map.IdMeasurementNavigation)
        //                .Where(gs => gs.IdKillerNavigation.IdAssociation == 1)
        //                .Where(gs => gs.IdMapNavigation.IdMap == item.IdMap)
        //                .ToList();


        //            int MapRandom = GameStat.Where(gs => gs.IdWhoPlacedMapWin == idRandomMap.IdWhoPlacedMap).Count();
        //            int MapOffering = GameStat.Where(gs => gs.IdWhoPlacedMapWin == idOfferingMap.IdWhoPlacedMap).Count();

        //            double MapRandomPercent = Math.Round((double)MapRandom / GameStat.Count, 2);
        //            double MapOfferingPercent = Math.Round((double)MapOffering / GameStat.Count, 2);

        //            double PickRate = Math.Round((double)GameStat.Count / CountMatch, 2);

        //            var mapStat = new MapStat()
        //            {
        //                idMap = item.IdMap,
        //                MapName = item.MapName,
        //                MapMeasurement = item.IdMeasurementNavigation.MeasurementName,
        //                MapImage = item.MapImage,
        //                CountGame = GameStat.Count,
        //                PickRateMap = PickRate,
        //                FalloutMapRandom = MapRandom,
        //                FalloutMapRandomPercent = MapRandomPercent,
        //                FalloutMapOffering = MapOffering,
        //                FalloutMapOfferingPercent = MapOfferingPercent,
        //            };
        //            MapStatList.Add(mapStat);
        //        }
        //    }
        //}

        private void GetPlayerAssociation()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var playerAssociation = context.PlayerAssociations.ToList();
                foreach (var item in playerAssociation)
                {
                    PlayerAssociationList.Add(item);
                }
            }
        }

        private void GetMapStatisticData()
        {
            if (SelectedAssociation == "Общая")
            {
                GetTotalMapStat();
            }
            if (SelectedAssociation == "Личная за Убийц")
            {
                GetPersonalKillerMapStat();
            }
            if (SelectedAssociation == "Личная за Выживших")
            {
                GetPersonalSurvivorMapStat();
            }
        }

        private void GetTotalMapStat()
        {
            MapStatList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                int CountMatch = context.GameStatistics.Count();

                List<Map> maps = context.Maps.Include(x => x.IdMeasurementNavigation).ToList();

                WhoPlacedMap idRandomMap = context.WhoPlacedMaps.FirstOrDefault(WPM => WPM.WhoPlacedMapName == "Рандом");
                WhoPlacedMap idOfferingMap = context.WhoPlacedMaps.FirstOrDefault(WPM => WPM.WhoPlacedMapName == "Выживший" | WPM.WhoPlacedMapName == "Я");

                foreach (var map in maps)
                {
                    List<GameStatistic> GameStat = context.GameStatistics
                        .Include(gs => gs.IdMapNavigation)
                        .ThenInclude(Map => Map.IdMeasurementNavigation)
                        .Include(WPM => WPM.IdWhoPlacedMapNavigation)
                        .Include(WPM => WPM.IdWhoPlacedMapWinNavigation)
                        .Where(gs => gs.IdMapNavigation.IdMap == map.IdMap)
                        .ToList();

                    double PickRate = Math.Round((double)GameStat.Count / CountMatch * 100, 2);

                    int CountKills = 0;
                    foreach (var item in GameStat)
                    {
                        CountKills += item.CountKills;
                    }
                    double KillRate = Math.Round((double)CountKills / GameStat.Count / 4 * 100, 2);

                    int EscapeSurvivor = GameStat.Count * 4 - CountKills;
                    double Escape = Math.Round((double)EscapeSurvivor / (GameStat.Count * 4) * 100, 2);

                    double Win = Math.Round((double)GameStat.Where(x => x.CountKills == 3 | x.CountKills == 4).Count() / GameStat.Count * 100, 2);

                    int MapRandom = GameStat.Where(gs => gs.IdWhoPlacedMapWin == idRandomMap.IdWhoPlacedMap).Count();
                    int MapOffering = GameStat.Where(gs => gs.IdWhoPlacedMapWin == idOfferingMap.IdWhoPlacedMap).Count();

                    double MapRandomPercent = Math.Round((double)MapRandom / GameStat.Count * 100, 2);
                    double MapOfferingPercent = Math.Round((double)MapOffering / GameStat.Count * 100, 2);

                    var mapStat = new MapStat()
                    {
                        idMap = map.IdMap,
                        MapName = map.MapName,
                        MapMeasurement = map.IdMeasurementNavigation.MeasurementName,
                        idMapMeasurement = map.IdMeasurementNavigation.IdMeasurement,
                        MapImage = map.MapImage,
                        CountGame = GameStat.Count,
                        PickRateMap = PickRate,
                        KillRateMap = KillRate,
                        EscapeRateMap = Escape,
                        WinRateMap = Win,
                        FalloutMapRandom = MapRandom,
                        FalloutMapRandomPercent = MapRandomPercent,
                        FalloutMapOffering = MapOffering,
                        FalloutMapOfferingPercent = MapOfferingPercent,
                    };
                    MapStatList.Add(mapStat);
                }
            }
        }

        private void GetPersonalKillerMapStat()
        {
            MapStatList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                List<Map> Maps = context.Maps.Include(mapMeasurement => mapMeasurement.IdMeasurementNavigation).ToList();

                int CountMatch = context.GameStatistics
                    .Include(gs => gs.IdKillerNavigation)
                    .ThenInclude(killerInfo => killerInfo.IdAssociationNavigation)
                    .Where(gs => gs.IdKillerNavigation.IdAssociation == 1)
                    .Count();

                var idRandomMap = context.WhoPlacedMaps.FirstOrDefault(WPM => WPM.WhoPlacedMapName == "Рандом");
                var idOfferingMap = context.WhoPlacedMaps.FirstOrDefault(WPM => WPM.WhoPlacedMapName == "Выживший" | WPM.WhoPlacedMapName == "Я");

                foreach (var map in Maps)
                {
                    List<GameStatistic> GameStat = context.GameStatistics
                        .Include(gs => gs.IdMapNavigation)
                        .Include(gs => gs.IdKillerNavigation)
                        .ThenInclude(gs => gs.IdKillerNavigation)
                        .Include(gs => gs.IdMapNavigation)
                        .ThenInclude(Map => Map.IdMeasurementNavigation)
                        .Include(WPM => WPM.IdWhoPlacedMapNavigation)
                        .Include(WPM => WPM.IdWhoPlacedMapWinNavigation)
                        .Where(gs => gs.IdKillerNavigation.IdAssociation == 1)
                        .Where(gs => gs.IdMapNavigation.IdMap == map.IdMap)
                        .ToList();

                    double PickRate = Math.Round((double)GameStat.Count / CountMatch * 100, 2);

                    int CountKills = 0;
                    foreach (var item in GameStat)
                    {
                        CountKills += item.CountKills;
                    }
                    double KillRate = Math.Round((double)CountKills / GameStat.Count / 4 * 100, 2);

                    int EscapeSurvivor = GameStat.Count * 4 - CountKills;
                    double Escape = Math.Round((double)EscapeSurvivor / (GameStat.Count * 4) * 100, 2);

                    double Win = Math.Round((double)GameStat.Where(x => x.CountKills == 3 | x.CountKills == 4).Count() / GameStat.Count * 100, 2);

                    int MapRandom = GameStat.Where(gs => gs.IdWhoPlacedMapWin == idRandomMap.IdWhoPlacedMap).Count();
                    int MapOffering = GameStat.Where(gs => gs.IdWhoPlacedMapWin == idOfferingMap.IdWhoPlacedMap).Count();

                    double MapRandomPercent = Math.Round((double)MapRandom / GameStat.Count * 100, 2);
                    double MapOfferingPercent = Math.Round((double)MapOffering / GameStat.Count * 100, 2);

                    var mapStat = new MapStat()
                    {
                        idMap = map.IdMap,
                        MapName = map.MapName,
                        MapMeasurement = map.IdMeasurementNavigation.MeasurementName,
                        idMapMeasurement = map.IdMeasurementNavigation.IdMeasurement,
                        MapImage = map.MapImage,
                        CountGame = GameStat.Count,
                        PickRateMap = PickRate,
                        KillRateMap = KillRate,
                        EscapeRateMap = Escape,
                        WinRateMap = Win,
                        FalloutMapRandom = MapRandom,
                        FalloutMapRandomPercent = MapRandomPercent,
                        FalloutMapOffering = MapOffering,
                        FalloutMapOfferingPercent = MapOfferingPercent,
                    };
                    MapStatList.Add(mapStat);
                }
            }
        }

        private void GetPersonalSurvivorMapStat()
        {
            MapStatList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                List<Map> Maps = context.Maps.Include(mapMeasurement => mapMeasurement.IdMeasurementNavigation).ToList();

                int CountMatch = context.GameStatistics
                    .Include(gs => gs.IdKillerNavigation)
                    .ThenInclude(killerInfo => killerInfo.IdAssociationNavigation)
                    .Where(gs => gs.IdKillerNavigation.IdAssociation == 3)
                    .Count();

                var idRandomMap = context.WhoPlacedMaps.FirstOrDefault(WPM => WPM.WhoPlacedMapName == "Рандом");
                var idOfferingMap = context.WhoPlacedMaps.FirstOrDefault(WPM => WPM.WhoPlacedMapName == "Выживший" | WPM.WhoPlacedMapName == "Я");

                foreach (var map in Maps)
                {
                    List<GameStatistic> GameStat = context.GameStatistics
                        .Include(gs => gs.IdMapNavigation)
                        .Include(gs => gs.IdKillerNavigation)
                        .ThenInclude(gs => gs.IdKillerNavigation)
                        .Include(gs => gs.IdMapNavigation)
                        .ThenInclude(Map => Map.IdMeasurementNavigation)
                        .Include(WPM => WPM.IdWhoPlacedMapNavigation)
                        .Include(WPM => WPM.IdWhoPlacedMapWinNavigation)
                        .Where(gs => gs.IdKillerNavigation.IdAssociation == 3)
                        .Where(gs => gs.IdMapNavigation.IdMap == map.IdMap)
                        .ToList();

                    double PickRate = Math.Round((double)GameStat.Count / CountMatch * 100, 2);

                    int CountKills = 0;
                    foreach (var item in GameStat)
                    {
                        CountKills += item.CountKills;
                    }
                    double KillRate = Math.Round((double)CountKills / GameStat.Count / 4 * 100, 2);

                    int EscapeSurvivor = GameStat.Count * 4 - CountKills;
                    double Escape = Math.Round((double)EscapeSurvivor / (GameStat.Count * 4) * 100, 2);

                    double Win = Math.Round((double)GameStat.Where(x => x.CountKills == 3 | x.CountKills == 4).Count() / GameStat.Count * 100, 2);

                    int MapRandom = GameStat.Where(gs => gs.IdWhoPlacedMapWin == idRandomMap.IdWhoPlacedMap).Count();
                    int MapOffering = GameStat.Where(gs => gs.IdWhoPlacedMapWin == idOfferingMap.IdWhoPlacedMap).Count();

                    double MapRandomPercent = Math.Round((double)MapRandom / GameStat.Count * 100, 2);
                    double MapOfferingPercent = Math.Round((double)MapOffering / GameStat.Count * 100, 2);

                    var mapStat = new MapStat()
                    {
                        idMap = map.IdMap,
                        MapName = map.MapName,
                        MapMeasurement = map.IdMeasurementNavigation.MeasurementName,
                        idMapMeasurement = map.IdMeasurementNavigation.IdMeasurement,
                        MapImage = map.MapImage,
                        CountGame = GameStat.Count,
                        PickRateMap = PickRate,
                        KillRateMap = KillRate,
                        EscapeRateMap = Escape,
                        WinRateMap = Win,
                        FalloutMapRandom = MapRandom,
                        FalloutMapRandomPercent = MapRandomPercent,
                        FalloutMapOffering = MapOffering,
                        FalloutMapOfferingPercent = MapOfferingPercent,
                    };
                    MapStatList.Add(mapStat);
                }
            }
        }

        #endregion

        #region Методы сортировки

        private void SortMapStatList()
        {
            GetMapStatisticData();

            switch (SelectedMapStatSortItem)
            {
                case "Измерению (Убыв.)":
                    SortMapStatsByMeasurementByDescendingOrder();
                    break;

                case "Измерению (Возр.)":
                    SortMapStatsByMeasurementByAscendingOrder();
                    break;

                case "Дате выхода (Убыв.)":
                    SortMapStatsByDescendingOrder();
                    break;

                case "Дате выхода (Возр.)":
                    SortMapStatsByAscendingOrder();
                    break;

                case "Алфавит (Я-А)":
                    SortMapStatsByMapNameDescendingOrder();
                    break;

                case "Алфавит (А-Я)":
                    SortMapStatsByMapNameAscendingOrder();
                    break;

                case "Пикрейт (Убыв.)":
                    SortMapStatsByPickRateDescendingOrder();
                    break;

                case "Пикрейт (Возр.)":
                    SortMapStatsByPickRateAscendingOrder();
                    break;

                case "Винрейт (Убыв.)":
                    SortMapStatsByWinRateDescendingOrder();
                    break;

                case "Винрейт (Возр.)":
                    SortMapStatsByWinRateAscendingOrder();
                    break;

                case "Киллрейт (Убыв.)":
                    SortMapStatsByKillRateDescendingOrder();
                    break;

                case "Киллрейт (Возр.)":
                    SortMapStatsByKillRateAscendingOrder();
                    break;

                case "Количеству сыгранных игр (Убыв.)":
                    SortMapStatsByCountGameDescendingOrder();
                    break;

                case "Количеству сыгранных игр (Возр.)":
                    SortMapStatsByCountGameAscendingOrder();
                    break;
                
                case "Количеству игр с подношениями (Убыв.)":
                    SortMapStatsByCountGameWithOfferingDescendingOrder();
                    break;

                case "Количеству игр с подношениями (Возр.)":
                    SortMapStatsByCountGameWithOfferingAscendingOrder();
                    break;

                case "Количеству игр без подношений (Убыв.)":
                    SortMapStatsByCountGameWithoutOfferingDescendingOrder();
                    break;

                case "Количеству игр без подношений (Возр.)":
                    SortMapStatsByCountGameWithoutOfferingAscendingOrder();
                    break;
            }
        }

        private void SortMapStatsByMeasurementByDescendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderByDescending(Map =>Map.idMapMeasurement))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByMeasurementByAscendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderBy(Map =>Map.idMapMeasurement))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByDescendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList)
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByAscendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.Reverse())
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByMapNameDescendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderByDescending(Map => Map.MapName))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByMapNameAscendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderBy(Map => Map.MapName))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByPickRateDescendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderByDescending(Map => Map.PickRateMap))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByPickRateAscendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderBy(Map => Map.PickRateMap))
            {
                MapStatSortedList.Add(item);
            }
        } 
        
        private void SortMapStatsByWinRateDescendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderByDescending(Map => Map.WinRateMap))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByWinRateAscendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderBy(Map => Map.WinRateMap))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByKillRateDescendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderByDescending(Map => Map.KillRateMap))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByKillRateAscendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderBy(Map => Map.KillRateMap))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByCountGameDescendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderByDescending(ks => ks.CountGame))
            {
                MapStatSortedList.Add(item);
            }
        } 
        
        private void SortMapStatsByCountGameAscendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderBy(ks => ks.CountGame))
            {
                MapStatSortedList.Add(item);
            }
        }   

        private void SortMapStatsByCountGameWithOfferingDescendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderByDescending(ks => ks.FalloutMapOffering))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByCountGameWithOfferingAscendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderBy(ks => ks.FalloutMapOffering))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByCountGameWithoutOfferingDescendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderByDescending(ks => ks.FalloutMapRandom))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByCountGameWithoutOfferingAscendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderBy(ks => ks.FalloutMapRandom))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SearchMapName()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.Where(ms => ms.MapName.ToLower().Contains(SearchTextBox.ToLower())))
            {
                MapStatSortedList.Add(item);
            }
        }  
   
        #endregion
    }
}
