using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Enums;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.AssociationCase;
using DBDAnalytics.Application.UseCases.Abstraction.MapCase;
using DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase;
using DBDAnalytics.Application.UseCases.Abstraction.StatisticCase;
using DBDAnalytics.Domain.Enums;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.Models;
using System.Collections.ObjectModel;

namespace DBDAnalytics.WPF.ViewModels.PageVM
{
    internal class MapDetailsVM : BaseVM, IUpdatable
    {

        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IPageNavigationService _pageNavigationService;

        private readonly IGetDetailsMatchUseCase _getDetailsMatchUseCase;

        private readonly IGetMapUseCase _getMapUseCase;
        private readonly IGetMeasurementUseCase _getMeasurementUseCase;
        private readonly IGetAssociationUseCase _getAssociationUseCase;
        private readonly IWhoPlacedMapService _whoPlacedMapService;

        private readonly ICalculationKillerService _calculationKillerService;
        private readonly ICalculationSurvivorService _calculationSurvivorService;
        private readonly ICalculationTimeService _calculationTimeService;
        private readonly ICalculationGeneralService _calculationGeneralService;
        private readonly ICalculationMapService _calculationMapService;

        public MapDetailsVM(IWindowNavigationService windowNavigationService,
                            IPageNavigationService pageNavigationService,
                            IGetDetailsMatchUseCase getDetailsMatchUseCase,
                            IGetMapUseCase getMapUseCase,
                            IGetMeasurementUseCase getMeasurementUseCase,
                            IGetAssociationUseCase getAssociationUseCase,
                            IWhoPlacedMapService placedMapService,
                            ICalculationKillerService calculationKillerService,
                            ICalculationSurvivorService calculationSurvivorService,
                            ICalculationTimeService calculationTimeService,
                            ICalculationMapService calculationMapService,
                            ICalculationGeneralService calculationGeneralService)
        {
            _windowNavigationService = windowNavigationService;
            _pageNavigationService = pageNavigationService;
            _getDetailsMatchUseCase = getDetailsMatchUseCase;
            _getMapUseCase = getMapUseCase;
            _getMeasurementUseCase = getMeasurementUseCase;
            _getAssociationUseCase = getAssociationUseCase;
            _whoPlacedMapService = placedMapService;

            _calculationKillerService = calculationKillerService;
            _calculationSurvivorService = calculationSurvivorService;
            _calculationTimeService = calculationTimeService;
            _calculationGeneralService = calculationGeneralService;
            _calculationMapService = calculationMapService;

            IsCalculationSelectedTimePeriod = false;
            IsCalculationSelectedAssociation = false;
            IsCalculationSelectedAssociation = false;

            TextStatistic = new TextStatistic();

            GetAssociations();
            SelectedAssociation = PlayerAssociations.FirstOrDefault();

            GetMaps();
            GetMeasurement();

            SelectedTimePeriod = PeriodTimes[2];
            SelectedTypeMap = TypeMaps.FirstOrDefault();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {

        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Константы

        const string Map = "Карты";
        const string Measurement = "Измерение";

        #endregion

        /*--Поля--*/

        #region Поля : Разрешение на вызов методов в Selected свойствах

        private bool IsCalculationSelectedTimePeriod;
        private bool IsCalculationSelectedAssociation;

        #endregion

        /*--Коллекции--*/

        #region Коллекция : Список матчей

        private List<DetailsMatchDTO> _transmittedMatches { get; set; } = [];

        private List<DetailsMatchDTO> _mapDetails { get; set; } = [];

        #endregion

        #region Коллекция : Список Карт | Измерений

        private List<MapDTO> _maps = [];
        private List<MeasurementDTO> _measurements = [];

        public ObservableCollection<KeyValuePair<int, string>> TypeMaps { get; set; } = new()
        {
            new KeyValuePair<int, string>(0, Map),
            new KeyValuePair<int, string>(0, Measurement),
        };

        public ObservableCollection<object> Maps { get; set; } = [];

        #endregion

        #region Колекции : Параметры для сортировок

        public ObservableCollection<KeyValuePair<TimePeriod, string>> PeriodTimes { get; set; } = new()
        {
            new KeyValuePair<TimePeriod, string>(TimePeriod.Day,    "По дням"),
            new KeyValuePair<TimePeriod, string>(TimePeriod.Week,   "По неделям"),
            new KeyValuePair<TimePeriod, string>(TimePeriod.Month,  "По месяцам"),
            new KeyValuePair<TimePeriod, string>(TimePeriod.Year,   "По годам"),
        };

        public ObservableCollection<KeyValuePair<int, string>> PlayerAssociations { get; set; } = [];

        #endregion

        #region Колекции : Расчеты

        public ObservableCollection<LabeledValue> DetailsWhoPlaceMap { get; set; } = [];

        public ObservableCollection<LabeledValue> DetailsWhoPlaceMapWin { get; set; } = [];

        public ObservableCollection<LabeledValue> DetailsRecentGenerators { get; set; } = [];

        public ObservableCollection<LabeledValue> DetailsKills { get; set; } = [];

        public ObservableCollection<LabeledValue> DetailsHooks { get; set; } = [];

        #endregion

        /*--Свойства--*/

        #region Свойства : Выбор параметров

        private KeyValuePair<int, string> _selectedTypeMap;
        public KeyValuePair<int, string> SelectedTypeMap
        {
            get => _selectedTypeMap;
            set
            {
                _selectedTypeMap = value;
                LoadTypeMaps(value);
                OnPropertyChanged();
            }
        }

        private object _selectedMap;
        public object SelectedMap
        {
            get => _selectedMap;
            set
            {
                _selectedMap = value;

                GetDetailsMatches(value);

                CalculateHeaderStatistics();
                CalculateTextStatistics();
                CalculateDetailsStatistics();

                OnPropertyChanged();

                IsCalculationSelectedTimePeriod = true;
                IsCalculationSelectedAssociation = true;
            }
        }

        private KeyValuePair<TimePeriod, string> _selectedTimePeriod;
        public KeyValuePair<TimePeriod, string> SelectedTimePeriod
        {
            get => _selectedTimePeriod;
            set
            {
                _selectedTimePeriod = value;

                if (IsCalculationSelectedTimePeriod)
                {
                    
                }

                OnPropertyChanged();
            }
        }

        private KeyValuePair<int, string> _selectedAssociation;
        public KeyValuePair<int, string> SelectedAssociation
        {
            get => _selectedAssociation;
            set
            {
                _selectedAssociation = value;


                if (IsCalculationSelectedAssociation)
                {
                    GetDetailsMatches(SelectedMap);

                    CalculateHeaderStatistics();
                    CalculateTextStatistics();
                    CalculateDetailsStatistics();
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Статистика в шапке

        private int _totalCountMatch;

        private byte[] _selectedMapImage;
        public byte[] SelectedMapImage
        {
            get => _selectedMapImage;
            set
            {
                _selectedMapImage = value;
                OnPropertyChanged();
            }
        }

        private int _countMatch;
        public int CountMatch
        {
            get => _countMatch;
            set
            {
                _countMatch = value;
                OnPropertyChanged();
            }
        }

        private int _countWithOffering;
        public int CountWithOffering
        {
            get => _countWithOffering;
            set
            {
                _countWithOffering = value;
                OnPropertyChanged();
            }
        }

        private double _percentWithOffering;
        public double PercentWithOffering
        {
            get => _percentWithOffering;
            set
            {
                _percentWithOffering = value;
                OnPropertyChanged();
            }
        }
        
        private int _countNoOffering;
        public int CountNoOffering
        {
            get => _countNoOffering;
            set
            {
                _countNoOffering = value;
                OnPropertyChanged();
            }
        }

        private double _percentNoOffering;
        public double PercentNoOffering
        {
            get => _percentNoOffering;
            set
            {
                _percentNoOffering = value;
                OnPropertyChanged();
            }
        }

        private int _countMatchWin;
        public int CountMatchWin
        {
            get => _countMatchWin;
            set
            {
                _countMatchWin = value;
                OnPropertyChanged();
            }
        }

        private int _countMatchDefeat;
        public int CountMatchDefeat
        {
            get => _countMatchDefeat;
            set
            {
                _countMatchDefeat = value;
                OnPropertyChanged();
            }
        }

        private int _countMatchDraw;
        public int CountMatchDraw
        {
            get => _countMatchDraw;
            set
            {
                _countMatchDraw = value;
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

        private double _recentGenerators;
        public double RecentGenerators
        {
            get => _recentGenerators;
            set
            {
                _recentGenerators = value;
                OnPropertyChanged();
            }
        }

        private double _hookRate;
        public double HookRate
        {
            get => _hookRate;
            set
            {
                _hookRate = value;
                OnPropertyChanged();
            }
        }

        private double _winRate;
        public double WinRate
        {
            get => _winRate;
            set
            {
                _winRate = value;
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
        
        private double _escapeRatePersonal;
        public double EscapeRatePersonal
        {
            get => _escapeRatePersonal;
            set
            {
                _escapeRatePersonal = value;
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

        #endregion

        #region Свойства : Индексы карт

        private int _selectedMapIndex;
        public int SelectedMapIndex
        {
            get => _selectedMapIndex;
            set
            {
                if (value >= 0 && value < Maps.Count)
                {
                    _selectedMapIndex = value;
                    SelectedMap = Maps[value];
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Свойство : Статистика в виде списка

        private TextStatistic _textStatistic;
        public TextStatistic TextStatistic
        {
            get => _textStatistic;
            set
            {
                _textStatistic = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Переключение индиексов в списке карт

        private RelayCommand _nextMapCommand;
        public RelayCommand NextMapCommand
        {
            get => _nextMapCommand ??= new(obj =>
            {
                SelectedMapIndex++;
            });
        }

        private RelayCommand _previousMapCommand;
        public RelayCommand PreviousMapCommand
        {
            get => _previousMapCommand ??= new(obj =>
            {
                SelectedMapIndex--;
            });
        }

        #endregion

        #region Действия в шапке страницы

        private RelayCommand _updateCalculationCommand;
        public RelayCommand UpdateCalculationCommand
        {
            get => _updateCalculationCommand ??= new(obj =>
            {
                GetDetailsMatches(SelectedMap);

                CalculateHeaderStatistics();
                CalculateTextStatistics();
                CalculateDetailsStatistics();
            });
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Получение списка матчей

        private void GetDetailsMatches(object value)
        {
            _mapDetails.Clear();

            if (value is MapDTO selectedMap)
            {
                var (KillerDetailsMatch, TotalMatches) =
                    Task.Run(() => _getDetailsMatchUseCase.GetDetailsMatch(selectedMap.IdMap, (Associations)SelectedAssociation.Key, FilterParameter.Map)).Result;
                
                _totalCountMatch = TotalMatches;
                _mapDetails.AddRange(KillerDetailsMatch);
                SelectedMapImage = selectedMap.MapImage;
            }

            if (value is MeasurementDTO selectedMeasurement)
            {
                var (KillerDetailsMatch, TotalMatches) =
                    Task.Run(() => _getDetailsMatchUseCase.GetDetailsMatch(selectedMeasurement.IdMeasurement, (Associations)SelectedAssociation.Key, FilterParameter.Measurement)).Result;

                _totalCountMatch = TotalMatches;
                _mapDetails.AddRange(KillerDetailsMatch);

                SelectedMapImage = _maps.FirstOrDefault(x => x.IdMeasurement == selectedMeasurement.IdMeasurement).MapImage;
            }
        }

        #endregion

        #region Получение | Заполнение данных для сортировки/фильтрации

        private void GetAssociations()
        {
            var associations = _getAssociationUseCase.GetAll().Select(x =>
            {
                string label = x.IdPlayerAssociation switch
                {
                    (int)Associations.Me => "Статистика киллера",
                    (int)Associations.Opponent => "Статистика выжившего",
                    _ => string.Empty,
                };

                return new KeyValuePair<int, string>(x.IdPlayerAssociation, label);
            })
                .Where(x => x.Value != string.Empty)
                .ToList();

            foreach (var item in associations)
                PlayerAssociations.Add(item);
        }

        private void GetMaps()
        {
            _maps.AddRange(_getMapUseCase.GetAll());
        }

        private void GetMeasurement()
        {
            _measurements.AddRange(_getMeasurementUseCase.GetAll());
        }

        private void LoadTypeMaps(KeyValuePair<int, string> value)
        {
            Maps.Clear();

            if (value.Value == Map)
            {
                foreach (var item in _maps)
                {
                    Maps.Add(item);
                }
            }
            if (value.Value == Measurement)
            {
                foreach (var item in _measurements)
                {
                    Maps.Add(item);
                }
            }

            SelectedMap = Maps.FirstOrDefault();
        }

        #endregion

        /*--Расчеты--*/

        private readonly Func<DetailsMatchDTO, bool> WinMatchPredicate = x => x.CountKill > 2;
        private readonly Func<DetailsMatchDTO, bool> DrawMatchPredicate = x => x.CountKill == 2;
        private readonly Func<DetailsMatchDTO, bool> DefeatMatchPredicate = x => x.CountKill < 2;

        private readonly Func<DetailsMatchDTO, bool> MatchWithOfferingPredicate = x => x.IdWhoPlaceMapWin == (int)WhoPlacedMaps.Killer || x.IdWhoPlaceMapWin == (int)WhoPlacedMaps.Survivor || x.IdWhoPlaceMapWin == (int)WhoPlacedMaps.Me;
        private readonly Func<DetailsMatchDTO, bool> MatchNoOfferingPredicate = x => x.IdWhoPlaceMapWin == (int)WhoPlacedMaps.Random;

        #region Расчеты : Header

        private void CalculateHeaderStatistics()
        {
            CountMatch = _mapDetails.Count;

            KillRate = _calculationGeneralService.Average(_mapDetails.Sum(x => x.CountKill), CountMatch);
            HookRate = _calculationGeneralService.Average(_mapDetails.Sum(x => x.CountHook), CountMatch);
            WinRate = _calculationGeneralService.Percentage(_mapDetails.Where(x => WinMatchPredicate(x)).Count(), CountMatch);
            PickRate = _calculationGeneralService.Percentage(_mapDetails.Count, _totalCountMatch);
            RecentGenerators = _calculationGeneralService.Average(_mapDetails.Sum(x => x.RecentGenerator), CountMatch);

            EscapeRate = _calculationGeneralService.Percentage(_calculationSurvivorService.SumSurvivorsByTypeDeath(_mapDetails, TypeDeaths.Escape, Associations.None), CountMatch * 4);

            EscapeRatePersonal = _calculationGeneralService.Percentage(
                _calculationSurvivorService.SumSurvivorsByTypeDeath(_mapDetails, TypeDeaths.Escape, Associations.Me), 
                _calculationSurvivorService.CountMatchesPlayedAsSurvivor(_mapDetails, Associations.Me));

            CountWithOffering = _mapDetails.Count(x => MatchWithOfferingPredicate(x));
            CountNoOffering = _mapDetails.Count(x => MatchNoOfferingPredicate(x));

            PercentWithOffering = _calculationGeneralService.Percentage(CountWithOffering, CountMatch);
            PercentNoOffering = _calculationGeneralService.Percentage(CountNoOffering, CountMatch);
        }

        #endregion

        #region Расчеты : Text

        private void CalculateTextStatistics()
        {
            TimeStats();
            TimeWinStats();
            TimeDrawStats();
            TimeDefeatStats();
            OnPropertyChanged(nameof(TextStatistic));
        }

        private void TimeStats()
        {
            var (TotalTime, LongestTime, ShortestTime, AverageTime) = _calculationTimeService.CalculateTimeStats(_mapDetails);

            TextStatistic.TotalTimeMatches = _calculationTimeService.FormatTimeSpanAdaptive(TotalTime);
            TextStatistic.LongestTimeMatch = _calculationTimeService.FormatTimeSpanAdaptive(LongestTime);
            TextStatistic.ShortestTimeMatch = _calculationTimeService.FormatTimeSpanAdaptive(ShortestTime);
            TextStatistic.AverageTimeMatch = _calculationTimeService.FormatTimeSpanAdaptive(AverageTime);
        }

        private void TimeWinStats()
        {
            var (TotalWinTime, LongestWinTime, ShortestWinTime, AverageWinTime) = _calculationTimeService.CalculateTimeStats(_mapDetails.Where(x => WinMatchPredicate(x)).ToList());

            TextStatistic.LongestTimeWinMatch = _calculationTimeService.FormatTimeSpanAdaptive(LongestWinTime);
            TextStatistic.ShortestTimeWinMatch = _calculationTimeService.FormatTimeSpanAdaptive(ShortestWinTime);
            TextStatistic.AverageTimeWinMatch = _calculationTimeService.FormatTimeSpanAdaptive(AverageWinTime);
            TextStatistic.SeriesWins = _calculationGeneralService.Series(_mapDetails, x => WinMatchPredicate(x));

            CountMatchWin = _mapDetails.Count(x => WinMatchPredicate(x));
        }

        private void TimeDrawStats()
        {
            var (TotalDrawTime, LongestDrawTime, ShortestDrawTime, AverageDrawTime) = _calculationTimeService.CalculateTimeStats(_mapDetails.Where(x => DrawMatchPredicate(x)).ToList());

            TextStatistic.LongestTimeDrawMatch = _calculationTimeService.FormatTimeSpanAdaptive(LongestDrawTime);
            TextStatistic.ShortestTimeDrawMatch = _calculationTimeService.FormatTimeSpanAdaptive(ShortestDrawTime);
            TextStatistic.AverageTimeDrawMatch = _calculationTimeService.FormatTimeSpanAdaptive(AverageDrawTime);
            TextStatistic.SeriesDraws = _calculationGeneralService.Series(_mapDetails, x => DrawMatchPredicate(x));

            CountMatchDraw = _mapDetails.Count(x => DrawMatchPredicate(x));
        }

        private void TimeDefeatStats()
        {
            var (TotalDefeatTime, LongestDefeatTime, ShortestDefeatTime, AverageDefeatTime) = _calculationTimeService.CalculateTimeStats(_mapDetails.Where(x => DefeatMatchPredicate(x)).ToList());

            TextStatistic.LongestTimeDefeatMatch = _calculationTimeService.FormatTimeSpanAdaptive(LongestDefeatTime);
            TextStatistic.ShortestTimeDefeatMatch = _calculationTimeService.FormatTimeSpanAdaptive(ShortestDefeatTime);
            TextStatistic.AverageTimeDefeatMatch = _calculationTimeService.FormatTimeSpanAdaptive(AverageDefeatTime);
            TextStatistic.SeriesDefeats = _calculationGeneralService.Series(_mapDetails, x => DefeatMatchPredicate(x));

            CountMatchDefeat = _mapDetails.Count(x => DefeatMatchPredicate(x));
        }

        #endregion

        #region Расчеты : Details

        private void CalculateDetailsStatistics()
        {
            DetailingWhoPlaceMapStats();
            DetailingWhoPlaceMapWinStats();
            DetailingRecentGeneratorsStats();
            DetailingHooksStats();
            DetailingKillsStats();  
        }

        private void DetailingWhoPlaceMapStats()
        {
            DetailsWhoPlaceMap.Clear();

            foreach (var item in _calculationMapService.DetailingWhoPlaceMap(_mapDetails, _whoPlacedMapService.GetAll()))
                DetailsWhoPlaceMap.Add(item);
        }
        
        private void DetailingWhoPlaceMapWinStats()
        {
            DetailsWhoPlaceMapWin.Clear();

            foreach (var item in _calculationMapService.DetailingWhoPlaceMap(_mapDetails, _whoPlacedMapService.GetAll(), isWin: true))
                DetailsWhoPlaceMapWin.Add(item);
        }

        private void DetailingRecentGeneratorsStats()
        {
            DetailsRecentGenerators.Clear();

            foreach (var item in _calculationGeneralService.DetailingRecentGenerators(_mapDetails))
                DetailsRecentGenerators.Add(item);
        }

        private void DetailingHooksStats()
        {
            DetailsHooks.Clear();

            foreach (var item in _calculationKillerService.DetailingHooks(_mapDetails, $"Игр с {{0}} повесами: "))
            {
                DetailsHooks.Add(item);
            }
        }

        private void DetailingKillsStats()
        {
            DetailsKills.Clear();

            foreach (var item in _calculationKillerService.DetailingKills(_mapDetails, $"Игр с {{0}} киллами: "))
                DetailsKills.Add(item);
        }

        #endregion
    }
}