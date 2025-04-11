using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Enums;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.AssociationCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerAddonCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCase;
using DBDAnalytics.Application.UseCases.Abstraction.PlatformCase;
using DBDAnalytics.Application.UseCases.Abstraction.StatisticCase;
using DBDAnalytics.Application.UseCases.Abstraction.TypeDeathCase;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using System.Collections.ObjectModel;

namespace DBDAnalytics.WPF.ViewModels.PageVM
{
    internal class KillerDetailsVM : BaseVM, IUpdatable
    {
        private readonly IGetKillerUseCase _getKillerUseCase;
        private readonly IGetAssociationUseCase _getAssociationUseCase;
        private readonly IGetPlatformUseCase _getPlatformUseCase;
        private readonly IGetTypeDeathUseCase _getTypeDeathUseCase;

        private readonly IGetKillerAddonUseCase _getKillerAddonUseCase;
        private readonly IGetKillerPerkUseCase _getKillerPerkUseCase;

        private readonly ICalculationKillerService _calculationKillerService;
        private readonly ICalculationSurvivorService _calculationSurvivorService;
        private readonly ICalculationTimeService _calculationTimeService;
        private readonly ICalculationGeneralService _calculationGeneralService;

        private readonly IGetDetailsMatchUseCase _getDetailsMatchUseCase;

