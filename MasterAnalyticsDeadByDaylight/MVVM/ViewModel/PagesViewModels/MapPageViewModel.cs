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
            "Количеству выигранных игр (Убыв.)", "Количеству выигранных игр (Возр.)",
            ];

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
            MapStatList = [];
            PlayerAssociationList = [];
            MapStatSortedList = [];
            GetPlayerAssociation();
            GetMapStatisticData();
            SelectedMapStatSortItem = SortingList.First();
            SelectedTypePlayerItem = PlayerAssociationList.First();
        }

        #region Команды

        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { GetMapStatisticData(); }); }

        #endregion

        #region Методы

        private void GetMapStatisticData()
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

                foreach (var item in Maps)
                {
                    List<GameStatistic> GameStat = context.GameStatistics
                        .Include(gs => gs.IdMapNavigation)
                        .Include(gs => gs.IdKillerNavigation)
                        .ThenInclude(gs => gs.IdKillerNavigation)
                        .Include(gs => gs.IdMapNavigation)
                        .ThenInclude(Map => Map.IdMeasurementNavigation)
                        .Where(gs => gs.IdKillerNavigation.IdAssociation == 1)
                        .Where(gs => gs.IdMapNavigation.IdMap == item.IdMap)
                        .ToList();


                    int MapRandom = GameStat.Where(gs => gs.IdWhoPlacedMapWin == idRandomMap.IdWhoPlacedMap).Count();
                    int MapOffering = GameStat.Where(gs => gs.IdWhoPlacedMapWin == idOfferingMap.IdWhoPlacedMap).Count();

                    double MapRandomPercent = Math.Round((double)MapRandom / GameStat.Count , 2);
                    double MapOfferingPercent = Math.Round((double)MapOffering / GameStat.Count, 2);

                    double PickRate = Math.Round((double)GameStat.Count / CountMatch, 2);

                    var mapStat = new MapStat()
                    {
                        idMap = item.IdMap,
                        MapName = item.MapName,
                        MapMeasurement = item.IdMeasurementNavigation.MeasurementName,
                        MapImage = item.MapImage,
                        CountGame = GameStat.Count,
                        PickRateMap = PickRate,
                        FalloutMapRandom = MapRandom,
                        FalloutMapRandomPercent = MapRandomPercent,
                        FalloutMapOffering = MapOffering,
                        FalloutMapOfferingPercent = MapOfferingPercent,
                    };
                    MapStatList.Add(mapStat);
                }
            }
        }

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

        #endregion

        #region Методы сортировки

        private void SortMapStatList()
        {
            switch (SelectedMapStatSortItem)
            {
                case "Измерению (Убыв.)":
                    SortMapStatsByMeasurementByDescendingOrder();
                    break;

                case "Дате выхода (Убыв.)":
                    SortMapStatsByDescendingOrder();
                    break;
                case "Дате выхода (Возр.)":

                    break;
                case "Алфавит (Я-А)":

                    break;
                case "Алфавит (А-Я)":

                    break;
                case "Пикрейт (Убыв.)":

                    break;
                case "Пикрейт (Возр.)":

                    break;
                case "Винрейт (Убыв.)":

                    break;
                case "Винрейт (Возр.)":

                    break;
                case "Киллрейт (Убыв.)":

                    break;
                case "Киллрейт (Возр.)":

                    break;
                case "Количеству сыгранных игр (Убыв.)":
                    SortMapStatsByCountGameDescendingOrder();
                    break;
                case "Количеству сыгранных игр (Возр.)":

                    break;
                case "Количеству выигранных игр (Убыв.)":

                    break;
                case "Количеству выигранных игр (Возр.)":

                    break;
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
          private void SortMapStatsByMeasurementByDescendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderByDescending(Map =>Map.MapMeasurement))
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

        private void SearchMapName()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.Where(ms => ms.MapName.ToLower().Contains(SearchTextBox.ToLower())))
            {
                MapStatSortedList.Add(item);
            }

            if (SearchTextBox == string.Empty)
            {
                SortMapStatList();
            }
        }

       
        #endregion
    }
}
