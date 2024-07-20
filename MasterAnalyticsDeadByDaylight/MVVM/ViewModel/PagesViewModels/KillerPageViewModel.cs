using LiveCharts.Wpf;
using LiveCharts;
using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    internal class KillerPageViewModel : BaseViewModel
    {

        #region Колекции 

        private ObservableCollection<KillerStat> KillerStatList { get; set; }

        public ObservableCollection<KillerStat> KillerStatSortedList { get; set; }

        public ObservableCollection<PlayerAssociation> PlayerAssociationList { get; set; }

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
            get  => _selectedKillerStatSortItem;
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
        });}
        
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

        #endregion

        #region Средний счет очков (День, Неделя, Месяц, Год, за патч)

        #endregion

        #region Количество сыгранных матчей (День, Неделя, Месяц, Год, за патч)

        #endregion

        #region KillRate (День, Неделя, Месяц, Год, за патч)

        #endregion

        #region WinRate (День, Неделя, Месяц, Год, за патч)

        #endregion

        #region Частота использования перков (Выжившие против выбранного киллера, Киллер)

        #endregion

        #region Частота использования билдов (Выжившие против выбранного киллера, Киллер)

        #endregion

        #region Частота использования билдов (Выжившие против выбранного киллера, Киллер)

        #endregion

        #region Популярные предметы выживших

        #endregion

        #region Частота использования Улучшений (Выжившие(Предметы) против выбранного киллера, Киллер)

        #endregion

        #region Частота использования подношений (Выжившие(Предметы) против выбранного киллера, Киллер)

        #endregion

        #region Количество престижей выживших

        #endregion

        #region Количество игроков по платформам

        #endregion

        #region % ливающих игроков

        #endregion

        #region % Анонимных игроков

        #endregion

        #region Количество персонажей (Выжившие), встречаемые в игре на выбранном киллере

        #endregion

        #region % Повесов (0-12)

        #endregion

        #region % Оставшихся генераторов (0-5)

        #endregion

        #region Типы смертей выживший

        #endregion

        #region Количество сыгранных матчей на картах ( W\R, K\R, без подношений, с подношениями)

        #endregion

        #region % киллов (0-4)

        #endregion
    }
}
