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
using MasterAnalyticsDeadByDaylight.Utils.Calculation;
using Microsoft.EntityFrameworkCore;

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

        public KillerPageViewModel(KillerPage KillerPage,
                                   IDataService dataService)
        {
            _killerPage = KillerPage;
            _dataService = dataService;

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
            List<Killer> KillerList = await _dataService.GetAllDataInListAsync<Killer>(x => x.Skip(1));

            int CountMatch = _dataService.Count<GameStatistic>(x => x.Where(x => x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation));

            foreach (var killer in KillerList)
            {
                List<GameStatistic> matches = await _dataService.GetAllDataInListAsync<GameStatistic>(x => x
                                               .Where(gs => gs.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                                                    .Where(gs => gs.IdKillerNavigation.IdKillerNavigation.IdKiller == killer.IdKiller));
                double CountKill = await CalculationKiller.CountKillAsync(matches);
                //double KillRate = await _killerCalculationService.CalculatingKillRate(GameStat, CountKill);
                //double KillRatePercentage = await _killerCalculationService.CalculatingKillRatePercentage(KillRate); 
                double KillRate = await CalculationKiller.AVGKillRateAsync(matches);
                double KillRatePercentage = await CalculationKiller.AVGKillRatePercentageAsync(matches);
                double PickRate = await CalculationKiller.PickRateAsync(matches.Count, CountMatch);
                double MatchWin = await CalculationKiller.CountMatchWinAsync(matches);
                double WinRate = await CalculationKiller.WinRateAsync((int)MatchWin, matches.Count);

                var KillDistribution = await CalculationKiller.KillDistributionAsync(matches);

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
                KillerStatList.Add(killerStat);
            }
            SortKillerStatList();
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
            Matches.AddRange(await _dataService.GetAllDataInListAsync<GameStatistic>(x => x
                    .Include(x => x.IdMapNavigation).ThenInclude(x => x.IdMeasurementNavigation)
                        .Include(x => x.IdKillerNavigation)
                            .Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdAssociationNavigation)
                                .Include(x => x.IdSurvivors1Navigation)
                                    .Include(x => x.IdSurvivors2Navigation)
                                        .Include(x => x.IdSurvivors3Navigation)
                                            .Include(x => x.IdSurvivors4Navigation)
                    .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)));
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

        private async Task GetStatTimeMatchAsync()
        {
            var (Shortest, Fastest, AVG) = await CalculationTime.StatTimeMatchAsync(Matches, SelectedKiller.KillerID, SelectedPlayerAssociationStatItem.IdPlayerAssociation);
            ShortestMatch = Shortest;
            FastestMatch = Fastest;
            AVGMatchMatch = AVG;
        }

        #endregion

        #region Средний счет очков

        public ObservableCollection<KillerAverageScoreTracker> KillerAVGScore { get; set; } = [];

        private async Task AverageScoreAsync()
        {
            KillerAVGScore.Clear();
            List<KillerAverageScoreTracker> avg = await CalculationKiller.AverageScoreForPeriodTimeAsyncAsync(Matches, TypeTime.Month);

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
            List<CountMatchTracker> countMatch = await CalculationGeneral.CountMatchForPeriodTimeAsync(Matches, TypeTime.Month);

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
            List<KillerKillRateTracker> killRate = await CalculationKiller.KillRateForPeriodTimeAsync(Matches, TypeTime.Month);

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
            List<KillerWinRateTracker> killRate = await CalculationKiller.WinRateForPeriodTimeAsync(Matches, TypeTime.Month);

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
            List<ActivityByHoursTracker> activityByHours = await CalculationGeneral.CountMatchesPlayedInEachHourAsync(Matches);

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
                List<PlayerPlatformTracker> platformTrackers = await CalculationSurvivor.PlayersByPlatformsAsync(Matches);

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
            List<SurvivorBotTracker> survivorBotTrackers = await CalculationSurvivor.PlayersBotAsync(Matches);

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
            List<SurvivorAnonymousTracker> survivorAnonymousTrackers = await CalculationSurvivor.AnonymousPlayersAsync(Matches);

            foreach (var item in survivorAnonymousTrackers)
            {
                SurvivorAnonymousTracker.Add(item);
            }
        }

        #endregion

        #region % Повесов (0-12)

        public ObservableCollection<KillerHooksTracker> KillerHooks { get; set; } = [];

        private async Task CountHookPercentageAsync()
        {
            KillerHooks.Clear();
            List<KillerHooksTracker> killerHooksTrackers = await CalculationKiller.CountHooksAsync(Matches);

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
            List<RecentGeneratorsTracker> recentGeneratorsTrackers = await CalculationGeneral.RecentGeneratorsAsync(Matches);

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
            List<SurvivorTypeDeathTracker> survivorTypeDeathTrackers = await CalculationSurvivor.TypeDeathSurvivorsAsync(Matches);

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
            //MapStatList.Clear();
            //List<MapStat> mapStats = await _mapCalculationService.CalculatingMapStatAsync(Matches);

            //foreach (var item in mapStats)
            //{
            //    MapStatList.Add(item);
            //}
        }

        #endregion

        #endregion
    }
}