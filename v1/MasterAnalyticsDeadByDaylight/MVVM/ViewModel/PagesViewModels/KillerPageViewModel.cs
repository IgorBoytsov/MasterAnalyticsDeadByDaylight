using LiveCharts;
using LiveCharts.Wpf;
using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.ChartModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation;
using MasterAnalyticsDeadByDaylight.Utils.Calculation;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Media;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    internal class KillerPageViewModel : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDataService _dataService;
        private readonly IPageNavigationService _pageNavigationService;

        public KillerPageViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dataService = _serviceProvider.GetService<IDataService>();
            _pageNavigationService = _serviceProvider.GetService<IPageNavigationService>();

            IsPopupFilterOpen = false;

            // Получение нужных данных при запуске страницы
            GetKillers();
            GetPlayerAssociations();
        }

        public void Update(object value)
        {
            //Обновление расчетов, если был добавлен матч с участие данного киллера
            if (value is Killer killer)
            {
                if (SelectedKiller.IdKiller == killer.IdKiller)
                {
                    _matches.Clear();
                    _matches.AddRange(GetMatches(SelectedKiller));
                    CalculateHeaderStats();
                    CalculateExtendedStats();
                }
            }
        }

        /*--Общие Свойства \ Коллекции--------------------------------------------------------------------*/

        #region Коллекции : Общие

        public ObservableCollection<Killer> Killers { get; set; } = [];

        public ObservableCollection<PlayerAssociation> PlayerAssociations { get; set; } = [];

        public ObservableCollection<KillerStat> KillerStats { get; set; } = [];

        #endregion

        #region Коллекции : Расширеная статистика

        public ObservableCollection<ActivityByHoursTracker> ActivityByHours { get; set; } = [];

        public ObservableCollection<AverageScoreTracker> KillerAverageScore { get; set; } = [];

        public ObservableCollection<CountMatchTracker> CountMatch { get; set; } = [];

        public ObservableCollection<KillerHooksTracker> KillerHooks { get; set; } = [];

        public ObservableCollection<KillerKillRateTracker> KillRates { get; set; } = [];

        public ObservableCollection<KillerWinRateTracker> KillerWinRates { get; set; } = [];

        public ObservableCollection<PlayerPlatformTracker> PlayerPlatforms { get; set; } = [];

        public ObservableCollection<RecentGeneratorsTracker> RecentGenerators { get; set; } = [];

        public ObservableCollection<SurvivorAnonymousTracker> SurvivorAnonymous { get; set; } = [];

        public ObservableCollection<SurvivorBotTracker> SurvivorBots { get; set; } = [];

        public ObservableCollection<SurvivorTypeDeathTracker> SSurvivorTypeDeaths { get; set; } = [];

        #endregion

        #region Коллекция : Матчи выбранного киллера

        private List<GameStatistic> _matches = [];

        #endregion

        #region Свойства : Выбор киллера \ выбор индекса

        private Killer _selectedKiller;
        public Killer SelectedKiller
        {
            get => _selectedKiller;
            set
            {
                if (_selectedKiller != value)
                {
                    _selectedKiller = value;
                    _matches.Clear();
                    _matches.AddRange(GetMatches(value));
                    CalculateHeaderStats();
                    CalculateExtendedStats();
                    OnPropertyChanged();
                }
            }
        }

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
                OnPropertyChanged();
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
                    KillerStats.Clear();
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Свойства : CountMatches, K\R, W\R, P\R, GameTime, ShortestTimeMatch, LongestTimeMatch, AVGTimeMatch

        private double _countMatches;
        public double CountMatches
        {
            get => _countMatches;
            set
            {
                _countMatches = value;
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

        private string _timeSpent;
        public string TimeSpent
        {
            get => _timeSpent;
            set
            {
                _timeSpent = value;
                OnPropertyChanged();
            }
        }

        private string _shortestTimeMatch;
        public string ShortestTimeMatch
        {
            get => _shortestTimeMatch;
            set
            {
                _shortestTimeMatch = value;
                OnPropertyChanged();
            }
        }

        private string _longestTimeMatch;
        public string LongestTimeMatch
        {
            get => _longestTimeMatch;
            set
            {
                _longestTimeMatch = value;
                OnPropertyChanged();
            }
        }

        private string _aVGTimeMatch;
        public string AVGTimeMatch
        {
            get => _aVGTimeMatch;
            set
            {
                _aVGTimeMatch = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойство : Максимальная ширина элементов

        public int MaxWidth { get; set; } = 1200;

        #endregion

        #region Свойство : Popup - Список киллеров для сравнения

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
        private RelayCommand _nextKillerCommand;
        public RelayCommand NextKillerCommand { get => _nextKillerCommand ??= new(obj => { NextKiller(); }); }

        private RelayCommand _previousKillerCommand;
        public RelayCommand PreviousKillerCommand { get => _previousKillerCommand ??= new(obj => { PreviousKiller(); }); }

        //Команды добавление киллеров в список сравнения
        private RelayCommand _addSingleToComparisonCommand;
        public RelayCommand AddSingleToComparisonCommand { get => _addSingleToComparisonCommand ??= new(obj => { AddToComparison(); }); }

        private RelayCommand _addAllToComparisonCommand;
        public RelayCommand AddAllToComparisonCommand { get => _addAllToComparisonCommand ??= new(obj => { AddAllToComparison(); }); }
        
        //Очистка списка статистики киллеров
        private RelayCommand _clearComparisonListCommand;
        public RelayCommand ClearComparisonListCommand { get => _clearComparisonListCommand ??= new(obj => { KillerStats.Clear(); }); }

        //Команд открытие страницы сравнений
        private RelayCommand _openComparisonPageCommand;
        public RelayCommand OpenComparisonPageCommand { get => _openComparisonPageCommand ??= new(obj => { OpenComparisonPage(); }); } 
        
        //Команда обновление данных
        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { ReloadData(); }); }

        //Открытие Popup
        private RelayCommand _openPopupListKillersCommand;
        public RelayCommand OpenPopupListKillersCommand { get => _openPopupListKillersCommand ??= new(obj => { IsPopupFilterOpen = true; }); }

        /*--Получение первоначальных данных---------------------------------------------------------------*/

        #region Метод : Получение списка "Киллеров"

        private void GetKillers()
        {
            foreach (var item in _dataService.GetAllDataInList<Killer>(x => x.Skip(1)))
            {
                Killers.Add(item);
            }
        }

        #endregion

        #region Метод : Получение списка "Игровой ассоциации"

        private void GetPlayerAssociations()
        {
            foreach (var item in _dataService.GetAllDataInList<PlayerAssociation>())
            {
                PlayerAssociations.Add(item);
            }
            SelectedPlayerAssociation = PlayerAssociations.FirstOrDefault();
        }

        #endregion

        #region Метод : Получение списка матчей у текущего киллера

        private List<GameStatistic> GetMatches(Killer killer)
        {
            return _dataService.GetAllDataInList<GameStatistic>(
                x => x.Include(x => x.IdKillerNavigation)
                .Include(x => x.IdSurvivors1Navigation)
                    .Include(x => x.IdSurvivors2Navigation)
                        .Include(x => x.IdSurvivors3Navigation)
                            .Include(x => x.IdSurvivors4Navigation)
                .Where(x => x.IdKillerNavigation.IdKiller == killer.IdKiller && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociation.IdPlayerAssociation));
        }

        #endregion

        #region Метод : Обновление данных

        private void ReloadData()
        {
            _matches.Clear();
            _matches.AddRange(GetMatches(SelectedKiller));
            CalculateHeaderStats();
            CalculateExtendedStats();
        }

        #endregion 

        /*--Взаимодействие с списком----------------------------------------------------------------------*/

        #region Методы : Переключение элементов списка киллеров (По индексу)

        private void PreviousKiller()
        {
            SelectedKillerIndex--;
        }

        private void NextKiller()
        {
            SelectedKillerIndex++;
        }

        #endregion

        /*--Расчеты---------------------------------------------------------------------------------------*/

        #region Метод : Открытие страницы сравнений

        private void OpenComparisonPage()
        {
            _pageNavigationService.NavigateTo("ComparisonPage", KillerStats);
        }

        #endregion

        #region Метод : Основная статистика

        private async void CalculateHeaderStats()
        {
            if (_matches.Count != 0)
            {
                var doubleStats = await Task.WhenAll(
                     CalculationKiller.AVGKillRateAsync(_matches),
                     CalculationKiller.WinRateAsync(_matches.Count(x => x.CountKills >= 3), _matches.Count),
                     CalculationKiller.PickRateAsync(_matches.Count, _dataService.Count<GameStatistic>(x => x.Where(x => x.IdKillerNavigation.IdAssociation == 1)))
                     );

                var timeSpanStats = await Task.WhenAll(
                    CalculationTime.TotalTime(_matches)
                    );

                var (Longest, Fastest, AVG) = await CalculationTime.StatTimeMatchAsync(_matches, SelectedKiller.IdKiller, 1);

                CountMatches = _matches.Count;
                KillRate = doubleStats[0];
                WinRate = doubleStats[1];
                PickRate = doubleStats[2];
                TimeSpent = timeSpanStats[0].TotalHours > 24 ? $"{timeSpanStats[0].Days}д {timeSpanStats[0].Hours}ч {timeSpanStats[0].Minutes}м" : $"{timeSpanStats[0].Hours}ч {timeSpanStats[0].Minutes}м";
                ShortestTimeMatch = Fastest;
                LongestTimeMatch = Longest;
                AVGTimeMatch = AVG;
            }
            else
            {
                CountMatches = 0;
                KillRate = 0;
                WinRate = 0;
                TimeSpent = "0д 0ч 0}м";
                ShortestTimeMatch = "0";
                LongestTimeMatch = "0";
                AVGTimeMatch = "0";
            }
        }

        #endregion

        #region Методы : Расширение статистика

        private async void CalculateExtendedStats()
        {
            if (_matches.Count != 0)
            {
                var tasks = new List<Task>
                {
                    ActivityByHoursAsync(),
                    KillerAverageScoreAsync(),
                    CountMatchAsync(),
                    KillerHooksAsync(),
                    KillRateAsync(),
                    WinRateAsync(),
                    PlayerPlatformAsync(),
                    RecentGeneratorsAsync(),
                    SurvivorAnonymousAsync(),
                    SurvivorBotsAsync(),
                    SurvivorTypeDeathsAsync(),
                };

                await Task.WhenAll(tasks);
            }
            else
            {
                ActivityByHours.Clear();
                ActivityByHours.Add(new ActivityByHoursTracker() {CountMatch = 0, Hours = DateTime.MinValue} );

                KillerAverageScore.Clear();
                KillerAverageScore.Add(new AverageScoreTracker() { AvgScore = 0, DateTime = DateTime.MinValue.ToString() });
            }
        }

        #region Активность по часам

        public SeriesCollection ActivityByHoursSeriesCollection { get; set; } = [];
        public List<string> ActivityByHoursLabels { get; set; } = [];

        private async Task ActivityByHoursAsync()
        {
            var values = new Dictionary<DateTime, double>();
            var activityByHours = await CalculationGeneral.CountMatchesPlayedInEachHourAsync(_matches);

            foreach (var item in activityByHours)
            {
                values.Add(item.Hours, Math.Round(item.CountMatch, 0));
            }

            ActivityByHoursLabels = values.Keys.Select(dt => dt.ToString("HH", CultureInfo.InvariantCulture) + "ч").ToList();

            ActivityByHoursSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Количество",
                    Values = new ChartValues<double>(values.Values), // Значения столбцов
                    DataLabels = true, // Показывать значения на столбцах
                    Fill = new SolidColorBrush(Color.FromRgb(100, 149, 237)),  // Цвет столбцов
                    Stroke = Brushes.Black,          // Цвет обводки столбцов
                    StrokeThickness = 2,
                    LabelPoint = point => Math.Round(point.Y).ToString(),
                }
            };
            OnPropertyChanged(nameof(ActivityByHoursSeriesCollection));
            OnPropertyChanged(nameof(ActivityByHoursLabels));
        }

        #endregion

        #region Счет за периуд времени

        public SeriesCollection KillerAverageScoreSeriesCollection { get; set; } = [];
        public List<string> KillerAverageScoreHoursLabels { get; set; } = [];

        private async Task KillerAverageScoreAsync()
        {
            var values = new Dictionary<string, double>();
            var avgScore = await CalculationKiller.AverageScoreForPeriodTimeAsyncAsync(_matches, TypeTime.Month);

            foreach (var item in avgScore)
            {
                values.Add(item.DateTime, item.AvgScore);
            }

            KillerAverageScoreHoursLabels = [.. values.Keys];

            KillerAverageScoreSeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Счет",
                    Values = new ChartValues<double>(values.Values),
                    DataLabels = true,
                    Fill = new SolidColorBrush(Color.FromRgb(100, 149, 237)),
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    LabelPoint = point => Math.Round(point.Y).ToString(),
                }
            };
            OnPropertyChanged(nameof(KillerAverageScoreSeriesCollection));
            OnPropertyChanged(nameof(KillerAverageScoreHoursLabels));
        }

        #endregion

        private async Task CountMatchAsync()
        {
            CountMatch.Clear();
            foreach (var item in await CalculationGeneral.CountMatchForPeriodTimeAsync(_matches, TypeTime.Month))
                CountMatch.Add(item);
        }

        private async Task KillRateAsync()
        {
            KillRates.Clear();
            foreach (var item in await CalculationKiller.KillRateForPeriodTimeAsync(_matches, TypeTime.Month))
                KillRates.Add(item);
        }

        private async Task KillerHooksAsync()
        {
            KillerHooks.Clear();
            foreach (var item in await CalculationKiller.CountHooksAsync(_matches))
                KillerHooks.Add(item);
        }

        private async Task WinRateAsync()
        {
            KillerWinRates.Clear();
            foreach (var item in await CalculationKiller.WinRateForPeriodTimeAsync(_matches, TypeTime.Month))
                KillerWinRates.Add(item);
        }

        private async Task PlayerPlatformAsync()
        {
            PlayerPlatforms.Clear();
            foreach (var item in await CalculationSurvivor.PlayersByPlatformsAsync(_matches))
                PlayerPlatforms.Add(item);
        }

        private async Task RecentGeneratorsAsync()
        {
            RecentGenerators.Clear();
            foreach (var item in await CalculationGeneral.RecentGeneratorsAsync(_matches))
                RecentGenerators.Add(item);
        }
         
        private async Task SurvivorAnonymousAsync()
        {
            SurvivorAnonymous.Clear();
            foreach (var item in await CalculationSurvivor.AnonymousPlayersAsync(_matches))
                SurvivorAnonymous.Add(item);
        } 
        
        private async Task SurvivorBotsAsync()
        {
            SurvivorBots.Clear();
            foreach (var item in await CalculationSurvivor.PlayersBotAsync(_matches))
                SurvivorBots.Add(item);
        }

        private async Task SurvivorTypeDeathsAsync()
        {
            SSurvivorTypeDeaths.Clear();
            foreach (var item in await CalculationSurvivor.TypeDeathSurvivorsAsync(_matches, _dataService))
                SSurvivorTypeDeaths.Add(item);
        }

        #endregion

        #region Методы : Создание KillerStat - добавлени его в список сравнения

        private async void AddToComparison()
        {
            if (KillerStats.Contains(KillerStats.FirstOrDefault(x => x.KillerID == SelectedKiller.IdKiller)))
                return;

            var (KillingZero, KillingOne, KillingTwo, KillingThree, KillingFour) = await CalculationKiller.KillDistributionAsync(_matches);

            KillerStats.Add(new KillerStat()
            {
                KillerID = SelectedKiller.IdKiller,
                KillerName = SelectedKiller.KillerName,
                KillerImage = SelectedKiller.KillerImage,
                KillerCountGame = CountMatches,
                KillerPickRate = PickRate,
                KillerKillRate = KillRate,
                KillerKillRatePercentage = await CalculationKiller.AVGKillRatePercentageAsync(_matches),
                KillerWinRate = WinRate,
                KillerMatchWin = await CalculationKiller.CountMatchWinAsync(_matches),
                KillingZeroSurvivor = KillingZero,
                KillingOneSurvivors = KillingOne,
                KillingTwoSurvivors = KillingTwo,
                KillingThreeSurvivors = KillingThree,
                KillingFourSurvivors = KillingFour,
            });
        }

        private async void AddAllToComparison()
        {
            foreach (var killer in Killers)
            {
                if (KillerStats.Contains(KillerStats.FirstOrDefault(x => x.KillerID == killer.IdKiller)))
                    continue;

                var matches = GetMatches(killer);
                double CountKill = await CalculationKiller.CountKillAsync(matches);
                //double KillRate = await _killerCalculationService.CalculatingKillRate(GameStat, CountKill);
                //double KillRatePercentage = await _killerCalculationService.CalculatingKillRatePercentage(KillRate); 
                double KillRate = await CalculationKiller.AVGKillRateAsync(matches);
                double KillRatePercentage = await CalculationKiller.AVGKillRatePercentageAsync(matches);
                double PickRate = await CalculationKiller.PickRateAsync(matches.Count, _dataService.Count<GameStatistic>(x => x.Where(x => x.IdKillerNavigation.IdAssociation == 1)));
                double MatchWin = await CalculationKiller.CountMatchWinAsync(matches);
                double WinRate = await CalculationKiller.WinRateAsync((int)MatchWin, matches.Count);

                var KillDistribution = await CalculationKiller.KillDistributionAsync(matches);

                double KillingZero = KillDistribution.KillingZero;
                double KillingOne = KillDistribution.KillingOne;
                double KillingTwo = KillDistribution.KillingTwo;
                double KillingThree = KillDistribution.KillingThree;
                double KillingFour = KillDistribution.KillingFour;

                App.Current.Dispatcher.Invoke(() =>
                {
                    var killerStat = new KillerStat()
                    {
                        KillerID = killer.IdKiller,
                        KillerName = killer.KillerName,
                        KillerImage = killer.KillerImage,
                        KillerPickRate = PickRate,
                        KillerCountGame = matches.Count,
                        KillerKillRate = KillRate,
                        KillerKillRatePercentage = KillRatePercentage,
                        KillerWinRate = WinRate,
                        KillerMatchWin = MatchWin,
                        KillingZeroSurvivor = KillingZero,
                        KillingOneSurvivors = KillingOne,
                        KillingTwoSurvivors = KillingTwo,
                        KillingThreeSurvivors = KillingThree,
                        KillingFourSurvivors = KillingFour
                    };
                    KillerStats.Add(killerStat);
                });
            }
        }

        #endregion

    }
}