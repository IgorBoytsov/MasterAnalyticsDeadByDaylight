using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.CalculationService.KillerService;
using MasterAnalyticsDeadByDaylight.Services.CalculationService.MapService;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    class MapPageViewModel : BaseViewModel
    {

        #region Колекции 

        public ObservableCollection<MapStat> MapStatList { get; set; } = [];

        public ObservableCollection<MapStat> MapStatSortedList { get; set; } = [];

        public ObservableCollection<PlayerAssociation> PlayerAssociationList { get; set; } = [];

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
            "Количеству игр без подношений (Убыв.)", "Количеству игр без подношений (Возр.)"
            ];

        public ObservableCollection<string> TypeMapList { get; set; } = ["Карты", "Измерение"];

        public ObservableCollection<string> TypeStatsList { get; set; } = [ "Общая", "Личная за Убийц", "Личная за Выживших"];

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

        private string _selectedTypeMap;
        public string SelectedTypeMap
        {
            get => _selectedTypeMap;
            set
            {
                _selectedTypeMap = value;
                OnPropertyChanged();
            }
        }

        private string _selectedTypeStat;
        public string SelectedTypeStat
        {
            get => _selectedTypeStat;
            set
            {
                _selectedTypeStat = value;
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

        #region Pupup

        private bool _isFilterPopupOpen;
        public bool IsFilterPopupOpen
        {
            get => _isFilterPopupOpen;
            set
            {
                _isFilterPopupOpen = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private readonly IDataService _dataService;
        private readonly IMapCalculationService _mapCalculationService;
        private readonly IKillerCalculationService _killerCalculationService;

        public MapPageViewModel(IDataService dataService, IMapCalculationService mapCalculationService, IKillerCalculationService killerCalculationService)
        {
            _dataService = dataService;
            _mapCalculationService = mapCalculationService;
            _killerCalculationService = killerCalculationService;
            IsFilterPopupOpen = false;

            SelectedMapStatSortItem = SortingList[1];
            SelectedTypeMap = TypeMapList.First();
            SelectedTypeStat = TypeStatsList[1];

            GetMapInfo(SelectedTypeMap);
        }

        #region Команды

        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { GetMapInfo(SelectedTypeMap); }); }

        private RelayCommand _openFilterCommand;
        public RelayCommand OpenFilterCommand { get => _openFilterCommand ??= new(obj => { IsFilterPopupOpen = true; }); }

        private RelayCommand _applyCommand;
        public RelayCommand ApplyCommand { get => _applyCommand ??= new(obj => 
        {
            GetMapInfo(SelectedTypeMap);
            IsFilterPopupOpen = false;
        }); }

        #endregion

        #region Методы

        private void GetMapInfo(string typeMapName)
        {
            Action action = typeMapName switch
            {
                "Карты" => GetMapStat,
                "Измерение" => GetMeasurementStat,
                _ => throw new Exception("Такого типа статистики нету!")
            };
            action.Invoke();
        }

        private async void GetMapStat()
        {
            MapStatList.Clear();

            int CountMatch = SelectedTypeStat switch
            {
                "Общая" => _dataService.Count<GameStatistic>(),
                "Личная за Убийц" => _dataService.Count<GameStatistic>(x => x.Include(gs => gs.IdKillerNavigation).ThenInclude(killerInfo => killerInfo.IdAssociationNavigation).Where(gs => gs.IdKillerNavigation.IdAssociation == 1)),
                "Личная за Выживших" => _dataService.Count<GameStatistic>(x => x.Include(gs => gs.IdKillerNavigation).ThenInclude(killerInfo => killerInfo.IdAssociationNavigation).Where(gs => gs.IdKillerNavigation.IdAssociation == 3)),
                _ => throw new Exception("Такого типа статистик нету")
            };

            List<Map> Maps = await _dataService.GetAllDataInListAsync<Map>(x => x.Include(x => x.IdMeasurementNavigation));

            foreach (var item in Maps)
            {
                List<GameStatistic> GameStat = await _mapCalculationService.GetMatchForMapAsync(item.IdMap, SelectedTypeStat);

                double PickRate = await _mapCalculationService.CalculatingPickRate(GameStat.Count, CountMatch);
                int CountKills = (int)await _killerCalculationService.CalculatingCountKill(GameStat);
                //double KillRatePercentage = await _killerCalculationService.CalculatingKillRatePercentage(GameStat.Count, CountKills);
                double AVGKillRate = await _killerCalculationService.CalculatingAVGKillRate(GameStat, CountKills);
                double KillRatePercentage = await _killerCalculationService.CalculatingAVGKillRatePercentage(AVGKillRate);
                int EscapeSurvivor = await _mapCalculationService.CalculatingEscapeSurvivor(GameStat.Count, CountKills);
                double EscapeRate = await _mapCalculationService.CalculatingEscapeRate(EscapeSurvivor, GameStat.Count);
                int WinMatch = GameStat.Where(x => x.CountKills == 3 | x.CountKills == 4).Count();
                double WinRatePercent = await _mapCalculationService.CalculatingWinRateRate(WinMatch, GameStat.Count);
                int MapRandom = await _mapCalculationService.CalculatingRandomMap(GameStat);
                int MapOffering = await _mapCalculationService.CalculatingOfferingMap(GameStat);
                double MapRandomPercent = await _mapCalculationService.CalculatingDropMapPercent(MapRandom, GameStat.Count);
                double MapOfferingPercent = await _mapCalculationService.CalculatingDropMapPercent(MapOffering, GameStat.Count);

                var mapStat = new MapStat()
                {
                    idMap = item.IdMap,
                    MapName = item.MapName,
                    MapMeasurement = item.IdMeasurementNavigation.MeasurementName,
                    idMapMeasurement = item.IdMeasurementNavigation.IdMeasurement,
                    MapImage = item.MapImage,
                    CountGame = GameStat.Count,
                    PickRateMap = PickRate,
                    KillRateMap = AVGKillRate,
                    KillRateMapPercent = KillRatePercentage,
                    EscapeRateMap = EscapeRate,
                    WinRateMap = WinMatch,
                    WinRateMapPercent = WinRatePercent,
                    FalloutMapRandom = MapRandom,
                    FalloutMapRandomPercent = MapRandomPercent,
                    FalloutMapOffering = MapOffering,
                    FalloutMapOfferingPercent = MapOfferingPercent,
                };
                MapStatList.Add(mapStat);
            }
            SortMapStatList();
        }

        private async void GetMeasurementStat()
        {
            MapStatList.Clear();

            int CountMatch = SelectedTypeStat switch
            {
                "Общая" => _dataService.Count<GameStatistic>(),
                "Личная за Убийц" => _dataService.Count<GameStatistic>(x => x.Include(gs => gs.IdKillerNavigation).ThenInclude(killerInfo => killerInfo.IdAssociationNavigation).Where(gs => gs.IdKillerNavigation.IdAssociation == 1)),
                "Личная за Выживших" => _dataService.Count<GameStatistic>(x => x.Include(gs => gs.IdKillerNavigation).ThenInclude(killerInfo => killerInfo.IdAssociationNavigation).Where(gs => gs.IdKillerNavigation.IdAssociation == 3)),
                _ => throw new Exception("Такого типа статистик нету")
            };

            List<Measurement> Measurements = await _dataService.GetAllDataInListAsync<Measurement>();
            List<Map> Maps = await _dataService.GetAllDataInListAsync<Map>(x => x.Include(x => x.IdMeasurementNavigation));

            foreach (var item in Measurements)
            {
                List<GameStatistic> GameStat = await _mapCalculationService.GetMatchForMeasurementAsync(item.IdMeasurement, SelectedTypeStat);

                double PickRate = await _mapCalculationService.CalculatingPickRate(GameStat.Count, CountMatch);
                int CountKills = (int)await _killerCalculationService.CalculatingCountKill(GameStat);
                //double KillRatePercentage = await _killerCalculationService.CalculatingKillRatePercentage(GameStat.Count, CountKills);
                double AVGKillRate = await _killerCalculationService.CalculatingAVGKillRate(GameStat, CountKills);
                double KillRatePercentage = await _killerCalculationService.CalculatingAVGKillRatePercentage(AVGKillRate);
                int EscapeSurvivor = await _mapCalculationService.CalculatingEscapeSurvivor(GameStat.Count, CountKills);
                double EscapeRate = await _mapCalculationService.CalculatingEscapeRate(EscapeSurvivor, GameStat.Count);
                int WinMatch = GameStat.Where(x => x.CountKills == 3 | x.CountKills == 4).Count();
                double WinRatePercent = await _mapCalculationService.CalculatingWinRateRate(WinMatch, GameStat.Count);
                int MapRandom = await _mapCalculationService.CalculatingRandomMap(GameStat);
                int MapOffering = await _mapCalculationService.CalculatingOfferingMap(GameStat);
                double MapRandomPercent = await _mapCalculationService.CalculatingDropMapPercent(MapRandom, GameStat.Count);
                double MapOfferingPercent = await _mapCalculationService.CalculatingDropMapPercent(MapOffering, GameStat.Count);

                var mapStat = new MapStat()
                {
                    MapName = item.MeasurementName,
                    idMapMeasurement = item.IdMeasurement,
                    MapImage = Maps.FirstOrDefault(x => x.IdMeasurement == item.IdMeasurement).MapImage,
                    CountGame = GameStat.Count,
                    PickRateMap = PickRate,
                    KillRateMap = AVGKillRate,
                    KillRateMapPercent = KillRatePercentage,
                    EscapeRateMap = EscapeRate,
                    WinRateMap = WinMatch,
                    WinRateMapPercent = WinRatePercent,
                    FalloutMapRandom = MapRandom,
                    FalloutMapRandomPercent = MapRandomPercent,
                    FalloutMapOffering = MapOffering,
                    FalloutMapOfferingPercent = MapOfferingPercent,
                };
                MapStatList.Add(mapStat);
            }
            SortMapStatList();
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
                case null:
                    SortMapStatsByMeasurementByAscendingOrder();
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
            foreach (var item in MapStatList.OrderByDescending(Map => Map.WinRateMapPercent))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByWinRateAscendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderBy(Map => Map.WinRateMapPercent))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByKillRateDescendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderByDescending(Map => Map.KillRateMapPercent))
            {
                MapStatSortedList.Add(item);
            }
        }

        private void SortMapStatsByKillRateAscendingOrder()
        {
            MapStatSortedList.Clear();
            foreach (var item in MapStatList.OrderBy(Map => Map.KillRateMapPercent))
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