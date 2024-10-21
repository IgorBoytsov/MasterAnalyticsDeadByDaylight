using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.CalculationService.PerkService;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using System.Collections.ObjectModel;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    public class PerkPageViewModel : BaseViewModel
    {
        #region Колекции

        public List<Role> RoleList { get; set; } = [];

        public List<KillerPerkCategory> KillerPerkCategories { get; set; } = [];

        public List<SurvivorPerkCategory> SurvivorPerkCategories { get; set; } = [];

        public ObservableCollection<string> SortCategories { get; set; } = [];

        public ObservableCollection<PerkStat> PerkStatList { get; set; } = [];

        public List<PlayerAssociation> PlayerAssociationList { get; set; } = [];

        #endregion

        #region Свойства

        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                if (value.IdRole == 2)
                {
                    SortCategories.Clear();
                    foreach (var item in KillerPerkCategories.OrderBy(x => x.CategoryName))
                    {
                        SortCategories.Add(item.CategoryName);
                        SelectedSortValue = SortCategories.FirstOrDefault();
                    }
                }
                if (value.IdRole == 3)
                {
                    SortCategories.Clear();
                    foreach (var item in SurvivorPerkCategories.OrderBy(x => x.CategoryName))
                    {
                        SortCategories.Add(item.CategoryName);
                        SelectedSortValue = SortCategories.FirstOrDefault();
                    }
                }
                else return;

                OnPropertyChanged();
            }
        }

        private string _selectedSortValue;
        public string SelectedSortValue
        {
            get => _selectedSortValue;
            set
            {
                _selectedSortValue = value;
                OnPropertyChanged();
            }
        }

        private PlayerAssociation _selectedPlayerAssociation;
        public PlayerAssociation SelectedPlayerAssociation
        {
            get => _selectedPlayerAssociation;
            set
            {
                _selectedPlayerAssociation = value;
                OnPropertyChanged();
            }
        }

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

        private readonly IDataService _dataService;
        private readonly IPerkCalculationService _perkCalculationService;

        public PerkPageViewModel(IDataService dataService, IPerkCalculationService perkCalculationService)
        {
            _dataService = dataService;
            _perkCalculationService = perkCalculationService;
            IsFilterPopupOpen = false;

            GetKillerPerkCategoryData();
            GetSurvivorPerkCategoryData();
            GetPlayerAssociationData();

            GetRoleData();

            GetPerkStat();
        }

        #region Команды

        private RelayCommand _openFilterCommand;
        public RelayCommand OpenFilterCommand { get => _openFilterCommand ??= new(obj => { IsFilterPopupOpen = true; }); }

        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { GetPerkStat(); }); }

        private RelayCommand _applyCommand;
        public RelayCommand ApplyCommand
        {
            get => _applyCommand ??= new(obj =>
            {
                GetPerkStat();
                IsFilterPopupOpen = false;
            });
        }

        #endregion

        #region Методы

        private async void GetPerkStat()
        {
            PerkStatList.Clear();
            foreach (var item in await _perkCalculationService.CalculatingPerkStatAsync(SelectedRole, SelectedPlayerAssociation, SelectedSortValue))
            {
                PerkStatList.Add(item);
            }
        }

        private void GetRoleData()
        {
            RoleList.AddRange(_dataService.GetAllDataInList<Role>(x => x.Where(x => x.IdRole != 5)));
            SelectedRole = RoleList.FirstOrDefault();
        }

        private void GetKillerPerkCategoryData()
        {
            KillerPerkCategories.AddRange(_dataService.GetAllDataInList<KillerPerkCategory>());
        }

        private void GetSurvivorPerkCategoryData()
        {
            SurvivorPerkCategories.AddRange(_dataService.GetAllDataInList<SurvivorPerkCategory>());
        }

        private void GetPlayerAssociationData()
        {
            PlayerAssociationList.AddRange(_dataService.GetAllDataInList<PlayerAssociation>());
            SelectedPlayerAssociation = PlayerAssociationList.FirstOrDefault();
        }

        #endregion

    }
}
