using LiveCharts.Wpf;
using LiveCharts;
using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using MasterAnalyticsDeadByDaylight.MVVM.Model.ChartModel;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using MasterAnalyticsDeadByDaylight.MVVM.View.Pages;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.CalculationService.KillerService;
using MasterAnalyticsDeadByDaylight.Services.CalculationService.MapService;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using System.Text.RegularExpressions;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    internal class KillerPageViewModel : BaseViewModel
    {
        #region Колекции 

        private ObservableCollection<KillerStat> KillerStatList { get; set; } = [];

        public ObservableCollection<KillerStat> KillerStatSortedList { get; set; } = [];

        public ObservableCollection<GameStatistic> MatchesSelectedKillerList { get; set; } = [];

        public List<PlayerAssociation> PlayerAssociationList { get; set; } = [];

        public List<TypeDeath> TypeDeathList { get; set; } = [];

        //public List<Map> MapList { get; set; } = [];

        public ObservableCollection<string> SortingList { get; set; } =
            [
            "Дате выхода (Убыв.)", "Дате выхода (Возр.)",
            "Алфавит (Я-А)", "Алфавит (А-Я)",
            "Пикрейт (Убыв.)", "Пикрейт (Возр.)",
            "Винрейт (Убыв.)","Винрейт (Возр.)",
            "Киллрейт (Убыв.)","Киллрейт (Возр.)",
            "Количеству сыгранных игр (Убыв.)", "Количеству сыгранных игр (Возр.)",
            "Количеству выигранных игр (Убыв.)", "Количеству выигранных игр (Возр.)",
            ];

        #endregion

        #region Свойства Selected

        private string _selectedKillerStatSortItem;
        public string SelectedKillerStatSortItem
        {
            get => _selectedKillerStatSortItem;
            set
            {
                _selectedKillerStatSortItem = value;
                OnPropertyChanged();
            }
        }

        private PlayerAssociation _selectedPlayerAssociationStatItem;
        public PlayerAssociation SelectedPlayerAssociationStatItem
        {
            get => _selectedPlayerAssociationStatItem;
            set
            {
                _selectedPlayerAssociationStatItem = value;
                OnPropertyChanged();
            }
        }

        private KillerStat _selectedKiller;
        public KillerStat SelectedKiller
        {
            get => _selectedKiller;
            set
            {
                _selectedKiller = value;
                GetAdditionalStatistics();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства Visibility

        private Visibility _killerListVisibility;
        public Visibility KillerListVisibility
        {
            get => _killerListVisibility;
            set
            {
                _killerListVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _sortMenuVisibility;
        public Visibility SortMenuVisibility
        {
            get => _sortMenuVisibility;
            set
            {
                _sortMenuVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _detailedInformationVisibility;
        public Visibility DetailedInformationVisibility
        {
            get => _detailedInformationVisibility;
            set
            {
                _detailedInformationVisibility = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойство для поиска по KillerStatSortedList

        private string _searchTextBox;
        public string SearchTextBox
        {
            get => _searchTextBox;
            set
            {
                _searchTextBox = value;
                SearchKillerName();
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

        private KillerPage _killerPage;

        private readonly IDataService _dataService;
        private readonly IKillerCalculationService _killerCalculationService;
        private readonly IMapCalculationService _mapCalculationService;

        public KillerPageViewModel(KillerPage KillerPage,
                                   IDataService dataService, 
                                   IKillerCalculationService killerCalculationService,
                                   IMapCalculationService mapCalculationService)
        {
            _killerPage = KillerPage;
            _dataService = dataService;
            _killerCalculationService = killerCalculationService;
            _mapCalculationService = mapCalculationService;

            GetPlayerAssociationData();

            SelectedKillerStatSortItem = SortingList.First();

            SetDefaultVisibility();
            IsFilterPopupOpen = false;

            GetKillerStatisticData();
        }

        #region Команды

        private RelayCommand _showDetailsKillerCommand;
        public RelayCommand ShowDetailsKillerCommand => _showDetailsKillerCommand ??= new RelayCommand(ShowDetailsKiller);

        private RelayCommand _backToListViewCommand;
        public RelayCommand BackToListViewCommand { get => _backToListViewCommand ??= new(obj =>
        {
            KillerListVisibility = Visibility.Visible;
            SortMenuVisibility = Visibility.Visible;
            DetailedInformationVisibility = Visibility.Collapsed;
            _killerPage.MainScrollViewer.ScrollToTop();
        }); }

        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { GetKillerStatisticData(); SortKillerStatsByDescendingOrder(); SearchTextBox = string.Empty; }); }

        private RelayCommand _openFilterCommand;
        public RelayCommand OpenFilterCommand { get => _openFilterCommand ??= new(obj => { IsFilterPopupOpen = true; }); }

        private RelayCommand _closeFilterCommand;
        public RelayCommand CloseFilterCommand { get => _closeFilterCommand ??= new(obj => 
        { 
            IsFilterPopupOpen = false;
            GetKillerStatisticData();
        }); }

        #endregion

        #region Методы видимости элементов

        private void SetDefaultVisibility()
        {
            SortMenuVisibility = Visibility.Visible;
            DetailedInformationVisibility = Visibility.Collapsed;
        }

        #endregion

        #region Методы получение данных

        private async void GetKillerStatisticData()
        {
            KillerStatList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                List<Killer> KillerList = await _dataService.GetAllDataInListAsync<Killer>(x => x.Skip(1));

                int CountMatch = await _killerCalculationService.GetAllKillerCountMatch(SelectedPlayerAssociationStatItem.IdPlayerAssociation);

                foreach (var killer in KillerList)
                {
                    List<GameStatistic> GameStat = await _killerCalculationService.GetSelectedKillerMatch(killer.IdKiller, SelectedPlayerAssociationStatItem.IdPlayerAssociation);
                    double CountKill = await _killerCalculationService.CalculatingCountKill(GameStat);
                    double KillRate = await _killerCalculationService.CalculatingKillRate(GameStat, CountKill);
                    double KillRatePercentage = await _killerCalculationService.CalculatingKillRatePercentage(KillRate);
                    double PickRate = await _killerCalculationService.CalculatingPickRate(GameStat.Count, CountMatch);
                    double MatchWin = await _killerCalculationService.CalculatingKillerCountMatchWin(GameStat);
                    double WinRate = await _killerCalculationService.CalculatingWinRate((int)MatchWin, GameStat.Count);

                    var KillDistribution = await _killerCalculationService.CalculatingKillerKillDistribution(GameStat);

                    double KillingZero = KillDistribution.KillingZero;
                    double KillingOne = KillDistribution.KillingOne;
                    double KillingTwo = KillDistribution.KillingTwo;
                    double KillingThree = KillDistribution.KillingThree;
                    double KillingFour = KillDistribution.KillingFour;

                    var killerStat = new KillerStat()
                    {
                        KillerID = killer.IdKiller,
                        KillerName = killer.KillerName,
                        KillerImage = killer.KillerImage,
                        KillerPickRate = PickRate,
                        KillerCountGame = GameStat.Count,
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
                    KillerStatList.Add(killerStat);
                }
                SortKillerStatList();
            }
        }

        private async void GetPlayerAssociationData()
        {
            PlayerAssociationList.Clear();
            var association = await _dataService.GetAllDataInListAsync<PlayerAssociation>();
            PlayerAssociationList.AddRange(association.Where(x => x.IdPlayerAssociation == 1 || x.IdPlayerAssociation == 3));
            SelectedPlayerAssociationStatItem = PlayerAssociationList.First();
        }

        #endregion

        #region Методы сортировки

        private void SortKillerStatList()
        {
            switch (SelectedKillerStatSortItem)
            {
                case "Дате выхода (Убыв.)":
                    SortKillerStatsByDescendingOrder();
                    break;
                case "Дате выхода (Возр.)":
                    SortKillerStatsByAscendingOrder();
                    break;
                case "Алфавит (Я-А)":
                    SortKillerStatsByKillerNameAscendingOrder();
                    break;
                case "Алфавит (А-Я)":
                    SortKillerStatsByKillerNameDescendingOrder();
                    break;
                case "Пикрейт (Убыв.)":
                    SortKillerStatsByKillerPickRateDescendingOrder();
                    break;
                case "Пикрейт (Возр.)":
                    SortKillerStatsByKillerPickRateAscendingOrder();
                    break;
                case "Винрейт (Убыв.)":
                    SortKillerStatsByKillerWinRateDescendingOrder();
                    break;
                case "Винрейт (Возр.)":
                    SortKillerStatsByKillerWinRateAscendingOrder();
                    break;
                case "Киллрейт (Убыв.)":
                    SortKillerStatsByKillerKillRateDescendingOrder();
                    break;
                case "Киллрейт (Возр.)":
                    SortKillerStatsByKillerKillRateAscendingOrder();
                    break;
                case "Количеству сыгранных игр (Убыв.)":
                    SortKillerStatsByKillerCountGameDescendingOrder();
                    break;
                case "Количеству сыгранных игр (Возр.)":
                    SortKillerStatsByKillerCountGameAscendingOrder();
                    break;
                case "Количеству выигранных игр (Убыв.)":
                    SortKillerStatsByKillerCountWinGameDescendingOrder();
                    break;
                case "Количеству выигранных игр (Возр.)":
                    SortKillerStatsByKillerCountWinGameAscendingOrder();
                    break;
            }
        }

        private void SearchKillerName()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.Where(x => x.KillerName.ToLower().Contains(SearchTextBox.ToLower())))
            {
                KillerStatSortedList.Add(item);
            } 
        }

        private void SortKillerStatsByDescendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList)
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByAscendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.Reverse())
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByKillerNameDescendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.OrderBy(ks => ks.KillerName))
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByKillerNameAscendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.OrderByDescending(ks => ks.KillerName))
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByKillerPickRateAscendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.OrderBy(ks => ks.KillerPickRate))
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByKillerPickRateDescendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.OrderByDescending(ks => ks.KillerPickRate))
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByKillerWinRateAscendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.OrderBy(ks => ks.KillerWinRate))
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByKillerWinRateDescendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.OrderByDescending(ks => ks.KillerWinRate))
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByKillerKillRateAscendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.OrderBy(ks => ks.KillerKillRate))
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByKillerKillRateDescendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.OrderByDescending(ks => ks.KillerKillRate))
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByKillerCountGameAscendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.OrderBy(ks => ks.KillerCountGame))
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByKillerCountGameDescendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.OrderByDescending(ks => ks.KillerCountGame))
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByKillerCountWinGameAscendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.OrderBy(ks => ks.KillerMatchWin))
            {
                KillerStatSortedList.Add(item);
            }
        }

        private void SortKillerStatsByKillerCountWinGameDescendingOrder()
        {
            KillerStatSortedList.Clear();
            foreach (var item in KillerStatList.OrderByDescending(ks => ks.KillerMatchWin))
            {
                KillerStatSortedList.Add(item);
            }
        }
        #endregion

        #region Метод подсчета дополнительное статистики

        private void ShowDetailsKiller(object parameter)
        {
            if (parameter is KillerStat CurrentKiller)
            {
                SelectedKiller = CurrentKiller;

                KillerListVisibility = Visibility.Collapsed;
                SortMenuVisibility = Visibility.Collapsed;
                DetailedInformationVisibility = Visibility.Visible;


                _killerPage.MainScrollViewer.ScrollToTop();
            }
        }

        private async void GetAdditionalStatistics()
        {
            await GetMatches();
            var tasks = new List<Task>
            {
                GetStatTimeMatchAsync(),
                AverageScoreAsync(),
                KillerCountMatchAsync(),
                KillerActivityByHours(),
                KillerKillRateAsync(),
                KillerWinRateAsync(),

                CountPlayersPlatformsAsync(),
                CountPlayersBotAsync(),
                CountPlayersAnonymousAsync(),

                CountSurvivorsKilledAsync(),

                CountHookPercentageAsync(),
                CountNumberRecentGeneratorsPercentageAsync(),
                CountTypeTypeDeathSurvivorAsync(),

                CountMatchPlayedOnMapAsync(),
            };

            await Task.WhenAll(tasks);
        }

        #endregion

        #region Доп статистика

        private readonly List<GameStatistic> Matches = [];

        private async Task GetMatches()
        {
            Matches.Clear();
            Matches.AddRange(await _killerCalculationService.GetMatchForKillerAsync(SelectedKiller.KillerID, SelectedPlayerAssociationStatItem.IdPlayerAssociation));
        }

        #region Статистика в виде списка

        private string _shortestMatch;
        public string ShortestMatch
        {
            get => _shortestMatch;
            set
            {
                _shortestMatch = value;
                OnPropertyChanged();
            }
        }

        private string _fastestMatch;
        public string FastestMatch
        {
            get => _fastestMatch;
            set
            {
                _fastestMatch = value;
                OnPropertyChanged();
            }
        }

        private string _avgTimeMatch;
        public string AVGMatchMatch
        {
            get => _avgTimeMatch;
            set
            {
                _avgTimeMatch = value;
                OnPropertyChanged();
            }
        }

        // TODO: Убрать Using
        private async Task GetStatTimeMatchAsync()
        {
            await Task.Run(async () =>
            {
                var matches = await _dataService.GetAllDataInListAsync<GameStatistic>(x => x
                       .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID &&
                                   x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation));

                if (matches.Count == 0)
                    return;
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    TimeSpan[] timeSpans = context.GameStatistics
                               .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                                    .Select(s => TimeSpan.Parse(s.GameTimeMatch)).ToArray();

                    ShortestMatch = timeSpans.Max().ToString();
                    FastestMatch = timeSpans.Min().ToString();

                    TimeSpan totalTime = TimeSpan.Zero;
                    foreach (var timeSpan in timeSpans)
                    {
                        totalTime += timeSpan;
                    }

                    TimeSpan averageMatchTime = TimeSpan.FromTicks(totalTime.Ticks / timeSpans.Length);
                    averageMatchTime = new TimeSpan(averageMatchTime.Hours, averageMatchTime.Minutes, averageMatchTime.Seconds);
                    AVGMatchMatch = averageMatchTime.ToString();
                } 
            });
        }

        #endregion

        #region Средний счет очков

        public ObservableCollection<KillerAverageScoreTracker> KillerAVGScore { get; set; } = [];

        private async Task AverageScoreAsync()
        {
            KillerAVGScore.Clear();
            List<KillerAverageScoreTracker> avg = await _killerCalculationService.AverageKillerScoreAsync(Matches, TypeTime.Month);

            foreach (var item in avg)
            {
                KillerAVGScore.Add(item);
            }
        }

        #endregion

        #region Количество сыгранных матчей

        public ObservableCollection<CountMatchTracker> KillerCountMatch { get; set; } = [];

        private async Task KillerCountMatchAsync()
        {
            KillerCountMatch.Clear();
            List<CountMatchTracker> countMatch = await _killerCalculationService.CountMatchAsync(Matches, TypeTime.Month);

            foreach (var item in countMatch)
            {
                KillerCountMatch.Add(item);
            }
        }

        #endregion

        #region KillRate

        public ObservableCollection<KillerKillRateTracker> KillerKillRate { get; set; } = [];

        private async Task KillerKillRateAsync()
        {
            KillerKillRate.Clear();
            List<KillerKillRateTracker> killRate = await _killerCalculationService.KillRateAsync(Matches, TypeTime.Month);

            foreach (var item in killRate)
            {
                KillerKillRate.Add(item);
            }
        }

        #endregion

        #region WinRate 

        public ObservableCollection<KillerWinRateTracker> KillerWinRate { get; set; } = [];

        private async Task KillerWinRateAsync()
        {
            KillerWinRate.Clear();
            List<KillerWinRateTracker> killRate = await _killerCalculationService.WinRateAsync(Matches, TypeTime.Month);

            foreach (var item in killRate)
            {
                KillerWinRate.Add(item);
            }
        }

        #endregion

        #region Количество матчей по часам (Активность по часам)

        public ObservableCollection<ActivityByHoursTracker> ActivityByHours { get; set; } = [];

        private async Task KillerActivityByHours()
        {
            ActivityByHours.Clear();
            List<ActivityByHoursTracker> activityByHours = await _killerCalculationService.ActivityByHourAsync(Matches);

            foreach (var item in activityByHours)
            {
                ActivityByHours.Add(item);
            }
        }

        #endregion

        #region Количество игроков по платформам

        public ObservableCollection<PlayerPlatformTracker> PlayerPlatformTrackers { get; set; } = [];

        private async Task CountPlayersPlatformsAsync()
        {
            PlayerPlatformTrackers.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                List<PlayerPlatformTracker> platformTrackers = await _killerCalculationService.CalculatingPlayersByPlatformsAsync(Matches);

                foreach (var item in platformTrackers)
                {
                    PlayerPlatformTrackers.Add(item);
                }
            }        
        }

        #endregion

        #region % ливающих игроков

        public ObservableCollection<SurvivorBotTracker> SurvivorBotTracker { get; set; } = [];

        private async Task CountPlayersBotAsync()
        {
            SurvivorBotTracker.Clear();
            List<SurvivorBotTracker> survivorBotTrackers = await _killerCalculationService.CalculatingPlayersBotAsync(Matches);

            foreach (var item in survivorBotTrackers)
            {
                SurvivorBotTracker.Add(item);
            }
        }

        #endregion

        #region % Анонимных игроков

        public ObservableCollection<SurvivorAnonymousTracker> SurvivorAnonymousTracker { get; set; } = [];

        private async Task CountPlayersAnonymousAsync()
        {
            SurvivorAnonymousTracker.Clear();
            List<SurvivorAnonymousTracker> survivorAnonymousTrackers = await _killerCalculationService.CalculatingAnonymousPlayerAsync(Matches);

            foreach (var item in survivorAnonymousTrackers)
            {
                SurvivorAnonymousTracker.Add(item);
            }
        }

        #endregion

        // TODO: Решить оставлять ли этот расчет на этой странички, либо перенести в профиль.
        #region Частота встречамости Выживших

        private List<SurvivorDeathTracker> _survivorDeathTracker { get; set; } = [];

        public ObservableCollection<SurvivorDeathTracker> SurvivorDeaths { get; set; } = [];

        private async Task CountSurvivorsKilledAsync()
        {
            SurvivorDeaths.Clear();
            _survivorDeathTracker.Clear();

            await Task.Run(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var survivors = context.Survivors.Skip(1).ToList();

                    foreach (var surv in survivors)
                    {

                        int OneSurvivorDeathHook = context.GameStatistics
                         .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                         .Where(x => x.IdSurvivors1Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors1Navigation.IdTypeDeath == 1)
                         .Count();

                        int TwoSurvivorDeathHook = context.GameStatistics
                         .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                         .Where(x => x.IdSurvivors2Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors2Navigation.IdTypeDeath == 1)
                         .Count();

                        int ThreeSurvivorDeathHook = context.GameStatistics
                         .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                         .Where(x => x.IdSurvivors3Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors3Navigation.IdTypeDeath == 1)
                         .Count();

                        int FourSurvivorDeathHook = context.GameStatistics
                         .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                         .Where(x => x.IdSurvivors4Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors4Navigation.IdTypeDeath == 1)
                         .Count();

                        int SurvivorDeathHook = OneSurvivorDeathHook + TwoSurvivorDeathHook + ThreeSurvivorDeathHook + FourSurvivorDeathHook;


                        int OneSurvivorDeathGround = context.GameStatistics
                       .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                       .Where(x => x.IdSurvivors1Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors1Navigation.IdTypeDeath == 2)
                       .Count();

                        int TwoSurvivorDeathGround = context.GameStatistics
                        .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(x => x.IdSurvivors2Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors2Navigation.IdTypeDeath == 2)
                        .Count();

                        int ThreeSurvivorDeathGround = context.GameStatistics
                        .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(x => x.IdSurvivors3Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors3Navigation.IdTypeDeath == 2)
                        .Count();

                        int FourSurvivorDeathGround = context.GameStatistics
                        .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(x => x.IdSurvivors4Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors4Navigation.IdTypeDeath == 2)
                        .Count();

                        int SurvivorDeathGround = OneSurvivorDeathGround + TwoSurvivorDeathGround + ThreeSurvivorDeathGround + FourSurvivorDeathGround;


                        int OneSurvivorDeathMemento = context.GameStatistics
                       .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                       .Where(x => x.IdSurvivors1Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors1Navigation.IdTypeDeath == 3)
                       .Count();

                        int TwoSurvivorDeathMemento = context.GameStatistics
                        .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(x => x.IdSurvivors2Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors2Navigation.IdTypeDeath == 3)
                        .Count();

                        int ThreeSurvivorDeathMemento = context.GameStatistics
                        .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(x => x.IdSurvivors3Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors3Navigation.IdTypeDeath == 3)
                        .Count();

                        int FourSurvivorDeathMemento = context.GameStatistics
                        .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(x => x.IdSurvivors4Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors4Navigation.IdTypeDeath == 3)
                        .Count();

                        int SurvivorDeathMemento = OneSurvivorDeathMemento + TwoSurvivorDeathMemento + ThreeSurvivorDeathMemento + FourSurvivorDeathMemento;


                        int OneSurvivorDeathKillersAbility = context.GameStatistics
                       .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                       .Where(x => x.IdSurvivors1Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors1Navigation.IdTypeDeath == 4)
                       .Count();

                        int TwoSurvivorDeathKillersAbility = context.GameStatistics
                        .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(x => x.IdSurvivors2Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors2Navigation.IdTypeDeath == 4)
                        .Count();

                        int ThreeSurvivorDeathKillersAbility = context.GameStatistics
                        .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(x => x.IdSurvivors3Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors3Navigation.IdTypeDeath == 4)
                        .Count();

                        int FourSurvivorDeathKillersAbility = context.GameStatistics
                        .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(x => x.IdSurvivors4Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors4Navigation.IdTypeDeath == 4)
                        .Count();

                        int SurvivorDeathKillersAbility = OneSurvivorDeathKillersAbility + TwoSurvivorDeathKillersAbility + ThreeSurvivorDeathKillersAbility + FourSurvivorDeathKillersAbility;


                        int OneSurvivorEscaped = context.GameStatistics
                       .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                       .Where(x => x.IdSurvivors1Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors1Navigation.IdTypeDeath == 5)
                       .Count();

                        int TwoSurvivorEscaped = context.GameStatistics
                        .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(x => x.IdSurvivors2Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors2Navigation.IdTypeDeath == 5)
                        .Count();

                        int ThreeSurvivorEscaped = context.GameStatistics
                        .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(x => x.IdSurvivors3Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors3Navigation.IdTypeDeath == 5)
                        .Count();

                        int FourSurvivorEscaped = context.GameStatistics
                        .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(x => x.IdSurvivors4Navigation.IdSurvivor == surv.IdSurvivor && x.IdSurvivors4Navigation.IdTypeDeath == 5)
                        .Count();

                        int SurvivorEscaped = OneSurvivorEscaped + TwoSurvivorEscaped + ThreeSurvivorEscaped + FourSurvivorEscaped;

                        int TotalDeath = SurvivorDeathHook + SurvivorDeathGround + SurvivorDeathMemento + SurvivorDeathKillersAbility;

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var survivor = new SurvivorDeathTracker()
                            {
                                SurvivorImage = surv.SurvivorImage,
                                SurvivorName = surv.SurvivorName,
                                CountDeathHook = SurvivorDeathHook,
                                CountDeathGround = SurvivorDeathGround,
                                CountDeathMemento = SurvivorDeathMemento,
                                CountDeathKillersAbility = SurvivorDeathKillersAbility,
                                CountEscaped = SurvivorEscaped,
                                TotalDead = TotalDeath
                            };
                            _survivorDeathTracker.Add(survivor);
                        });
                    }
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var item in _survivorDeathTracker.OrderByDescending(x => x.TotalDead))
                        {
                            SurvivorDeaths.Add(item);
                        }
                    });
                }
            });
        }

        #endregion

        #region % Повесов (0-12)

        public ObservableCollection<KillerHooksTracker> KillerHooks { get; set; } = [];

        private async Task CountHookPercentageAsync()
        {
            KillerHooks.Clear();
            List<KillerHooksTracker> killerHooksTrackers = await _killerCalculationService.CalculatingCountHooksAsync(Matches);

            foreach (var item in killerHooksTrackers)
            {
                KillerHooks.Add(item);
            }
        }

        #endregion

        #region % Оставшихся генераторов (0-5)

        public ObservableCollection<RecentGeneratorsTracker> RecentGenerators { get; set; } = [];

        private async Task CountNumberRecentGeneratorsPercentageAsync()
        {
            RecentGenerators.Clear();
            List<RecentGeneratorsTracker> recentGeneratorsTrackers = await _killerCalculationService.CalculatingRecentGeneratorsAsync(Matches);

            foreach (var item in recentGeneratorsTrackers)
            {
                RecentGenerators.Add(item);
            }
        }

        #endregion

        #region Типы смертей выживший

        public ObservableCollection<SurvivorTypeDeathTracker> SurvivorTypeDeathTrackerList { get; set; } = [];

        private async Task CountTypeTypeDeathSurvivorAsync()
        {
            SurvivorTypeDeathTrackerList.Clear();
            List<SurvivorTypeDeathTracker> survivorTypeDeathTrackers = await _killerCalculationService.CalculatingTypeDeathSurvivorAsync(Matches);

            foreach (var item in survivorTypeDeathTrackers)
            {
                SurvivorTypeDeathTrackerList.Add(item);
            }
        }

        #endregion

        #region Количество сыгранных матчей на картах ( W\R, K\R, без подношений, с подношениями)

        public ObservableCollection<MapStat> MapStatList { get; set; } = [];

        private async Task CountMatchPlayedOnMapAsync()
        {
            MapStatList.Clear();
            List<MapStat> mapStats = await _mapCalculationService.CalculatingMapStatAsync(Matches);
            
            foreach (var item in mapStats)
            {
                MapStatList.Add(item);
            }
        }

        #endregion

        #endregion
    }
}