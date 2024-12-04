using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Utils.Calculation;
using System.Collections.ObjectModel;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    public class OfferingPageViewModel : BaseViewModel
    {
        #region Количекции

        public ObservableCollection<OfferingStat> OfferingStatList { get; set; } = [];

        public ObservableCollection<OfferingCategory> OfferingCategories { get; set; } = [];

        public ObservableCollection<PlayerAssociation> TypeAssociation { get; set; } = [];

        #endregion

        private readonly IDataService _dataService;

        public OfferingPageViewModel(IDataService dataService)
        {
            _dataService = dataService;

            GetTypeAssociationData();
            GetOfferingCategoryData();

            SelectedOfferingCategory = OfferingCategories.FirstOrDefault();
            SelectedPlayerAssociation = TypeAssociation.FirstOrDefault();

            GetOfferingInfo();
            IsFilterPopupOpen = false;
        }

        #region Свойства

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

        private OfferingCategory _selectedOfferingCategory;
        public OfferingCategory SelectedOfferingCategory
        {
            get => _selectedOfferingCategory;
            set
            {
                _selectedOfferingCategory = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Команды

        private RelayCommand _openFilterCommand;
        public RelayCommand OpenFilterCommand { get => _openFilterCommand ??= new(obj => { IsFilterPopupOpen = true; }); }

         private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { GetOfferingInfo(); }); }

        private RelayCommand _applyCommand;
        public RelayCommand ApplyCommand
        {
            get => _applyCommand ??= new(obj =>
            {
                GetOfferingInfo();
                IsFilterPopupOpen = false;
            });
        }

        #endregion

        #region Методы

        private async void GetOfferingInfo()
        {
            OfferingStatList.Clear();
            //var offeringStat = await _offeringCalculationService.CalculatingOfferingStat(SelectedPlayerAssociation, SelectedOfferingCategory);
            var offeringStat = await CalculationOffering.CalculatingOfferingStat(SelectedPlayerAssociation, SelectedOfferingCategory);
            foreach (var item in offeringStat)
            {
                OfferingStatList.Add(item);
            }
          
        }

        private void GetOfferingCategoryData()
        {
            var offeringCategory = _dataService.GetAllData<OfferingCategory>();
            foreach (var item in offeringCategory)
            {
                OfferingCategories.Add(item);
            }
           
        }
        
        private void GetTypeAssociationData()
        {
            var playerAssociation = _dataService.GetAllData<PlayerAssociation>();
            foreach (var item in playerAssociation)
            {
                TypeAssociation.Add(item);
            }
           
        }

        #endregion

    }
}