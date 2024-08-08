using LiveCharts.Wpf;
using LiveCharts;
using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Numerics;
using MasterAnalyticsDeadByDaylight.MVVM.Model.ChartModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    internal class KillerPageViewModel : BaseViewModel
    {

        #region Колекции 

        private ObservableCollection<KillerStat> KillerStatList { get; set; }

        public ObservableCollection<KillerStat> KillerStatSortedList { get; set; }

        public ObservableCollection<GameStatistic> MatchesSelectedKillerList { get; set; }

        public ObservableCollection<PlayerAssociation> PlayerAssociationList { get; set; }

        public ObservableCollection<Survivor> SurvivorList { get; set; } = [];

        public ObservableCollection<TypeDeath> TypeDeathList { get; set; } = [];

        public ObservableCollection<Map> MapList { get; set; } = [];

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

        public ObservableCollection<string> Names { get; set; } = ["Ноль", "Один", "Два", "Три", "Четыре", "Пять", "Шесть", "Семь", "Восемь", "Девять", "Десять", "Одиннадцать", "Двенадцать"];
        #endregion

        #region Свойства Selected

        private string _selectedKillerStatSortItem;
        public string SelectedKillerStatSortItem
        {
            get => _selectedKillerStatSortItem;
            set
            {
                _selectedKillerStatSortItem = value;
                GetKillerStatisticData();
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
                GetKillerStatisticData();
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

        public KillerPageViewModel()
        {
            DeclareCollections();
            GetPlayerAssociationData();
            GetSurvivorData();
            GetTypeDeathData();
            GetMapData();
            SelectedPlayerAssociationStatItem = PlayerAssociationList.First();
            SetDefaultVisibility();
            SelectedKillerStatSortItem = SortingList.First();
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
        }); }

        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { GetKillerStatisticData(); SortKillerStatsByDescendingOrder(); SearchTextBox = string.Empty; }); }

        #endregion

        #region Объявление списков

        private void DeclareCollections()
        {
            KillerStatList = [];
            KillerStatSortedList = [];
            PlayerAssociationList = [];
        }

        #endregion

        #region Методы получение данных

        private void GetKillerStatisticData()
        {
            KillerStatList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                List<Killer> KillerList = context.Killers.Skip(1).ToList();

                int CountMatch = context.GameStatistics
                    .Include(gs => gs.IdKillerNavigation)
                    .ThenInclude(killerInfo => killerInfo.IdAssociationNavigation)
                    .Where(gs => gs.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                    .Count();

                foreach (var killer in KillerList)
                {
                    List<GameStatistic> GameStat = context.GameStatistics
                        .Include(gs => gs.IdKillerNavigation)
                        .ThenInclude(killerInfo => killerInfo.IdKillerNavigation)

                        .Include(gs => gs.IdKillerNavigation)
                        .ThenInclude(killerInfo => killerInfo.IdAssociationNavigation)

                        .Where(gs => gs.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation)
                        .Where(gs => gs.IdKillerNavigation.IdKillerNavigation.IdKiller == killer.IdKiller)
                        .ToList();

                    double CountKill = 0;
                    double KillRate;
                    double KillRatePercentage;
                    foreach (var item in GameStat) { CountKill += item.CountKills; }

                    if (GameStat.Count == 0) { KillRate = 0; }
                    else { KillRate = Math.Round(CountKill / GameStat.Count, 1); }

                    KillRatePercentage = Math.Round(KillRate / 4 * 100, 2);

                    double PickRate = Math.Round((double)GameStat.Count / CountMatch * 100, 2);
                    double MatchWin = GameStat.Where(gs => gs.CountKills == 3 | gs.CountKills == 4).Count();
                    double WinRate = Math.Round(MatchWin / GameStat.Count * 100, 2);

                    double KillingZero = Math.Round((double)GameStat.Where(gs => gs.CountKills == 0).Count() / GameStat.Count * 100, 2);
                    double KillingOne = Math.Round((double)GameStat.Where(gs => gs.CountKills == 1).Count() / GameStat.Count * 100, 2);
                    double KillingTwo = Math.Round((double)GameStat.Where(gs => gs.CountKills == 2).Count() / GameStat.Count * 100, 2);
                    double KillingThree = Math.Round((double)GameStat.Where(gs => gs.CountKills == 3).Count() / GameStat.Count * 100, 2);
                    double KillingFour = Math.Round((double)GameStat.Where(gs => gs.CountKills == 4).Count() / GameStat.Count * 100, 2);

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

        private void GetPlayerAssociationData()
        {
            PlayerAssociationList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var association = context.PlayerAssociations.Where(pa => pa.IdPlayerAssociation == 1 || pa.IdPlayerAssociation == 3).ToList();
                foreach (var item in association)
                {
                    PlayerAssociationList.Add(item);
                }
            }
        }

        private void GetSurvivorData()
        {
            SurvivorList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var survivors = context.Survivors.ToList();
                foreach (var item in survivors)
                {
                    SurvivorList.Add(item);
                }
            }
        }

        private void GetTypeDeathData()
        {
            TypeDeathList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var typeDeaths = context.TypeDeaths.ToList();

                foreach (var item in typeDeaths)
                {
                    TypeDeathList.Add(item);
                }
            }
        }

        private void GetMapData()
        {
            MapList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var maps = context.Maps.Include(x => x.IdMeasurementNavigation).OrderBy(x => x.IdMeasurement).ToList();

                foreach (var item in maps)
                {
                    MapList.Add(item);
                }
            }
        }

        #endregion

        #region Методы видимости элементов

        private void ShowDetailsKiller(object parameter)
        {
            if (parameter is KillerStat CurrentKiller)
            {
                KillerListVisibility = Visibility.Collapsed;
                SortMenuVisibility = Visibility.Collapsed;
                DetailedInformationVisibility = Visibility.Visible;

                SelectedKiller = CurrentKiller;
                GetDateChart();
            }
        }

        private void SetDefaultVisibility()
        {
            SortMenuVisibility = Visibility.Visible;
            DetailedInformationVisibility = Visibility.Collapsed;
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
            foreach (var item in KillerStatList.Where(ks => ks.KillerName.ToLower().Contains(SearchTextBox.ToLower())))
            {
                KillerStatSortedList.Add(item);
            }

            //if (SearchTextBox == string.Empty)
            //{
            //    SortKillerStatList();
            //} 
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

        private async void GetAdditionalStatistics()
        {
            TypeTime = "Месяц";
            await CountSurvivorsKilledAsync();
            await CountHookPercentageAsync();
            await CountNumberRecentGeneratorsPercentageAsync();
            await CountTypeTypeDeathSurvivorAsync();
            await CountMatchPlayedOnMapAsync();
            await CountPlayersPlatformsAsync();
            await CountPlayersBotAsync();
            await CountPlayersAnonymousAsync();
            await AverageScoreAsync();
            await KillerCountMatchAsync();
            await KillerKillRateAsync();
            await KillerWinRateAsync();
            await GetStatTimeMatchAsync();
        }

        #endregion

        public string _typeTime;
        public string TypeTime
        {
            get => _typeTime;
            set
            {
                _typeTime = value;
                OnPropertyChanged();
            }
        }

        #region Граффик

        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged();
            }
        }

        private string[] _labels;
        public string[] Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                OnPropertyChanged();
            }
        }

        private Func<double, string> _yFormatter;
        public Func<double, string> YFormatter
        {
            get => _yFormatter;
            set
            {
                _yFormatter = value;
                OnPropertyChanged();
            }
        }

        private void GetDateChart()
        {

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Матчей",
                    Values = new ChartValues<double>(KillerStatList.Select(x => x.KillerCountGame)),
                    LabelsPosition = 0,

                },
                new ColumnSeries
                {
                    Title = "Килрейт",
                    Values = new ChartValues<double>(KillerStatList.Select(x => x.KillerKillRate)),
                    LabelsPosition = 0,

                }
            };

            Labels = KillerStatList.Select(x => x.KillerName).ToArray();
            YFormatter = value => value.ToString();
        }

        #endregion

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
            await Task.Run(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var matches = context.GameStatistics.Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation).ToList();

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
            await Task.Run(() =>
            {
                
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var matches = context.GameStatistics.Include(x => x.IdKillerNavigation).Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdAssociationNavigation)
                    .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation).ToList();

                    //GameStatistic FirstMatch;
                    //GameStatistic LastMatch;

                    //DateTime FirstMathDate;
                    //DateTime LastMathDate;
                    //if (matches.Count == 0)
                    //{
                    //    return;
                    //}
                    //else
                    //{
                    //    FirstMatch = matches.First();
                    //    LastMatch = matches.Last();

                    //    FirstMathDate = FirstMatch.DateTimeMatch.Value;
                    //    LastMathDate = LastMatch.DateTimeMatch.Value;
                    //}

                    var FirstMatch = matches.FirstOrDefault();
                    DateTime FirstMathDate = FirstMatch.DateTimeMatch.Value;

                    var LastMatch = matches.LastOrDefault();
                    DateTime LastMathDate = LastMatch.DateTimeMatch.Value;

                    if (TypeTime == "День")
                    {
                        for (DateTime date = FirstMathDate; date <= LastMathDate; date = date.AddDays(1))
                        {
                            //var matchByDate = matches.Where(x => DateOnly.FromDateTime((DateTime)x.DateTimeMatch) == DateOnly.FromDateTime(date)).ToList(); // Так же рабочий вариант
                            var matchByDate = matches
                            .Where(x => x.DateTimeMatch.Value.Year == date.Year)
                            .Where(x => x.DateTimeMatch.Value.Month == date.Month)
                            .Where(x => x.DateTimeMatch.Value.Day == date.Day).ToList();

                            int Account = 0;
                            foreach (var item in matchByDate)
                            {
                                Account += item.IdKillerNavigation.KillerAccount;
                            }

                            double avg = Account == 0 ? 0 : Math.Round(Account / (double)matchByDate.Count, 0);

                            var KillerScore = new KillerAverageScoreTracker
                            {
                                AvgScore = avg,
                                DateTime = date.ToString("D"),
                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                KillerAVGScore.Add(KillerScore);
                            });
                        }
                    }
                    if (TypeTime == "Месяц")
                    {
                        for (DateTime date = FirstMathDate; date.Month <= LastMathDate.Month; date = date.AddMonths(1))
                        {
                            //var matchByDate = matches.Where(x => x.DateTimeMatch.Value.Month == date.Month).ToList();
                            var matchByDate = matches
                            .Where(x => x.DateTimeMatch.Value.Year == date.Year)
                            .Where(x => x.DateTimeMatch.Value.Month == date.Month).ToList();

                            int Account = 0;

                            foreach (var item in matchByDate)
                            {
                                Account += item.IdKillerNavigation.KillerAccount;
                            }

                            double avg = Account == 0 ? 0 : Math.Round(Account / (double)matchByDate.Count, 0);

                            var KillerScore = new KillerAverageScoreTracker
                            {
                                AvgScore = avg,
                                DateTime = date.ToString("Y"),
                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                KillerAVGScore.Add(KillerScore);
                            });
                        }
                    }
                    if (TypeTime == "Год")
                    {
                        for (DateTime date = FirstMathDate; date <= LastMathDate; date = date.AddYears(1))
                        {
                            //var matchByDate = matches.Where(x => DateOnly.FromDateTime((DateTime)x.DateTimeMatch) == DateOnly.FromDateTime(date)).ToList();                            
                            var matchByDate = matches.Where(x => x.DateTimeMatch.Value.Year == date.Year).ToList();

                            int Account = 0;

                            foreach (var item in matchByDate)
                            {
                                Account += item.IdKillerNavigation.KillerAccount;
                            }

                            double avg = Account == 0 ? 0 : Math.Round(Account / (double)matchByDate.Count, 0);

                            var KillerScore = new KillerAverageScoreTracker
                            {
                                AvgScore = avg,
                                DateTime = date.ToString("yyy"),
                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                KillerAVGScore.Add(KillerScore);
                            });
                        }
                    }
                }
            });
        }

        #endregion

        #region Количество сыгранных матчей

        public ObservableCollection<KillerCountMatchTracker> KillerCountMatch { get; set; } = [];

        private async Task KillerCountMatchAsync()
        {
            KillerCountMatch.Clear();
            await Task.Run(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var matches = context.GameStatistics.Include(x => x.IdKillerNavigation).Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdAssociationNavigation)
                    .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation).ToList();

                    var FirstMatch = matches.First();
                    DateTime FirstMathDate = FirstMatch.DateTimeMatch.Value;

                    var LastMatch = matches.Last();
                    DateTime LastMathDate = LastMatch.DateTimeMatch.Value;

                    if (TypeTime == "День")
                    {
                        for (DateTime date = FirstMathDate; date <= LastMathDate; date = date.AddDays(1))
                        {
                            //var matchByDate = matches.Where(x => DateOnly.FromDateTime((DateTime)x.DateTimeMatch) == DateOnly.FromDateTime(date)).ToList();
                            var matchByDate = matches
                            .Where(x => x.DateTimeMatch.Value.Year == date.Year)
                            .Where(x => x.DateTimeMatch.Value.Month == date.Month)
                            .Where(x => x.DateTimeMatch.Value.Day == date.Day).ToList();
                            var CountMatch = new KillerCountMatchTracker
                            {
                                CountMatch = matchByDate.Count,
                                DateTime = date.ToString("D"),
                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                KillerCountMatch.Add(CountMatch);
                            });
                        }
                    }
                    if (TypeTime == "Месяц")
                    {
                        for (DateTime date = FirstMathDate; date.Month <= LastMathDate.Month; date = date.AddMonths(1))
                        {
                            /*var matchByDate = matches.Where(x => DateOnly.FromDateTime((DateTime)x.DateTimeMatch).Month == DateOnly.FromDateTime((DateTime)date).Month).ToList();*/ //Один из возможно рабочих вариантов
                            var matchByDate = matches
                            .Where(x => x.DateTimeMatch.Value.Year == date.Year)
                            .Where(x => x.DateTimeMatch.Value.Month == date.Month).ToList();
                            var CountMatch = new KillerCountMatchTracker
                            {
                                CountMatch = matchByDate.Count,
                                DateTime = date.ToString("Y"),
                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                KillerCountMatch.Add(CountMatch);
                            });
                        }
                    }
                    if (TypeTime == "Год")
                    {
                        for (DateTime date = FirstMathDate; date <= LastMathDate; date = date.AddYears(1))
                        {
                            //var matchByDate = matches.Where(x => DateOnly.FromDateTime((DateTime)x.DateTimeMatch) == DateOnly.FromDateTime(date)).ToList();
                            var matchByDate = matches.Where(x => x.DateTimeMatch.Value.Year == date.Year).ToList();

                            var CountMatch = new KillerCountMatchTracker
                            {
                                CountMatch = matchByDate.Count,
                                DateTime = date.ToString("yyy"),
                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                KillerCountMatch.Add(CountMatch);
                            });
                        }
                    }
                }
            });
        }

        #endregion

        #region KillRate

        public ObservableCollection<KillerKillRateTracker> KillerKillRate { get; set; } = [];

        private async Task KillerKillRateAsync()
        {
            KillerKillRate.Clear();
            await Task.Run(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var matches = context.GameStatistics.Include(x => x.IdKillerNavigation).Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdAssociationNavigation)
                    .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation).ToList();

                    var FirstMatch = matches.First();
                    DateTime FirstMathDate = FirstMatch.DateTimeMatch.Value;

                    var LastMatch = matches.Last();
                    DateTime LastMathDate = LastMatch.DateTimeMatch.Value;

                    if (TypeTime == "День")
                    {
                        for (DateTime date = FirstMathDate; date <= LastMathDate; date = date.AddDays(1))
                        {
                            //var matchByDate = matches.Where(x => DateOnly.FromDateTime((DateTime)x.DateTimeMatch) == DateOnly.FromDateTime(date)).ToList();
                            var matchByDate = matches
                            .Where(x => x.DateTimeMatch.Value.Year == date.Year)
                            .Where(x => x.DateTimeMatch.Value.Month == date.Month)
                            .Where(x => x.DateTimeMatch.Value.Day == date.Day).ToList();

                            double CountKill = 0;
                            double KillRate;

                            foreach (var item in matchByDate) 
                            { 
                                CountKill += item.CountKills; 
                            }

                            if (matchByDate.Count == 0) 
                            { 
                                KillRate = 0;
                            }
                            else 
                            { 
                                KillRate = Math.Round(CountKill / matchByDate.Count, 1); 
                            }

                            var CountMatch = new KillerKillRateTracker
                            {
                                KillRate = KillRate,
                                DateTime = date.ToString("D"),
                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                KillerKillRate.Add(CountMatch);
                            });
                        }
                    }
                    if (TypeTime == "Месяц")
                    {
                        for (DateTime date = FirstMathDate; date.Month <= LastMathDate.Month; date = date.AddMonths(1))
                        {
                            /*var matchByDate = matches.Where(x => DateOnly.FromDateTime((DateTime)x.DateTimeMatch).Month == DateOnly.FromDateTime((DateTime)date).Month).ToList();*/ //Один из возможно рабочих вариантов
                            var matchByDate = matches
                            .Where(x => x.DateTimeMatch.Value.Year == date.Year)
                            .Where(x => x.DateTimeMatch.Value.Month == date.Month).ToList();

                            double CountKill = 0;
                            double KillRate = 0;

                            foreach (var item in matchByDate)
                            {
                                CountKill += item.CountKills;
                            }
                            if (matchByDate.Count == 0)
                            {
                                KillRate = 0;
                            }
                            else
                            {
                                KillRate = Math.Round(CountKill / matchByDate.Count, 1);
                            }

                            var KillRateMatch = new KillerKillRateTracker
                            {
                                KillRate = KillRate,
                                DateTime = date.ToString("Y"),
                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                KillerKillRate.Add(KillRateMatch);
                            });
                        }
                    }
                    if (TypeTime == "Год")
                    {
                        for (DateTime date = FirstMathDate; date <= LastMathDate; date = date.AddYears(1))
                        {
                            //var matchByDate = matches.Where(x => DateOnly.FromDateTime((DateTime)x.DateTimeMatch) == DateOnly.FromDateTime(date)).ToList();
                            var matchByDate = matches.Where(x => x.DateTimeMatch.Value.Year == date.Year).ToList();

                            double CountKill = 0;
                            double KillRate;

                            foreach (var item in matchByDate)
                            {
                                CountKill += item.CountKills;
                            }

                            if (matchByDate.Count == 0)
                            {
                                KillRate = 0;
                            }
                            else
                            {
                                KillRate = Math.Round(CountKill / matchByDate.Count, 1);
                            }

                            var CountMatch = new KillerKillRateTracker
                            {
                                KillRate = KillRate,
                                DateTime = date.ToString("D"),
                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                KillerKillRate.Add(CountMatch);
                            });
                        }
                    }
                }
            });
        }

        #endregion

        #region WinRate 

        public ObservableCollection<KillerWinRateTracker> KillerWinRate { get; set; } = [];

        private async Task KillerWinRateAsync()
        {
            KillerWinRate.Clear();
            await Task.Run(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var matches = context.GameStatistics.Include(x => x.IdKillerNavigation).Include(x => x.IdKillerNavigation).ThenInclude(x => x.IdAssociationNavigation)
                    .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation).ToList();

                    var FirstMatch = matches.First();
                    DateTime FirstMathDate = FirstMatch.DateTimeMatch.Value;

                    var LastMatch = matches.Last();
                    DateTime LastMathDate = LastMatch.DateTimeMatch.Value;

                    if (TypeTime == "День")
                    {
                        for (DateTime date = FirstMathDate; date <= LastMathDate; date = date.AddDays(1))
                        {
                            //var matchByDate = matches.Where(x => DateOnly.FromDateTime((DateTime)x.DateTimeMatch) == DateOnly.FromDateTime(date)).ToList();
                            var matchByDate = matches
                            .Where(x => x.DateTimeMatch.Value.Year == date.Year)
                            .Where(x => x.DateTimeMatch.Value.Month == date.Month)
                            .Where(x => x.DateTimeMatch.Value.Day == date.Day).ToList();

                            var MatchWin = matchByDate.Where(x => x.CountKills == 3 | x.CountKills == 4).Count();

                            double WinRate = Math.Round((double)MatchWin / matchByDate.Count * 100, 2);

                            var WinRateStat = new KillerWinRateTracker
                            {
                                WinRate = WinRate,
                                DateTime = date.ToString("D"),
                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                KillerWinRate.Add(WinRateStat);
                            });
                        }
                    }
                    if (TypeTime == "Месяц")
                    {
                        for (DateTime date = FirstMathDate; date.Month <= LastMathDate.Month; date = date.AddMonths(1))
                        {
                            /*var matchByDate = matches.Where(x => DateOnly.FromDateTime((DateTime)x.DateTimeMatch).Month == DateOnly.FromDateTime((DateTime)date).Month).ToList();*/ //Один из возможно рабочих вариантов
                            var matchByDate = matches
                            .Where(x => x.DateTimeMatch.Value.Year == date.Year)
                            .Where(x => x.DateTimeMatch.Value.Month == date.Month).ToList();

                            var MatchWin = matchByDate.Where(x => x.CountKills == 3 | x.CountKills == 4).Count();

                            double WinRate = Math.Round((double)MatchWin / matchByDate.Count * 100, 2);

                            var WinRateStat = new KillerWinRateTracker
                            {
                                WinRate = WinRate,
                                DateTime = date.ToString("Y"),
                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                KillerWinRate.Add(WinRateStat);
                            });
                        }
                    }
                    if (TypeTime == "Год")
                    {
                        for (DateTime date = FirstMathDate; date <= LastMathDate; date = date.AddYears(1))
                        {
                            //var matchByDate = matches.Where(x => DateOnly.FromDateTime((DateTime)x.DateTimeMatch) == DateOnly.FromDateTime(date)).ToList();
                            var matchByDate = matches.Where(x => x.DateTimeMatch.Value.Year == date.Year).ToList();

                            var MatchWin = matchByDate.Where(x => x.CountKills == 3 | x.CountKills == 4).Count();

                            double WinRate = Math.Round((double)MatchWin / matchByDate.Count * 100, 2);

                            var WinRateStat = new KillerWinRateTracker
                            {
                                WinRate = WinRate,
                                DateTime = date.ToString("D"),
                            };

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                KillerWinRate.Add(WinRateStat);
                            });
                        }
                    }
                }
            });
        }

        #endregion

        #region Количество игроков по платформам

        public ObservableCollection<PlayerPlatformTracker> PlayerPlatformTrackers { get; set; } = [];

        private async Task CountPlayersPlatformsAsync()
        {
            PlayerPlatformTrackers.Clear();
            await Task.Run(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var matches = context.GameStatistics.Include(x => x.IdSurvivors1Navigation).Include(x => x.IdSurvivors2Navigation).Include(x => x.IdSurvivors3Navigation).Include(x => x.IdSurvivors4Navigation)
                    .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation).ToList();

                    var platforms = context.Platforms.ToList();

                    foreach (var item in platforms)
                    {
                        int first = matches.Where(x => x.IdSurvivors1Navigation.IdPlatform == item.IdPlatform).Count();
                        int second = matches.Where(x => x.IdSurvivors2Navigation.IdPlatform == item.IdPlatform).Count();
                        int third = matches.Where(x => x.IdSurvivors3Navigation.IdPlatform == item.IdPlatform).Count();
                        int fourth = matches.Where(x => x.IdSurvivors4Navigation.IdPlatform == item.IdPlatform).Count();

                        int platformCount = first + second + third + fourth;

                        double platformPercentages = Math.Round((double)platformCount / (matches.Count * 4) * 100, 2);

                        var playerPlatform = new PlayerPlatformTracker
                        {
                            PlatformName = item.PlatformName,
                            PlayerCount = platformCount,
                            PlatformPercentages = platformPercentages
                        };

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            PlayerPlatformTrackers.Add(playerPlatform);
                        });
                    }
                }
            });
        }

        #endregion

        #region % ливающих игроков

        public ObservableCollection<SurvivorBotTracker> SurvivorBotTracker { get; set; } = [];

        private async Task CountPlayersBotAsync()
        {
            SurvivorBotTracker.Clear();
            await Task.Run(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var matches = context.GameStatistics.Include(x => x.IdSurvivors1Navigation).Include(x => x.IdSurvivors2Navigation).Include(x => x.IdSurvivors3Navigation).Include(x => x.IdSurvivors4Navigation)
                    .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation).ToList();

                    int firstBot = matches.Where(x => x.IdSurvivors1Navigation.Bot == true).Count();
                    int secondBot = matches.Where(x => x.IdSurvivors2Navigation.Bot == true).Count();
                    int thirdBot = matches.Where(x => x.IdSurvivors3Navigation.Bot == true).Count();
                    int fourthBot = matches.Where(x => x.IdSurvivors4Navigation.Bot == true).Count();

                    int playerBot = firstBot + secondBot + thirdBot + fourthBot;

                    double playerBotPercentages = Math.Round((double)playerBot / (matches.Count * 4) * 100, 2);

                    var botTracker = new SurvivorBotTracker
                    {
                        PlayerBot = playerBotPercentages,
                        CountPlayerBot = playerBot,
                        CountPlayer = matches.Count * 4,
                    };

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SurvivorBotTracker.Add(botTracker);
                    });
                }
            });
        }

        #endregion

        #region % Анонимных игроков

        public ObservableCollection<SurvivorAnonymousTracker> SurvivorAnonymousTracker { get; set; } = [];

        private async Task CountPlayersAnonymousAsync()
        {
            SurvivorAnonymousTracker.Clear();
            await Task.Run(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var matches = context.GameStatistics.Include(x => x.IdSurvivors1Navigation).Include(x => x.IdSurvivors2Navigation).Include(x => x.IdSurvivors3Navigation).Include(x => x.IdSurvivors4Navigation)
                    .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation).ToList();

                    int firstAnonymous = matches.Where(x => x.IdSurvivors1Navigation.AnonymousMode == true).Count();
                    int secondAnonymous = matches.Where(x => x.IdSurvivors2Navigation.AnonymousMode == true).Count();
                    int thirdAnonymous = matches.Where(x => x.IdSurvivors3Navigation.AnonymousMode == true).Count();
                    int fourthAnonymous = matches.Where(x => x.IdSurvivors4Navigation.AnonymousMode == true).Count();

                    int playerAnonymous = firstAnonymous + secondAnonymous + thirdAnonymous + fourthAnonymous;

                    double playerAnonymousPercentages = Math.Round((double)playerAnonymous / (matches.Count * 4) * 100, 2);

                    var anonymousTracker = new SurvivorAnonymousTracker
                    {
                        PlayerAnonymous = playerAnonymousPercentages,
                        CountPlayerAnonymous = playerAnonymous,
                        CountPlayer = matches.Count * 4,
                    };

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SurvivorAnonymousTracker.Add(anonymousTracker);
                    });
                }
            });
        }

        #endregion

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
            await Task.Run(() => 
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var matches = context.GameStatistics.Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation).ToList();

                    for (int i = 0; i <= 12; i++)
                    {
                        var killerHook = new KillerHooksTracker
                        {
                            CountHookName = Names[i],
                            CountHookPercentages = Math.Round((double)matches.Where(x => x.CountHooks == i).Count() / matches.Count * 100, 2),
                            CountGame = matches.Where(x => x.CountHooks == i).Count(),
                        };

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            KillerHooks.Add(killerHook);
                        });
                    }
                }
            });
        }

        #endregion

        #region % Оставшихся генераторов (0-5)

        public ObservableCollection<RecentGeneratorsTracker> RecentGenerators { get; set; } = [];

        private async Task CountNumberRecentGeneratorsPercentageAsync()
        {
            RecentGenerators.Clear();
            await Task.Run(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var matches = context.GameStatistics.Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation).ToList();

                    for (int i = 0; i <= 5; i++)
                    {
                        var recentGenerators = new RecentGeneratorsTracker
                        {
                            CountRecentGeneratorsName = Names[i],
                            CountRecentGeneratorsPercentages = Math.Round((double)matches.Where(x => x.NumberRecentGenerators == i).Count() / matches.Count * 100, 2),
                            CountGame = matches.Where(x => x.NumberRecentGenerators == i).Count(),
                        };

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            RecentGenerators.Add(recentGenerators);
                        });
                    }
                }
            });
        }

        #endregion

        #region Типы смертей выживший

        public ObservableCollection<SurvivorTypeDeathTracker> SurvivorTypeDeathTrackerList { get; set; } = [];

        private async Task CountTypeTypeDeathSurvivorAsync()
        {
            SurvivorTypeDeathTrackerList.Clear();
            await Task.Run(() => 
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var matches = context.GameStatistics.Include(x => x.IdSurvivors1Navigation).Include(x => x.IdSurvivors2Navigation).Include(x => x.IdSurvivors3Navigation).Include(x => x.IdSurvivors4Navigation)
                    .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation).ToList();

                    foreach (var item in TypeDeathList)
                    {
                        int first = matches.Where(x => x.IdSurvivors1Navigation.IdTypeDeath == item.IdTypeDeath).Count();
                        int second = matches.Where(x => x.IdSurvivors2Navigation.IdTypeDeath == item.IdTypeDeath).Count();
                        int third = matches.Where(x => x.IdSurvivors3Navigation.IdTypeDeath == item.IdTypeDeath).Count();
                        int fourth = matches.Where(x => x.IdSurvivors4Navigation.IdTypeDeath == item.IdTypeDeath).Count();

                        int countSurvivorDeath = first + second + third + fourth;

                        double countSurvivorTypeDeathPercentages = Math.Round((double)countSurvivorDeath / (matches.Count * 4) * 100, 2);

                        var SurvivorTypeDeath = new SurvivorTypeDeathTracker
                        {
                            TypeDeathName = item.TypeDeathName,
                            TypeDeathPercentages = countSurvivorTypeDeathPercentages,
                            CountGame = countSurvivorDeath
                        };

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            SurvivorTypeDeathTrackerList.Add(SurvivorTypeDeath);
                        });
                    } 
                }
            });
        }

        #endregion

        #region Количество сыгранных матчей на картах ( W\R, K\R, без подношений, с подношениями) (ДОДЕЛАТЬ)

        public ObservableCollection<MapStat> MapStatList { get; set; } = [];

        private async Task CountMatchPlayedOnMapAsync()
        {
            await Task.Run(() => 
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var matches = context.GameStatistics.Include(x => x.IdMapNavigation).ThenInclude(x => x.IdMeasurementNavigation)
                    .Where(x => x.IdKillerNavigation.IdKiller == SelectedKiller.KillerID && x.IdKillerNavigation.IdAssociation == SelectedPlayerAssociationStatItem.IdPlayerAssociation).ToList();

                    foreach (var item in MapList)
                    {
                        double countGameMap = matches.Where(x => x.IdMap == item.IdMap).Count();
                        double winRateMap = Math.Round((double)matches.Where(x => x.IdMap == item.IdMap).Where(x => x.CountKills == 3 | x.CountKills == 4).Count() / matches.Where(x => x.IdMap == item.IdMap).Count() * 100, 2);

                        if (double.IsNaN(winRateMap))
                        {
                            winRateMap = 0;
                        }

                        double pickRateMap = Math.Round((double)matches.Where(x => x.IdMap == item.IdMap).Count() / matches.Count * 100, 2);
                        
                        var mapstat = new MapStat
                        {
                            MapName = item.MapName,
                            MapImage = item.MapImage,
                            MapMeasurement = item.IdMeasurementNavigation.MeasurementName,
                            CountGame = countGameMap,
                            WinRateMap = winRateMap,
                            PickRateMap = pickRateMap,
                        };

                        Application.Current.Dispatcher.Invoke(() => 
                        {
                            MapStatList.Add(mapstat);
                        });
                    }
                }
            });
        }

        #endregion
    }
}