        public KillerDetailsVM(IGetKillerUseCase getKillerUseCase,
                               IGetAssociationUseCase getAssociationUseCase,
                               IGetPlatformUseCase getPlatformUseCase,
                               IGetTypeDeathUseCase getTypeDeathUseCase,
                               IGetKillerAddonUseCase getKillerAddonUseCase,
                               IGetKillerPerkUseCase getKillerPerkUseCase,
                               ICalculationKillerService calculationKillerService,
                               ICalculationSurvivorService calculationSurvivorService,
                               ICalculationTimeService calculationTimeService,
                               ICalculationGeneralService calculationGeneralService,
                               IGetDetailsMatchUseCase getDetailsMatchUseCase)
        {
            _getKillerUseCase = getKillerUseCase;
            _getAssociationUseCase = getAssociationUseCase;
            _getPlatformUseCase = getPlatformUseCase;
            _getTypeDeathUseCase = getTypeDeathUseCase;
            _getKillerAddonUseCase = getKillerAddonUseCase;
            _getKillerPerkUseCase = getKillerPerkUseCase;
            _calculationGeneralService = calculationGeneralService;
            _calculationKillerService = calculationKillerService;
            _calculationSurvivorService = calculationSurvivorService;
            _calculationTimeService = calculationTimeService;
            _getDetailsMatchUseCase = getDetailsMatchUseCase;

            IsCalculationAllData = true;
            IsCalculationSelectedAssociation = false;
            IsCalculationSelectedTimePeriod = false;

            TextStatistic = new();

            SelectedTimePeriod = PeriodTimes[2];

            GetKillerPerks();

            LoadAssociations();
            SelectedAssociation = Associations.FirstOrDefault();

            GetAndLoadKillers();
            SelectedKiller = Killers.FirstOrDefault();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {

        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        /*--Поля--*/

        #region Поля : Разрешение на вызов методов в Selected свойствах

        private bool IsCalculationAllData;
        private bool IsCalculationSelectedTimePeriod;
        private bool IsCalculationSelectedAssociation;

        #endregion

        /*--Коллекции--*/

        #region Коллекции : Списки матчей 

        private List<DetailsMatchDTO> _killerDetails { get; set; } = [];

        public ObservableCollection<KillerDTO> Killers { get; set; } = [];

        #endregion

        #region Колекции : Улучшение | Перки

        private List<KillerAddonDTO> _killerAddons = [];

        private List<KillerPerkDTO> _killerPerks = [];

        #endregion

        #region Колекции : Расчеты

        public ObservableCollection<LabeledValue> DetailsKills { get; set; } = [];

        public ObservableCollection<LabeledValue> DetailsHooks { get; set; } = [];

        public ObservableCollection<LabeledValue> DetailsRecentGenerators { get; set; } = [];

        public ObservableCollection<LabeledValue> DetailsSurvivorPlatforms { get; set; } = [];

        public ObservableCollection<LabeledValue> DetailsRSurvivorDisconnect { get; set; } = [];

        public ObservableCollection<LabeledValue> DetailsSurvivorAnonyms { get; set; } = [];

        public ObservableCollection<LabeledValue> DetailsSurvivorTypeDeaths { get; set; } = [];

        public ObservableCollection<LabeledValue> MatchesByTimePeriod { get; set; } = [];

        public ObservableCollection<LabeledValue> AvgScoreByTimePeriod { get; set; } = [];

        public ObservableCollection<LabeledValue> HourlyActivity { get; set; } = [];

        public ObservableCollection<LabeledValue> DayOrWeekActivity { get; set; } = [];

        public ObservableCollection<LabeledValue> MonthlyActivity { get; set; } = [];

        public ObservableCollection<LoadoutPopularity> KillerAddonPopularity { get; set; } = [];

        public ObservableCollection<DoubleAddonsPopularity<KillerAddonDTO>> KillerDoubleAddonPopularity { get; set; } = [];

        public ObservableCollection<QuadruplePerksPopularity<KillerPerkDTO>> KillerQuadruplePerkPopularity { get; set; } = [];

        public ObservableCollection<LoadoutPopularity> KillerPerkPopularity { get; set; } = [];

        #endregion

        #region Колекции : Параметры для сортировок

        public ObservableCollection<KeyValuePair<TimePeriod, string>> PeriodTimes { get; set; } = new()
        {
            new KeyValuePair<TimePeriod, string>(TimePeriod.Day,    "По дням"),
            new KeyValuePair<TimePeriod, string>(TimePeriod.Week,   "По неделям"),
            new KeyValuePair<TimePeriod, string>(TimePeriod.Month,  "По месяцам"),
            new KeyValuePair<TimePeriod, string>(TimePeriod.Year,   "По годам"),
        };

        public ObservableCollection<KeyValuePair<int, string>> Associations { get; set; } = [];

        public ObservableCollection<KillerPerkCategoryDTO> KillerPerkCategories { get; set; } = [];

        #endregion

        /*--Свойства--*/

        #region Свойства : Выбор Киллера

        private KillerDTO _selectedKiller;
        public KillerDTO SelectedKiller
        {
            get => _selectedKiller;
            set
            {
                if (IsCalculationAllData)
                {
                    _selectedKiller = value;
                    GetSelectedKillerDetailsMatches();

                    GetKillerAddons();

                    CalculateHeaderStatistics();
                    CalculateTextStatistics();
                    CalculateDetailsStatistics();
                    CalculateExtendedDetailsStatistics();
                    CalculateLoadoutDetailsStatistics();

                    IsCalculationSelectedTimePeriod = true;
                    IsCalculationSelectedAssociation = true;
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
                    GetSelectedKillerDetailsMatches();

                    CalculateHeaderStatistics();
                    CalculateTextStatistics();
                    CalculateDetailsStatistics();
                    CalculateExtendedDetailsStatistics();
                    CalculateLoadoutDetailsStatistics();
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Выбор Loadout киллера

        private LoadoutPopularity _selectedKillerAddons;
        public LoadoutPopularity SelectedKillerAddons
        {
            get => _selectedKillerAddons;
            set
            {
                _selectedKillerAddons = value;
                OnPropertyChanged();
            }
        }

        private DoubleAddonsPopularity<KillerAddonDTO> _selectedDoubleKillerAddonsPopularity;
        public DoubleAddonsPopularity<KillerAddonDTO> SelectedDoubleKillerAddonsPopularity
        {
            get => _selectedDoubleKillerAddonsPopularity;
            set
            {
                _selectedDoubleKillerAddonsPopularity = value;
                OnPropertyChanged();
            }
        }

        private LoadoutPopularity _selectedKillerPerks;
        public LoadoutPopularity SelectedKillerPerks
        {
            get => _selectedKillerPerks;
            set
            {
                _selectedKillerPerks = value;
                OnPropertyChanged();
            }
        }

        private QuadruplePerksPopularity<KillerPerkDTO> _selectedDoubleKillerPerksPopularity;
        public QuadruplePerksPopularity<KillerPerkDTO> SelectedDoubleKillerPerksPopularity
        {
            get => _selectedDoubleKillerPerksPopularity;
            set
            {
                _selectedDoubleKillerPerksPopularity = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойство : Выбор периудов для фильтрации

        private KeyValuePair<TimePeriod, string> _selectedTimePeriod;
        public KeyValuePair<TimePeriod, string> SelectedTimePeriod
        {
            get => _selectedTimePeriod;
            set
            {
                _selectedTimePeriod = value;

                if (IsCalculationSelectedTimePeriod)
                {
                    CalculateExtendedDetailsStatistics();
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Статистика в шапке

        private int _totalCountMatch;

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

        #region Свойство : Выбор индексов в списке киллеров

        private int _selectedKillerIndex;
        public int SelectedKillerIndex
        {
            get => _selectedKillerIndex;
            set
            {
                if (value >= 0 && value < Killers.Count)
                {
                    _selectedKillerIndex = value;
                    SelectedKiller = Killers[value];
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

        #region Переключение индиексов в списке киллеров

        private RelayCommand _nextKillerCommand;
        public RelayCommand NextKillerCommand
        {
            get => _nextKillerCommand ??= new(obj =>
            {
                SelectedKillerIndex++;
            });
        }

        private RelayCommand _previousKillerCommand;
        public RelayCommand PreviousKillerCommand
        {
            get => _previousKillerCommand ??= new(obj =>
            {
                SelectedKillerIndex--;
            });
        }

        #endregion

        #region Действия в шапке страницы

        private RelayCommand _updateCalculationCommand;
        public RelayCommand UpdateCalculationCommand
        {
            get => _updateCalculationCommand ??= new(obj =>
            {
                GetSelectedKillerDetailsMatches();

                CalculateHeaderStatistics();
                CalculateTextStatistics();
                CalculateDetailsStatistics();
                CalculateExtendedDetailsStatistics();
                CalculateLoadoutDetailsStatistics();
            });
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Получение списка матчей

        private void GetSelectedKillerDetailsMatches()
        {
            _killerDetails.Clear();

            var (KillerDetailsMatch, TotalMatches) = 
                Task.Run(() => _getDetailsMatchUseCase.GetDetailsMatch(SelectedKiller.IdKiller, SelectedAssociation.Key)).Result;

            _killerDetails.AddRange(KillerDetailsMatch);
            _totalCountMatch = TotalMatches;
        }

        #endregion

        #region Получение | Заполнение данных для сортировки/фильтрации

        private void GetAndLoadKillers()
        {
            var killer = _getKillerUseCase.GetAll();

            foreach (var item in killer.Skip(1))
                Killers.Add(item);
        }

        private void GetKillerAddons()
        {
            _killerAddons.Clear();

            _killerAddons.AddRange(Task.Run(() => _getKillerAddonUseCase.GetAllByIdKiller(SelectedKiller.IdKiller)).Result);
            //_killerAddons.AddRange(await _getKillerAddonUseCase.GetAllByIdKiller(SelectedKiller.IdKiller));
        }

        private void GetKillerPerks()
        {
            _killerPerks.Clear();

            _killerPerks.AddRange(_getKillerPerkUseCase.GetAll());
        }

        private List<KeyValuePair<int, string>> GetAssociations()
        {
            return _getAssociationUseCase.GetAll().Select(x =>
                {
                    string label = x.IdPlayerAssociation switch
                    {
                        1 => "Личная статистика",
                        3 => "Статистика противника",
                        _ => string.Empty,
                    };

                    return new KeyValuePair<int, string>(x.IdPlayerAssociation, label);
                })
                .Where(x => x.Value != string.Empty)
                .ToList();
        }

        private void LoadAssociations()
        {
            foreach (var item in GetAssociations())
                Associations.Add(item);
        }

        #endregion

        /*--Расчеты--*/

        private readonly Func<DetailsMatchDTO, bool> WinMatchPredicate = x => x.CountKill > 2;
        private readonly Func<DetailsMatchDTO, bool> DrawMatchPredicate = x => x.CountKill == 2;
        private readonly Func<DetailsMatchDTO, bool> DefeatMatchPredicate = x => x.CountKill < 2;

        #region Расчеты : Header

        private void CalculateHeaderStatistics()
        {
            CountMatch = _killerDetails.Count;

            KillRate = _calculationGeneralService.Average(_killerDetails.Sum(x => x.CountKill), CountMatch);
            HookRate = _calculationGeneralService.Average(_killerDetails.Sum(x => x.CountHook), CountMatch);
            WinRate = _calculationGeneralService.Percentage(_killerDetails.Where(x => x.CountKill > 2).Count(), CountMatch);
            PickRate = _calculationGeneralService.Percentage(_killerDetails.Count, _totalCountMatch);
            RecentGenerators = _calculationGeneralService.Average(_killerDetails.Sum(x => x.RecentGenerator), CountMatch);
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
            var (TotalTime, LongestTime, ShortestTime, AverageTime) = _calculationTimeService.CalculateTimeStats(_killerDetails);

            TextStatistic.TotalTimeMatches = _calculationTimeService.FormatTimeSpanAdaptive(TotalTime);
            TextStatistic.LongestTimeMatch = _calculationTimeService.FormatTimeSpanAdaptive(LongestTime);
            TextStatistic.ShortestTimeMatch = _calculationTimeService.FormatTimeSpanAdaptive(ShortestTime);
            TextStatistic.AverageTimeMatch = _calculationTimeService.FormatTimeSpanAdaptive(AverageTime);
        }

        private void TimeWinStats()
        {
            var (TotalWinTime, LongestWinTime, ShortestWinTime, AverageWinTime) = _calculationTimeService.CalculateTimeStats(_killerDetails.Where(x => WinMatchPredicate(x)).ToList());

            TextStatistic.LongestTimeWinMatch = _calculationTimeService.FormatTimeSpanAdaptive(LongestWinTime);
            TextStatistic.ShortestTimeWinMatch = _calculationTimeService.FormatTimeSpanAdaptive(ShortestWinTime);
            TextStatistic.AverageTimeWinMatch = _calculationTimeService.FormatTimeSpanAdaptive(AverageWinTime);
            TextStatistic.SeriesWins = _calculationGeneralService.Series(_killerDetails, x => WinMatchPredicate(x));

            CountMatchWin = _killerDetails.Count(x => WinMatchPredicate(x));
        }

        private void TimeDrawStats()
        {
            var (TotalDrawTime, LongestDrawTime, ShortestDrawTime, AverageDrawTime) = _calculationTimeService.CalculateTimeStats(_killerDetails.Where(x => DrawMatchPredicate(x)).ToList());

            TextStatistic.LongestTimeDrawMatch = _calculationTimeService.FormatTimeSpanAdaptive(LongestDrawTime);
            TextStatistic.ShortestTimeDrawMatch = _calculationTimeService.FormatTimeSpanAdaptive(ShortestDrawTime);
            TextStatistic.AverageTimeDrawMatch = _calculationTimeService.FormatTimeSpanAdaptive(AverageDrawTime);
            TextStatistic.SeriesDraws = _calculationGeneralService.Series(_killerDetails, x => DrawMatchPredicate(x));

            CountMatchDraw = _killerDetails.Count(x => DrawMatchPredicate(x));
        }

        private void TimeDefeatStats()
        {
            var (TotalDefeatTime, LongestDefeatTime, ShortestDefeatTime, AverageDefeatTime) = _calculationTimeService.CalculateTimeStats(_killerDetails.Where(x => DefeatMatchPredicate(x)).ToList());

            TextStatistic.LongestTimeDefeatMatch = _calculationTimeService.FormatTimeSpanAdaptive(LongestDefeatTime);
            TextStatistic.ShortestTimeDefeatMatch = _calculationTimeService.FormatTimeSpanAdaptive(ShortestDefeatTime);
            TextStatistic.AverageTimeDefeatMatch = _calculationTimeService.FormatTimeSpanAdaptive(AverageDefeatTime);
            TextStatistic.SeriesDefeats = _calculationGeneralService.Series(_killerDetails, x => DefeatMatchPredicate(x));

            CountMatchDefeat = _killerDetails.Count(x => DefeatMatchPredicate(x));
        }

        #endregion

        #region Расчеты : Details

        private void CalculateDetailsStatistics()
        {
            DetailingHooksStats();
            DetailingKillsStats();
            DetailingRecentGeneratorsStats();
            DetailingPlatformStats();
            DetailingDisconnectStats();
            DetailingAnonymousStats();
            DetailingTypeDeathStats();
        }

        private void DetailingHooksStats()
        {
            //var hookToColorConverter = new HookRateToColorConverter();

            //DetailsHooksChartData.Clear();
            DetailsHooks.Clear();

            foreach (var item in _calculationKillerService.DetailingHooks(_killerDetails, $"Игр с {{0}} повесами: "))
            {
                DetailsHooks.Add(item);
                //DetailsHooksChartData.Add(new PieChartItem
                //{
                //    Name = item.DisplayName,
                //    Value = item.DisplayValue,
                //    DisplayValue = $"{item.DisplayValue} %",
                //    Color = hookToColorConverter.Convert(item.DisplayNameValue, null, null, null) as Brush
                //});
            }
        }

        private void DetailingKillsStats()
        {
            DetailsKills.Clear();

            foreach (var item in _calculationKillerService.DetailingKills(_killerDetails, $"Игр с {{0}} киллами: "))
                DetailsKills.Add(item);
        }
                
        private void DetailingRecentGeneratorsStats()
        {
            DetailsRecentGenerators.Clear();

            foreach (var item in _calculationGeneralService.DetailingRecentGenerators(_killerDetails))
                DetailsRecentGenerators.Add(item);
        }   
        
        private void DetailingPlatformStats()
        {
            DetailsSurvivorPlatforms.Clear();

            foreach (var item in _calculationSurvivorService.DetailingPlatform(_killerDetails, _getPlatformUseCase.GetAll()))
                DetailsSurvivorPlatforms.Add(item);
        }

        private void DetailingDisconnectStats()
        {
            DetailsRSurvivorDisconnect.Clear();

            foreach (var item in _calculationSurvivorService.DetailingDisconnect(_killerDetails))
                DetailsRSurvivorDisconnect.Add(item);
        }

        private void DetailingAnonymousStats()
        {
            DetailsSurvivorAnonyms.Clear();

            foreach (var item in _calculationSurvivorService.DetailingAnonymous(_killerDetails))
                DetailsSurvivorAnonyms.Add(item);
        }

        private void DetailingTypeDeathStats()
        {
            DetailsSurvivorTypeDeaths.Clear();

            foreach (var item in _calculationSurvivorService.DetailingTypeDeath(_killerDetails, _getTypeDeathUseCase.GetAll()))
                DetailsSurvivorTypeDeaths.Add(item);
        }

        #endregion

        #region Расчеты : ExtendedDetails

        private void CalculateExtendedDetailsStatistics()
        {
            CountMatchesByTimePeriodStats();
            AvgScoreByTimePeriodStats();
            HourlyActivityStats();
            DayOrWeekActivityStats();
            MonthlyActivityStats();
        }

        private void CountMatchesByTimePeriodStats()
        {
            MatchesByTimePeriod.Clear();

            foreach (var item in _calculationGeneralService.CountMatchesByTimePeriod(_killerDetails, SelectedTimePeriod.Key))
                MatchesByTimePeriod.Add(item);
        }

        private void AvgScoreByTimePeriodStats()
        {
            AvgScoreByTimePeriod.Clear();

            foreach (var item in _calculationGeneralService.AvgScoreByTimePeriod(_killerDetails, SelectedTimePeriod.Key, x => x.KillerDTO.Score))
            {
                AvgScoreByTimePeriod.Add(item);
                //AvgScoreByTimePeriodBarChart.Add(new BarChartData
                //{
                //    Label = item.DisplayName,
                //    Value = item.DisplayValue,
                //});
            }   
        } 
        
        private void HourlyActivityStats()
        {
            HourlyActivity.Clear();

            foreach (var item in _calculationGeneralService.HourlyActivity(_killerDetails))
                HourlyActivity.Add(item);
        }  
        
        private void DayOrWeekActivityStats()
        {
            DayOrWeekActivity.Clear();

            foreach (var item in _calculationGeneralService.DayOrWeekActivity(_killerDetails))
                DayOrWeekActivity.Add(item);
        }
        
        private void MonthlyActivityStats()
        {
            MonthlyActivity.Clear();

            foreach (var item in _calculationGeneralService.MonthlyActivity(_killerDetails))
                MonthlyActivity.Add(item);
        }

        #endregion

        #region Расчеты : Loadout

        private void CalculateLoadoutDetailsStatistics()
        {
            KillerAddonPopularityStats();
            KillerDoubleAddonPopularityStats();
            KillerQuadruplePerkPopularityStats();
            KillerPerkPopularityStats();
        }

        private void KillerAddonPopularityStats()
        {
            KillerAddonPopularity.Clear();

            var list = _calculationGeneralService.CalculatePopularity(
                _killerDetails,
                _killerAddons,
                (match, addon) => match.KillerDTO.FirstAddonID == addon.IdKillerAddon || match.KillerDTO.SecondAddonID == addon.IdKillerAddon,
                addon => addon.AddonName,
                addon => addon.AddonImage,
                match => match.CountKill > 2);

            foreach (var item in list)
                KillerAddonPopularity.Add(item);

            SelectedKillerAddons = KillerAddonPopularity.FirstOrDefault();
        }

        private void KillerDoubleAddonPopularityStats()
        {
            KillerDoubleAddonPopularity.Clear();

            var list = _calculationGeneralService.DoubleAddonPopularity(
                _killerDetails, _killerAddons,
                addon => _killerAddons.FirstOrDefault(x => x.IdKillerAddon == addon),
                idAddonSelector => (idAddonSelector.KillerDTO.FirstAddonID, idAddonSelector.KillerDTO.SecondAddonID),
                pairKey =>
                {
                    return match => (match.KillerDTO?.FirstAddonID == pairKey.FirstAddonID && match.KillerDTO?.SecondAddonID == pairKey.SecondAddonID) ||
                                    (match.KillerDTO?.FirstAddonID == pairKey.SecondAddonID && match.KillerDTO?.SecondAddonID == pairKey.FirstAddonID);
                },
                win => win.CountKill > 2);

            foreach (var item in list.OrderByDescending(x => x.Count))
                KillerDoubleAddonPopularity.Add(item);

            SelectedDoubleKillerAddonsPopularity = KillerDoubleAddonPopularity.FirstOrDefault();
        }

        private void KillerQuadruplePerkPopularityStats()
        {
            KillerQuadruplePerkPopularity.Clear();

            var list = _calculationGeneralService.QuadruplePerkPopularity(
                _killerDetails, _killerPerks,
                perk => _killerPerks.FirstOrDefault(x => x.IdKillerPerk == perk),
                idPerkSelector => (idPerkSelector.KillerDTO.FirstPerkID, idPerkSelector.KillerDTO.SecondPerkID, idPerkSelector.KillerDTO.ThirdPerkID, idPerkSelector.KillerDTO.FourthPerkID),
                 comboKey =>
                 {
                     return match =>
                     {
                         var matchPerks = new List<int?>();
                         if (match.KillerDTO != null)
                         {
                             matchPerks.Add(match.KillerDTO.FirstPerkID);
                             matchPerks.Add(match.KillerDTO.SecondPerkID);
                             matchPerks.Add(match.KillerDTO.ThirdPerkID);
                             matchPerks.Add(match.KillerDTO.FourthPerkID);
                         }
                         else
                             return false;

                         if (matchPerks.Any(id => !id.HasValue))
                             return false;

                         var matchPerkIdsSet = matchPerks.Select(id => id!.Value).ToHashSet();

                         var targetPerkIdsSet = new HashSet<int> { comboKey.FirstPerkID, comboKey.SecondPerkID, comboKey.ThirdPerkID, comboKey.FourthPerkID };

                         return matchPerkIdsSet.Count == 4 && targetPerkIdsSet.SetEquals(matchPerkIdsSet);

                         //var matchPerkIdsList = matchPerks.Select(id => id!.Value).ToList();
                         //matchPerkIdsList.Sort();
                         //return matchPerkIdsList.Count == 4 &&
                         //       matchPerkIdsList[0] == comboKey.FirstPerkID &&
                         //       matchPerkIdsList[1] == comboKey.SecondPerkID &&
                         //       matchPerkIdsList[2] == comboKey.ThirdPerkID &&
                         //       matchPerkIdsList[3] == comboKey.FourthPerkID;
                     };
                 },
                win => win.CountKill > 2);

            foreach (var item in list.OrderByDescending(x => x.Count))
                KillerQuadruplePerkPopularity.Add(item);

            //SelectedDoubleKillerAddonsPopularity = KillerDoubleAddonPopularity.FirstOrDefault();
        }

        private void KillerPerkPopularityStats()
        {
            KillerPerkPopularity.Clear();

            var list = _calculationGeneralService.CalculatePopularity(
                _killerDetails,
                _getKillerPerkUseCase.GetAll(),
                (match, perk) => match.KillerDTO.FirstPerkID == perk.IdKillerPerk || match.KillerDTO.SecondPerkID == perk.IdKillerPerk || match.KillerDTO.ThirdPerkID == perk.IdKillerPerk || match.KillerDTO.FourthPerkID == perk.IdKillerPerk,
                perk => perk.PerkName,
                perk => perk.PerkImage,
                match => match.CountKill > 2);

            foreach (var item in list)
                KillerPerkPopularity.Add(item);

            SelectedKillerPerks = KillerPerkPopularity.FirstOrDefault();
        }

        #endregion

    }
}