using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation;
using MasterAnalyticsDeadByDaylight.Utils.Calculation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    class MapPageViewModel : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDataService _dataService;
        private readonly IPageNavigationService _pageNavigationService;

        public MapPageViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dataService = _serviceProvider.GetService<IDataService>();
            _pageNavigationService = _serviceProvider.GetService<IPageNavigationService>();

            GetPlayerAssociations();
            SelectedTypeMap = TypeMap.FirstOrDefault();
        }

        public void Update(object value)
        {
            if (value is Map map)
            {
                if (SelectedMap.IdMap == map.IdMap || SelectedMap.IdMap == map.IdMeasurement)
                {
                    _matches.Clear();
                    _matches.AddRange(GetMapInfo(map));
                    CalculateHeaderStats();
                    CalculateExtendedStats();
                }
            }
        }

        /*--Общие Свойства \ Коллекции--------------------------------------------------------------------*/

        #region Коллекции : Общие

        public List<string> TypeMap { get; set; } = ["Карты", "Измерение"];

        public ObservableCollection<Map> MapList { get; set; } = [];

        public List<GameStatistic> _matches { get; set; } = [];

        public ObservableCollection<PlayerAssociation> PlayerAssociations { get; set; } = [];

        public ObservableCollection<MapStat> MapStats { get; set; } = [];

        #endregion

        #region Свойство : Выбор типа карт - Карты | Измерение

        private string _selectedTypeMap;
        public string SelectedTypeMap
        {
            get => _selectedTypeMap;
            set
            {
                _selectedTypeMap = value;
                DefineAndGetMapData();
                MapStats.Clear();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойство : Выбор Карты | Индекса

        private Map _selectedMap;
        public Map SelectedMap
        {
            get => _selectedMap;
            set
            {
                _selectedMap = value;
                if (value != null)
                {
                    _matches.Clear();
                    _matches.AddRange(GetMapInfo(value));
                    CalculateHeaderStats();
                    CalculateExtendedStats();
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedMapIndex;
        public int SelectedMapIndex
        {
            get => _selectedMapIndex;
            set
            {
                if (value >= 0 && value < MapList.Count)
                {
                    _selectedMapIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Свойства : Выбор игровой ассоциации

        private PlayerAssociation _selectedPlayerAssociation;
        public PlayerAssociation SelectedPlayerAssociation
        {
            get => _selectedPlayerAssociation;
            set
            {
                if (_selectedPlayerAssociation != value)
                {
                    _selectedPlayerAssociation = value;
                    MapStats.Clear();
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Свойства : Статистика в шапке

        private int _countMatches;
        public int CountMatches
        {
            get => _countMatches;
            set
            {
                _countMatches = value;
                OnPropertyChanged();
            }
        }

        private int _pickRateCount;
        public int PickRateCount
        {
            get => _pickRateCount;
            set
            {
                _pickRateCount = value;
                OnPropertyChanged();
            }
        }

        private double _pickRate;
        public double PickRate
        {
            get => _pickRate;
            set
            {
                _pickRate = value;
                OnPropertyChanged();
            }
        }

        private int _pickRateNoOfferingCount;
        public int PickRateNoOfferingCount
        {
            get => _pickRateNoOfferingCount;
            set
            {
                _pickRateNoOfferingCount = value;
                OnPropertyChanged();
            }
        }
        
        private double _pickRateNoOffering;
        public double PickRateNoOffering
        {
            get => _pickRateNoOffering;
            set
            {
                _pickRateNoOffering = value;
                OnPropertyChanged();
            }
        }

        private int _pickRateWithOfferingCount;
        public int PickRateWithOfferingCount
        {
            get => _pickRateWithOfferingCount;
            set
            {
                _pickRateWithOfferingCount = value;
                OnPropertyChanged();
            }
        } 
        
        private double _pickRateWithOffering;
        public double PickRateWithOffering
        {
            get => _pickRateWithOffering;
            set
            {
                _pickRateWithOffering = value;
                OnPropertyChanged();
            }
        } 
        
        private int _escapeCount;
        public int EscapeCount
        {
            get => _escapeCount;
            set
            {
                _escapeCount = value;
                OnPropertyChanged();
            }
        }

        private double _escapeRate;
        public double EscapeRate
        {
            get => _escapeRate;
            set
            {
                _escapeRate = value;
                OnPropertyChanged();
            }
        }

        private double _killRate;
        public double KillRate
        {
            get => _killRate;
            set
            {
                _killRate = value;
                OnPropertyChanged();
            }
        }

        private double _killRatePercent;
        public double KillRatePercent
        {
            get => _killRatePercent;
            set
            {
                _killRatePercent = value;
                OnPropertyChanged();
            }
        }

        private int _winRateKillerCount;
        public int WinRateKillerCount
        {
            get => _winRateKillerCount;
            set
            {
                _winRateKillerCount = value;
                OnPropertyChanged();
            }
        }

        private double _winRateKiller;
        public double WinRateKiller
        {
            get => _winRateKiller;
            set
            {
                _winRateKiller = value;
                OnPropertyChanged();
            }
        }  

        #endregion 

        #region Свойство : Максимальная ширина элементов

        public int MaxWidth { get; set; } = 1200;

        #endregion

        #region Свойство : Popup - Список выживших для сравнения

        private bool _isPopupFilterOpen;
        public bool IsPopupFilterOpen
        {
            get => _isPopupFilterOpen;
            set
            {
                _isPopupFilterOpen = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        // Команды переключение индексов
        private RelayCommand _nextSurvivorCommand;
        public RelayCommand NextSurvivorCommand { get => _nextSurvivorCommand ??= new(obj => { NextMap(); }); }

        private RelayCommand _previousSurvivorCommand;
        public RelayCommand PreviousSurvivorCommand { get => _previousSurvivorCommand ??= new(obj => { PreviousMao(); }); }

        //Команды добавление киллеров в список сравнения
        private RelayCommand _addSingleToComparisonCommand;
        public RelayCommand AddSingleToComparisonCommand { get => _addSingleToComparisonCommand ??= new(obj => { AddToComparison(); }); }

        private RelayCommand _addAllToComparisonCommand;
        public RelayCommand AddAllToComparisonCommand { get => _addAllToComparisonCommand ??= new(obj => { AddAllToComparison(); }); }

        //Очистка списка статистики киллеров
        private RelayCommand _clearComparisonListCommand;
        public RelayCommand ClearComparisonListCommand { get => _clearComparisonListCommand ??= new(obj => { MapStats.Clear(); }); }

        //Команд открытие страницы сравнений
        private RelayCommand _openComparisonPageCommand;
        public RelayCommand OpenComparisonPageCommand { get => _openComparisonPageCommand ??= new(obj => { OpenComparisonPage(); }); }

        //Команда обновление данных
        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { ReloadData(); }); }

        //Открытие Popup
        private RelayCommand _openPopupListSurvivorsCommand;
        public RelayCommand OpenPopupListSurvivorsCommand { get => _openPopupListSurvivorsCommand ??= new(obj => { IsPopupFilterOpen = true; }); }

        /*--Получение первоначальных данных---------------------------------------------------------------*/

        #region Метод : Определение и получение нужных данных в список Maps "Карт" 

        private void DefineAndGetMapData()
        {
            Action action = SelectedTypeMap switch
            {
                "Карты"     => GetMaps,
                "Измерение" => GetMeasurements,
                _ => throw new Exception("Такого типа карт нету.")
            };
            action?.Invoke();
        }

        #endregion

        #region Метод : Получение списка "Карт"

        private void GetMaps()
        {
            MapList.Clear();
            foreach (var item in _dataService.GetAllDataInList<Map>(x => x.Include(x => x.IdMeasurementNavigation)))
                MapList.Add(item);

            SelectedMap = MapList.FirstOrDefault();
        }

        #endregion

        #region Метод : Получение списка "Измерений"

        private void GetMeasurements()
        {
            MapList.Clear();

            var maps = _dataService.GetAllDataInList<Map>();

            foreach (var item in _dataService.GetAllDataInList<Measurement>())
            {
                var newMapInMeasurement = new Map()
                {
                    IdMap = item.IdMeasurement,
                    IdMeasurement = item.IdMeasurement,
                    MapName = item.MeasurementName,
                    MapImage = maps.FirstOrDefault(x => x.IdMeasurement == item.IdMeasurement).MapImage,
                };
                MapList.Add(newMapInMeasurement);
            }

            SelectedMap = MapList.FirstOrDefault();
        }

        #endregion

        #region Метод : Получение списка "Игровой ассоциации"

        private void GetPlayerAssociations()
        {
            foreach (var item in _dataService.GetAllDataInList<PlayerAssociation>(x => x.Where(x => x.IdPlayerAssociation == 1 || x.IdPlayerAssociation == 3)))
            {
                PlayerAssociations.Add(item);
            }
            SelectedPlayerAssociation = PlayerAssociations.FirstOrDefault();
        }

        #endregion

        #region Метод : Получение матчей на выбранной карте | измерение

        private List<GameStatistic> GetMapInfo(Map map)
        {
            if (SelectedTypeMap == "Карты")
            {
                return _dataService.GetAllDataInList<GameStatistic>(x => 
                        x.Include(x => x.IdMapNavigation.IdMeasurementNavigation)
                            .Include(x => x.IdKillerNavigation)
                                .Where(x => x.IdMapNavigation.IdMap == map.IdMap && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociation.IdPlayerAssociation));
            }
            if (SelectedTypeMap == "Измерение")
            {
                return _dataService.GetAllDataInList<GameStatistic>(x => 
                        x.Include(x => x.IdMapNavigation.IdMeasurementNavigation)
                            .Include(x => x.IdKillerNavigation)
                                .Where(x => x.IdMapNavigation.IdMeasurementNavigation.IdMeasurement == map.IdMap && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociation.IdPlayerAssociation));
            }

            return null;
        }

        private List<GameStatistic> GetMeasurementInfo(Measurement measurement)
        {
            return _dataService.GetAllDataInList<GameStatistic>(x =>
                        x.Include(x => x.IdMapNavigation.IdMeasurementNavigation)
                            .Include(x => x.IdKillerNavigation)
                                .Where(x => x.IdMapNavigation.IdMeasurementNavigation.IdMeasurement == measurement.IdMeasurement && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociation.IdPlayerAssociation));
        }

        #endregion

        #region Метод : Обновление данных

        private void ReloadData()
        {
            _matches.Clear();
            _matches.AddRange(GetMapInfo(SelectedMap));
            CalculateHeaderStats();
            CalculateExtendedStats();
        }

        #endregion 

        /*--Взаимодействие с списком----------------------------------------------------------------------*/

        #region Методы : Переключение элементов списка выживщих (По индексу)

        private void PreviousMao()
        {
            SelectedMapIndex--;
        }

        private void NextMap()
        {
            SelectedMapIndex++;
        }

        #endregion

        /*--Расчеты---------------------------------------------------------------------------------------*/

        #region Метод : Открытие страницы сравнений

        private void OpenComparisonPage()
        {
            _pageNavigationService.NavigateTo("ComparisonPage", MapStats);
        }

        #endregion

        #region Метод : Основная статистика

        private async void CalculateHeaderStats()
        {
            CountMatches = _matches.Count;

            EscapeCount = await CalculationMap.EscapeSurvivorAsync(CountMatches,(int)await CalculationKiller.CountKillAsync(_matches));
            EscapeRate = await CalculationMap.EscapeRateAsync(EscapeCount, CountMatches);

            PickRate = await CalculationMap.PickRateAsync(CountMatches, _dataService.Count<GameStatistic>(x => x.Where(x => x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociation.IdPlayerAssociation)));
            
            PickRateNoOfferingCount = await CalculationMap.RandomMapAsync(_matches);
            PickRateNoOffering = await CalculationMap.PickRateAsync(PickRateNoOfferingCount, CountMatches);

            PickRateWithOfferingCount = await CalculationMap.WithOfferingsAsync(_matches);
            PickRateWithOffering = await CalculationMap.PickRateAsync(PickRateWithOfferingCount, CountMatches);

            KillRatePercent = await CalculationKiller.AVGKillRatePercentageAsync(_matches);
            KillRate = await CalculationKiller.AVGKillRateAsync(_matches);

            WinRateKillerCount = await CalculationKiller.CountMatchWinAsync(_matches);
            WinRateKiller = await CalculationKiller.WinRateAsync(WinRateKillerCount, CountMatches);
        }

        #endregion

        #region Методы : Расширение статистика

        private void CalculateExtendedStats()
        {
           
        }

        #endregion

        #region Методы : Создание SurvivorStat - добавлени его в список сравнения

        private void AddToComparison()
        {
            if (MapStats.Contains(MapStats.FirstOrDefault(x => x.idMap == SelectedMap.IdMap)))
                return;

            var mapStat = new MapStat
            {
                idMap = SelectedMap.IdMap,
                MapName = SelectedMap.MapName,
                MapImage = SelectedMap.MapImage,
                CountGame = CountMatches,
                PickRateMap = PickRate,
                FalloutMapRandom = PickRateNoOfferingCount,
                FalloutMapRandomPercent = PickRateNoOffering,
                FalloutMapOffering = PickRateWithOfferingCount,
                FalloutMapOfferingPercent = PickRateWithOffering,
                KillRateMap = KillRate,
                KillRateMapPercent = KillRatePercent,
                WinRateMap = WinRateKillerCount,
                WinRateMapPercent = WinRateKiller,
                EscapeRateMap = EscapeRate,
            };

            MapStats.Add(mapStat);
        }

        private async void AddAllToComparison()
        {
            if (SelectedTypeMap == "Карты")
            {
                foreach (var map in _dataService.GetAllDataInList<Map>())
                {
                    if (MapStats.Contains(MapStats.FirstOrDefault(x => x.idMap == map.IdMap)))
                        continue;

                    var matches = GetMapInfo(map);

                    var mapStat = new MapStat
                    {
                        idMap = map.IdMap,
                        MapName = map.MapName,
                        MapImage = map.MapImage,
                        CountGame = matches.Count,
                        PickRateMap = await CalculationMap.PickRateAsync(CountMatches, _dataService.Count<GameStatistic>(x => x.Where(x => x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociation.IdPlayerAssociation))),
                        FalloutMapRandom = await CalculationMap.RandomMapAsync(matches),
                        FalloutMapRandomPercent = await CalculationMap.PickRateAsync(await CalculationMap.RandomMapAsync(matches), matches.Count),
                        FalloutMapOffering = await CalculationMap.WithOfferingsAsync(matches),
                        FalloutMapOfferingPercent = await CalculationMap.PickRateAsync(await CalculationMap.WithOfferingsAsync(matches), matches.Count),
                        KillRateMap = await CalculationKiller.AVGKillRateAsync(matches),
                        KillRateMapPercent = await CalculationKiller.AVGKillRatePercentageAsync(matches),
                        WinRateMap = await CalculationKiller.CountMatchWinAsync(matches),
                        WinRateMapPercent = await CalculationKiller.WinRateAsync(await CalculationKiller.CountMatchWinAsync(matches), matches.Count),
                        EscapeRateMap = await CalculationMap.EscapeRateAsync(await CalculationMap.EscapeSurvivorAsync(matches.Count, (int)await CalculationKiller.CountKillAsync(matches)), matches.Count),
                    };

                    MapStats.Add(mapStat);
                }
            }
            if (SelectedTypeMap == "Измерение")
            {
                foreach (var measurement in _dataService.GetAllDataInList<Measurement>())
                {
                    if (MapStats.Contains(MapStats.FirstOrDefault(x => x.idMap == measurement.IdMeasurement)))
                        continue;

                    var matches = GetMeasurementInfo(measurement);

                    var mapStat = new MapStat
                    {
                        idMap = measurement.IdMeasurement,
                        MapName = measurement.MeasurementName,
                        MapImage = MapList.FirstOrDefault(x => x.IdMap == measurement.IdMeasurement).MapImage,
                        CountGame = matches.Count,
                        PickRateMap = await CalculationMap.PickRateAsync(CountMatches, _dataService.Count<GameStatistic>(x => x.Where(x => x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociation.IdPlayerAssociation))),
                        FalloutMapRandom = await CalculationMap.RandomMapAsync(matches),
                        FalloutMapRandomPercent = await CalculationMap.PickRateAsync(await CalculationMap.RandomMapAsync(matches), matches.Count),
                        FalloutMapOffering = await CalculationMap.WithOfferingsAsync(matches),
                        FalloutMapOfferingPercent = await CalculationMap.PickRateAsync(await CalculationMap.WithOfferingsAsync(matches), matches.Count),
                        KillRateMap = await CalculationKiller.AVGKillRateAsync(matches),
                        KillRateMapPercent = await CalculationKiller.AVGKillRatePercentageAsync(matches),
                        WinRateMap = await CalculationKiller.CountMatchWinAsync(matches),
                        WinRateMapPercent = await CalculationKiller.WinRateAsync(await CalculationKiller.CountMatchWinAsync(matches), matches.Count),
                        EscapeRateMap = await CalculationMap.EscapeRateAsync(await CalculationMap.EscapeSurvivorAsync(matches.Count, (int)await CalculationKiller.CountKillAsync(matches)), matches.Count),
                    };

                    MapStats.Add(mapStat);
                }
            }
        }

        #endregion
    }
}