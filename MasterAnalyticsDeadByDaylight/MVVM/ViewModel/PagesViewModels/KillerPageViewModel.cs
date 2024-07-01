using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    internal class KillerPageViewModel : BaseViewModel
    {

        #region Колекции 

        private ObservableCollection<KillerStat> KillerStatList { get; set; }

        public ObservableCollection<KillerStat> KillerStatSortedList { get; set; }

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
            get { return _selectedKillerStatSortItem; }
            set
            {
                _selectedKillerStatSortItem = value;
                SortKillerStatList();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства Visibility

        private Visibility _listViewVisibility;
        public Visibility ListViewVisibility
        {
            get => _listViewVisibility;
            set
            {
                _listViewVisibility = value;
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

        private Visibility _backMenuVisibility;
        public Visibility BackMenuVisibility
        {
            get => _backMenuVisibility;
            set
            {
                _backMenuVisibility = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства для поиска по KillerStatSortedList

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
            KillerStatList = [];
            KillerStatSortedList = [];
            SetDefaultVisibility();
            GetKillerStatisticData();
            SelectedKillerStatSortItem = SortingList.First();
        }

        #region Команды

        private RelayCommand _showNameCommand;
        public RelayCommand ShowNameCommand => _showNameCommand ??= new RelayCommand(ShowNameKiller);

        private RelayCommand _showDetailsKillerCommand;
        public RelayCommand ShowDetailsKillerCommand { get => _showDetailsKillerCommand ??= new(obj => { ListViewVisibility = Visibility.Collapsed; SortMenuVisibility = Visibility.Collapsed; BackMenuVisibility = Visibility.Visible; }); }

        private RelayCommand _backToListViewCommand;
        public RelayCommand BackToListViewCommand { get => _backToListViewCommand ??= new(obj => { ListViewVisibility = Visibility.Visible; SortMenuVisibility = Visibility.Visible; BackMenuVisibility = Visibility.Collapsed; }); }
        
        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { GetKillerStatisticData(); SortKillerStatsByDescendingOrder(); SearchTextBox = string.Empty; }); }

        #endregion

        #region Методы

        private void GetKillerStatisticData()
        {
            KillerStatList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                List<Killer> KillerList = context.Killers.Skip(1).ToList();

                //Количество матчей в базе данных, в которых я играл за киллера
                int CountMatch =
                    context.GameStatistics
                    .Include(gs => gs.IdKillerNavigation)
                    .ThenInclude(killerInfo => killerInfo.IdAssociationNavigation)
                    .Where(gs => gs.IdKillerNavigation.IdAssociation == 1)
                    .Count();

                foreach (var killer in KillerList)
                {
                    //Количество игр за конкретного киллера, по порядку
                    List<GameStatistic> GameStat = 
                        context.GameStatistics
                        .Include(gs => gs.IdKillerNavigation)
                        .ThenInclude(killerInfo => killerInfo.IdKillerNavigation)

                        .Include(gs => gs.IdKillerNavigation)
                        .ThenInclude(killerInfo => killerInfo.IdAssociationNavigation)

                        .Where(gs => gs.IdKillerNavigation.IdAssociation == 1)
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

                    //Добавление данных по киллерам
                    var game = new KillerStat()
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
                    KillerStatList.Add(game);
                }
            }
        }

        private void SetDefaultVisibility()
        { 
            SortMenuVisibility = Visibility.Visible;
            BackMenuVisibility = Visibility.Collapsed;
        }

        private void ShowNameKiller(object parameter)
        {
            if (parameter is KillerStat selectedKiller)
            {
                MessageBox.Show(selectedKiller.KillerName);
            }
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

            if (SearchTextBox == string.Empty)
            {
                SortKillerStatList();
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
    }
}
