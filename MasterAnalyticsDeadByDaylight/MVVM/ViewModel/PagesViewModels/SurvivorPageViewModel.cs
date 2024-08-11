using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    class SurvivorPageViewModel : BaseViewModel
    {

        #region Колекции

        public ObservableCollection<SurvivorStat> SurvivorStatList { get; set; } = [];

        public ObservableCollection<SurvivorStat> SurvivorSortedStatList { get; set; } = [];

        public ObservableCollection<string> SortingList { get; set; } =
            [
            "Дате выхода (Убыв.)", "Дате выхода (Возр.)",
            "Алфавит (Я-А)", "Алфавит (А-Я)",
            "Пикрейт (%)",
            "Побегам (%)","Побегам (Кол-во)",
            "Анонимному моду (%)","Анонимному моду (Кол-во)",
            "Количеству игроков",
            "Количеству вышедших из игры (%)", "Количеству вышедших из игры (Кол-во)",
            ];

        public ObservableCollection<string> Association { get; set; } = ["Общая", "Личная по убийствам за киллера", "Личная за Выжившего"];

        #endregion

        private string _searchTextBox;
        public string SearchTextBox
        {
            get => _searchTextBox;
            set
            {
                _searchTextBox = value;
                SearchSurvivorName();
                OnPropertyChanged();
            }
        }

        #region Свойства Visibility

        private Visibility _survivorListVisibility;
        public Visibility SurvivorListVisibility
        {
            get => _survivorListVisibility;
            set
            {
                _survivorListVisibility = value;
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

        #region Свойства Selected

        private string _selectedSurvivorStatSortItem;
        public string SelectedSurvivorStatSortItem
        {
            get => _selectedSurvivorStatSortItem;
            set
            {
                _selectedSurvivorStatSortItem = value;
                SortSurvivorStatList();
                OnPropertyChanged();
            }
        }

        private string _selectedAssociation;
        public string SelectedAssociation
        {
            get => _selectedAssociation;
            set
            {
                _selectedAssociation = value;
                SortSurvivorStatList(); 
                OnPropertyChanged();
            }
        }

        #endregion

        public SurvivorPageViewModel() 
        {
            SelectedAssociation = Association[0];
            SelectedSurvivorStatSortItem = SortingList[0];
            SetDefaultVisibility();
        }

        #region Команды

        private RelayCommand _backToListViewCommand;
        public RelayCommand BackToListViewCommand
        {
            get => _backToListViewCommand ??= new(obj =>
            {
                SurvivorListVisibility = Visibility.Visible;
                SortMenuVisibility = Visibility.Visible;
                DetailedInformationVisibility = Visibility.Collapsed;
            });
        }

        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { SortSurvivorStatList(); SearchTextBox = string.Empty; }); }

        #endregion

        #region Методы

        private void SetDefaultVisibility()
        {
            SortMenuVisibility = Visibility.Visible;
            DetailedInformationVisibility = Visibility.Collapsed;
        }

        #endregion

        #region Методы получение данных

        private void GetSurvivorStatisticData()
        {
            SurvivorStatList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                List<Survivor> SurvivorList = context.Survivors.Skip(1).ToList();

                List<SurvivorInfo> SurvivorInfoList = null;

                if (SelectedAssociation == "Общая")
                {
                    SurvivorInfoList = context.SurvivorInfos.ToList();
                }
                if (SelectedAssociation == "Личная за Выжившего")
                {
                    SurvivorInfoList = context.SurvivorInfos.Where(x => x.IdAssociation == 1).ToList();
                }
                if (SelectedAssociation == "Личная по убийствам за киллера")
                {
                    SurvivorInfoList = context.SurvivorInfos.Where(x => x.IdAssociation == 3).ToList();
                }
                if (SelectedAssociation == null)
                {
                    return;
                }

                foreach (var survivor in SurvivorList)
                {
                    int survivorCount = SurvivorInfoList.Where(x => x.IdSurvivor == survivor.IdSurvivor).Count();
                    double survivorPickRate = Math.Round((double)survivorCount / SurvivorInfoList.Count * 100, 2);
                    int survivorEscapeCount = SurvivorInfoList.Where(x => x.IdSurvivor == survivor.IdSurvivor).Where(x => x.IdTypeDeath == 5).Count();
                    double survivorEscapePercentage = Math.Round((double)survivorEscapeCount / SurvivorInfoList.Where(x => x.IdSurvivor == survivor.IdSurvivor).Count() * 100, 2);
                    int survivorAnonymousModeCount = SurvivorInfoList.Where(x => x.IdSurvivor == survivor.IdSurvivor).Where(x => x.AnonymousMode == true).Count();
                    double survivorAnonymousModePercentage = Math.Round((double)survivorAnonymousModeCount / SurvivorInfoList.Where(x => x.IdSurvivor == survivor.IdSurvivor).Count() * 100, 2);
                    double avgPrestige = AVGPrestige(SurvivorInfoList);
                    int survivorBotCount = SurvivorInfoList.Where(x => x.IdSurvivor == survivor.IdSurvivor).Where(x => x.Bot == true).Count();
                    double survivorBotPercentage = Math.Round((double)survivorBotCount / SurvivorInfoList.Where(x => x.IdSurvivor == survivor.IdSurvivor).Count() * 100, 2);

                    var survivorStat = new SurvivorStat
                    {
                        SurvivorName = survivor.SurvivorName,
                        SurvivorImage = survivor.SurvivorImage,
                        SurvivorCount = survivorCount,
                        SurvivorPickRate = survivorPickRate,
                        SurvivorEscapeCount = survivorEscapeCount,
                        SurvivorEscapePercentage = survivorEscapePercentage,
                        SurvivorAnonymousModeCount = survivorAnonymousModeCount,
                        SurvivorAnonymousModePercentage = survivorAnonymousModePercentage,
                        SurvivorAVGPrestige = avgPrestige,
                        SurvivorBotCount = survivorBotCount,
                        SurvivorBotPercentage = survivorBotPercentage
                    };
                    SurvivorStatList.Add(survivorStat);
                }
            }
        }

        private static double AVGPrestige(List<SurvivorInfo> survivorInfos)
        {
            int CountPrestige = 0;
            foreach (var item in survivorInfos)
            {
                CountPrestige += item.Prestige;
            }
            return CountPrestige;
        }

        #endregion

        #region Методы сортировки

        private void SortSurvivorStatList()
        {
            GetSurvivorStatisticData(); 
            switch (SelectedSurvivorStatSortItem)
            {
                case "Дате выхода (Убыв.)":
                    SortSurvivorStatsByDescendingOrder();
                    break;
                case "Дате выхода (Возр.)":
                    SortSurvivorStatsByAscendingOrder();
                    break;
                case "Алфавит (Я-А)":
                    SortSurvivorStatsBySurvivorNameAscendingOrder();
                    break;
                case "Алфавит (А-Я)":
                    SortSurvivorStatsBySurvivorNameDescendingOrder();
                    break;
                case "Пикрейт (%)":
                    SortSurvivorStatsBySurvivorPickRatePercentageOrder();
                    break;
                case "Побегам (%)":
                    SortSurvivorStatsBySurvivorEscapeRatePercentageOrder();
                    break;
                case "Побегам (Кол-во)":
                    SortSurvivorStatsBySurvivorEscapeRateCountOrder();
                    break;
                case "Анонимному моду (%)":
                    SortSurvivorStatsBySurvivorSurvivorAnonymousModePercentageOrder();
                    break;
                case "Анонимному моду (Кол-во)":
                    SortSurvivorStatsBySurvivorSurvivorAnonymousModeCountOrder();
                    break;
                case "Количеству игроков":
                    SortSurvivorStatsBySurvivorCountOrder();
                    break;
                case "Количеству вышедших из игры (%)":
                    SortSurvivorStatsBySurvivorBotPercentageOrder();
                    break;
                case "Количеству вышедших из игры (Кол-во)":
                    SortSurvivorStatsBySurvivorBotCountOrder();
                    break;
            }
        }

        private void SearchSurvivorName()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList.Where(ks => ks.SurvivorName.ToLower().Contains(SearchTextBox.ToLower())))
            {
                SurvivorSortedStatList.Add(item);
            }
        }

        private void SortSurvivorStatsByDescendingOrder()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList)
            {
                SurvivorSortedStatList.Add(item);
            }
        }

        private void SortSurvivorStatsByAscendingOrder()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList.Reverse())
            {
                SurvivorSortedStatList.Add(item);
            }
        }

        private void SortSurvivorStatsBySurvivorNameDescendingOrder()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList.OrderByDescending(x => x.SurvivorName))
            {
                SurvivorSortedStatList.Add(item);
            }
        }

        private void SortSurvivorStatsBySurvivorNameAscendingOrder()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList.OrderBy(x => x.SurvivorName))
            {
                SurvivorSortedStatList.Add(item);
            }
        }

        private void SortSurvivorStatsBySurvivorPickRatePercentageOrder()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList.OrderByDescending(x => x.SurvivorPickRate))
            {
                SurvivorSortedStatList.Add(item);
            }
        } 

        private void SortSurvivorStatsBySurvivorEscapeRatePercentageOrder()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList.OrderByDescending(x => x.SurvivorEscapePercentage))
            {
                SurvivorSortedStatList.Add(item);
            }
        }

        private void SortSurvivorStatsBySurvivorEscapeRateCountOrder()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList.OrderByDescending(x => x.SurvivorEscapeCount))
            {
                SurvivorSortedStatList.Add(item);
            }
        }

        private void SortSurvivorStatsBySurvivorSurvivorAnonymousModePercentageOrder()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList.OrderByDescending(x => x.SurvivorAnonymousModePercentage))
            {
                SurvivorSortedStatList.Add(item);
            }
        }

        private void SortSurvivorStatsBySurvivorSurvivorAnonymousModeCountOrder()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList.OrderByDescending(x => x.SurvivorAnonymousModeCount))
            {
                SurvivorSortedStatList.Add(item);
            }
        }

        private void SortSurvivorStatsBySurvivorCountOrder()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList.OrderByDescending(x => x.SurvivorCount))
            {
                SurvivorSortedStatList.Add(item);
            }
        }

        private void SortSurvivorStatsBySurvivorBotPercentageOrder()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList.OrderByDescending(x => x.SurvivorBotPercentage))
            {
                SurvivorSortedStatList.Add(item);
            }
        }

        private void SortSurvivorStatsBySurvivorBotCountOrder()
        {
            SurvivorSortedStatList.Clear();
            foreach (var item in SurvivorStatList.OrderByDescending(x => x.SurvivorBotCount))
            {
                SurvivorSortedStatList.Add(item);
            }
        }

        #endregion

    }
}
