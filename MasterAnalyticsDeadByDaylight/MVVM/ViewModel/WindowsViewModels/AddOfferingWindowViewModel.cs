using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.View.Pages;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    public class AddOfferingWindowViewModel : BaseViewModel
    {
        #region Свойства

        public ObservableCollection<Role> RoleList { get; set; } = [];

        public ObservableCollection<Offering> OfferingList { get; set; } = [];
        
        public ObservableCollection<OfferingCategory> OfferingCategoryList { get; set; } = [];

        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (value == null) { return; }
                _selectedRole = value;
                GetOfferingData();

                SearchTextBox = string.Empty;
                OfferingName = string.Empty;
                OfferingDescription = string.Empty;
                SelectedOffering = null;
                OnPropertyChanged();
            }
        }

        private Offering _selectedOffering;
        public Offering SelectedOffering
        {
            get => _selectedOffering;
            set
            {
                if (value == null) { return; }
                _selectedOffering = value;
                OfferingName = value.OfferingName;
                OfferingDescription = value.OfferingDescription;
                OfferingImage = value.OfferingImage;
                SelectedRarity = RarityList.FirstOrDefault(x => x.IdRarity == value.IdRarity);
                SelectedOfferingCategory = OfferingCategoryList.FirstOrDefault(x => x.IdCategory == value.IdCategory);
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

        private string _searchTextBox;
        public string SearchTextBox
        {
            get => _searchTextBox;
            set
            {
                _searchTextBox = value;
                SearchOffering();
                OnPropertyChanged();
            }
        }

        private byte[] _imageOffering;
        public byte[] OfferingImage
        {
            get => _imageOffering;
            set
            {
                _imageOffering = value;
                OnPropertyChanged();
            }
        }

        private string _offeringName;
        public string OfferingName
        {
            get => _offeringName;
            set
            {
                _offeringName = value;
                OnPropertyChanged();
            }
        }

        private string _offeringDescription;
        public string OfferingDescription
        {
            get => _offeringDescription;
            set
            {
                _offeringDescription = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Rarity> RarityList { get; set; } = [];

        private Rarity _selectedRarity;
        public Rarity SelectedRarity
        {
            get => _selectedRarity;
            set
            {
                _selectedRarity = value;
                OnPropertyChanged();
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        #endregion

        ICustomDialogService _dialogService;
        IDataService _dataService;

        public AddOfferingWindowViewModel(ICustomDialogService service, IDataService dataService)
        {
            _dialogService = service;
            _dataService = dataService;
            Title = "Добавление подношение";
            GetRarityData();
            GetRoleData();
            GetOfferingCategoryData();
        }

        #region Команды

        private RelayCommand _addOfferingCommand;
        public RelayCommand AddOfferingCommand { get => _addOfferingCommand ??= new(obj => { AddOffering(); }); }

        private RelayCommand _deleteOfferingCommand;
        public RelayCommand DeleteOfferingCommand { get => _deleteOfferingCommand ??= new(async obj => 
        {
            if (SelectedOffering == null) return;
            if (MessageHelper.MessageDelete(SelectedOffering.OfferingName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedOffering);
                GetOfferingData();
            }
        }); }

        private RelayCommand _updateOfferingCommand;
        public RelayCommand UpdateOfferingCommand { get => _updateOfferingCommand ??= new(obj => { UpdateOffering(); }); }

        private RelayCommand _selectImageCommand;
        public RelayCommand SelectImageCommand { get => _selectImageCommand ??= new(obj => { SelectImageOffering(); }); }

        private RelayCommand _clearImageCommand;
        public RelayCommand ClearImageCommand { get => _clearImageCommand ??= new(obj => { OfferingImage = null; }); }

        #endregion

        #region Методы

        private async void GetOfferingData()
        {         
            if (SelectedRole == null) OfferingList.Clear();
            else
            {
                List<Offering> offList = await _dataService.GetAllDataInListAsync<Offering>(x => x
                .Include(x => x.IdRarityNavigation)
                .Where(x => x.IdRole == SelectedRole.IdRole)
                .OrderBy(x => x.IdRarity));

                OfferingList.Clear();

                foreach (var item in offList)
                {
                    OfferingList.Add(item);
                }
            }
        }

        private async void GetOfferingCategoryData()
        {
            var offeringCategory = await _dataService.GetAllDataAsync<OfferingCategory>();

            foreach (var item in offeringCategory)
            {
                OfferingCategoryList.Add(item);
            }
        }

        private async void GetRoleData()
        {
            var roles = await _dataService.GetAllDataAsync<Role>();

            foreach (var item in roles)
            {
                RoleList.Add(item);
            }
            SelectedRole = RoleList.FirstOrDefault();
        }

        private async void GetRarityData()
        {
            var rarities = await _dataService.GetAllDataAsync<Rarity>();

            foreach (var item in rarities)
            {
                RarityList.Add(item);
            }
        }

        private async void AddOffering()
        {
            var newOffering = new Offering() { IdRole = SelectedRole.IdRole, OfferingName = OfferingName, OfferingImage = OfferingImage, OfferingDescription = OfferingDescription, IdRarity = SelectedRarity.IdRarity, IdCategory = SelectedOfferingCategory.IdCategory };

            bool exists = OfferingList.Any(x => x.OfferingName.ToLower() == newOffering.OfferingName.ToLower());

            if (exists || string.IsNullOrEmpty(OfferingName)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync(newOffering);
                GetOfferingData();
                OfferingName = string.Empty;
                OfferingDescription = string.Empty;
                SelectedOffering = null;
            }
        }

        private async void UpdateOffering()
        {
            if (SelectedOffering == null) return;

            var entityToUpdate = await _dataService.FindAsync<Offering>(SelectedOffering.IdOffering);

            if (entityToUpdate != null)
            {
                bool exists = OfferingList.Any(x => x.OfferingName.ToLower() == OfferingName.ToLower());

                if (exists)
                {
                    if (MessageHelper.MessageUpdate(SelectedOffering.OfferingName, OfferingName, SelectedOffering.OfferingDescription, OfferingDescription) == MessageButtons.Yes)
                    {
                        entityToUpdate.OfferingName = OfferingName;
                        entityToUpdate.OfferingDescription = OfferingDescription;
                        entityToUpdate.OfferingImage = OfferingImage;
                        entityToUpdate.IdRole = SelectedRole.IdRole;
                        entityToUpdate.IdRarity = SelectedRarity.IdRarity;
                        entityToUpdate.IdCategory = SelectedOfferingCategory.IdCategory;
                        await _dataService.UpdateAsync(entityToUpdate);

                        GetOfferingData();
                        OfferingName = string.Empty;
                        OfferingDescription = string.Empty;
                        OfferingImage = null;
                        SelectedOffering = null;
                    }
                }
                else
                {
                    entityToUpdate.OfferingName = OfferingName;
                    entityToUpdate.OfferingDescription = OfferingDescription;
                    entityToUpdate.OfferingImage = OfferingImage;
                    entityToUpdate.IdRole = SelectedRole.IdRole;
                    entityToUpdate.IdRarity = SelectedRarity.IdRarity;
                    entityToUpdate.IdCategory = SelectedOfferingCategory.IdCategory;
                    await _dataService.UpdateAsync(entityToUpdate);

                    GetOfferingData();
                    OfferingName = string.Empty;
                    OfferingDescription = string.Empty;
                    OfferingImage = null;
                    SelectedOffering = null;
                }
            }
        }

        private async void SearchOffering()
        {
            var search = await _dataService.GetAllDataInListAsync<Offering>(x => x
                .Where(x => x.IdRole == SelectedRole.IdRole)
                .Where(x => x.OfferingName.Contains(SearchTextBox)));

            OfferingList.Clear();

            foreach (var item in search)
            {
                OfferingList.Add(item);
            }
        }

        private void SelectImageOffering()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Image image = Image.FromFile(openFileDialog.FileName))
                {
                    OfferingImage = ImageHelper.ImageToByteArray(image);
                }
            }
        }

        #endregion
    }
}
