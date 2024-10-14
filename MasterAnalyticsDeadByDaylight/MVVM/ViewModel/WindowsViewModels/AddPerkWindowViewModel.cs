using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    public class AddPerkWindowViewModel : BaseViewModel
    {
        #region Свойства

        public ObservableCollection<Killer> KillerList { get; set; } = [];

        public ObservableCollection<KillerPerkCategory> KillerPerkCategoryList { get; set; } = [];

        public ObservableCollection<Survivor> SurvivorList { get; set; } = [];

        public ObservableCollection<SurvivorPerkCategory> SurvivorPerkCategoryList { get; set; } = [];

        public ObservableCollection<Role> RoleList { get; set; } = [];

        public ObservableCollection<Character> CharacterList { get; set; } = [];

        public ObservableCollection<Perk> PerkList { get; set; } = [];

        public ObservableCollection<KillerPerk> KillerPerkList { get; set; } = [];

        public ObservableCollection<SurvivorPerk> SurvivorPerkList { get; set; } = [];

        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (value == null) return;
                _selectedRole = value;
                if (value.RoleName == "Убийца")
                {
                    KillerPerkCategoryVisibility = Visibility.Visible;
                    SurvivorPerkCategoryVisibility = Visibility.Collapsed;
                }
                if (value.RoleName == "Выживший")
                {
                    KillerPerkCategoryVisibility = Visibility.Collapsed;
                    SurvivorPerkCategoryVisibility = Visibility.Visible;
                }
                GetCharacterList();
                if (CharacterList.Count != 0) SelectedCharacter = CharacterList.First();
                OnPropertyChanged();
            }
        }

        private Character _selectedCharacter;
        public Character SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                if (value == null) return;
                _selectedCharacter = value;
                GetPerkData();
                PerkNameTextBox = string.Empty;
                ImagePerk = null;
                PerkDescriptionTextBox = string.Empty;
                OnPropertyChanged();
            }
        }

        private Perk _selectedPerk;
        public Perk SelectedPerk
        {
            get => _selectedPerk;
            set
            {
                if (value == null) return;
                _selectedPerk = value;
                PerkNameTextBox = value.PerkName;
                ImagePerk = value.PerkImage;
                if (SelectedRole.RoleName == "Убийца") SelectedKillerPerkCategory = KillerPerkCategoryList.FirstOrDefault(x => x.IdKillerPerkCategory == value.IdCategory);
                if (SelectedRole.RoleName == "Выживший") SelectedSurvivorPerkCategory = SurvivorPerkCategoryList.FirstOrDefault(x => x.IdSurvivorPerkCategory == value.IdCategory);
                PerkDescriptionTextBox = value.PerkDescription;
                OnPropertyChanged();
            }
        }

        private KillerPerkCategory _selectedKillerPerkCategory;
        public KillerPerkCategory SelectedKillerPerkCategory
        {
            get => _selectedKillerPerkCategory;
            set
            {
                _selectedKillerPerkCategory = value;
                OnPropertyChanged();
            }
        } 
        
        private KillerPerkCategory _selectedKillerPerkCategoryListView;
        public KillerPerkCategory SelectedKillerPerkCategoryListView
        {
            get => _selectedKillerPerkCategoryListView;
            set
            {
                _selectedKillerPerkCategoryListView = value;
                CategoryName = value.CategoryName;
                OnPropertyChanged();
            }
        }

        private Visibility _killerPerkCategoryVisibility;
        public Visibility KillerPerkCategoryVisibility
        {
            get => _killerPerkCategoryVisibility;
            set
            {
                _killerPerkCategoryVisibility = value;
                OnPropertyChanged();
            }
        }

        private SurvivorPerkCategory _selectedSurvivorPerkCategory;
        public SurvivorPerkCategory SelectedSurvivorPerkCategory
        {
            get => _selectedSurvivorPerkCategory;
            set
            {
                _selectedSurvivorPerkCategory = value;
                OnPropertyChanged();
            }
        }
        
        private SurvivorPerkCategory _selectedSurvivorPerkCategoryListView;
        public SurvivorPerkCategory SelectedSurvivorPerkCategoryListView
        {
            get => _selectedSurvivorPerkCategoryListView;
            set
            {
                _selectedSurvivorPerkCategoryListView = value;
                CategoryName = value.CategoryName;
                OnPropertyChanged();
            }
        } 
        
        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
                OnPropertyChanged();
            }
        }

        private Visibility _survivorPerkCategoryVisibility;
        public Visibility SurvivorPerkCategoryVisibility
        {
            get => _survivorPerkCategoryVisibility;
            set
            {
                _survivorPerkCategoryVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _comboBoxCharacterIsActive;
        public bool ComboBoxCharacterIsActive
        {
            get => _comboBoxCharacterIsActive;
            set
            {
                _comboBoxCharacterIsActive = value;
                OnPropertyChanged();
            }
        }

        private byte[] _imagePerk;
        public byte[] ImagePerk
        {
            get => _imagePerk;
            set
            {
                _imagePerk = value;
                OnPropertyChanged();
            }
        }

        private string _perkNameTextBox;
        public string PerkNameTextBox
        {
            get => _perkNameTextBox;
            set
            {
                _perkNameTextBox = value;
                OnPropertyChanged();
            }
        }

        private string _perkDescriptionTextBox;
        public string PerkDescriptionTextBox
        {
            get => _perkDescriptionTextBox;
            set
            {
                _perkDescriptionTextBox = value;
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

        private bool _isAddCategoryPopupOpen;
        public bool IsAddCategoryPopupOpen
        {
            get => _isAddCategoryPopupOpen;
            set
            {
                _isAddCategoryPopupOpen = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private readonly ICustomDialogService _dialogService;
        private readonly IDataService _dataService;

        public AddPerkWindowViewModel(ICustomDialogService dialogService,IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;
            Title = "Добавить перк";
            IsAddCategoryPopupOpen = false;
            KillerPerkCategoryVisibility = Visibility.Collapsed;
            SurvivorPerkCategoryVisibility = Visibility.Collapsed;
            GetSurvivorPerkCategoryData();
            GetKillerPerkCategoryData();
            GetKillerData();
            GetSurvivorData();
            GetRoleData();
            GetPerkData();
        }

        #region Команды

        private RelayCommand _addPerkCommand;
        public RelayCommand AddPerkCommand { get => _addPerkCommand ??= new(obj => { AddPerk(); }); }

        private RelayCommand _addCategoryPerkCommand;
        public RelayCommand AddCategoryPerkCommand { get => _addCategoryPerkCommand ??= new(obj => { AddCategoryPerk(); }); }

        private RelayCommand _deletePerkCommand;
        public RelayCommand DeletePerkCommand { get => _deletePerkCommand ??= new(obj => { DeletePerk(); }); }
       
        private RelayCommand _deleteKillerCategoryPerkCommand;
        public RelayCommand DeleteKillerCategoryPerkCommand { get => _deleteKillerCategoryPerkCommand ??= new(async obj => 
        {
            if (SelectedKillerPerkCategoryListView == null) return;

            if (MessageHelper.MessageDelete(SelectedKillerPerkCategoryListView.CategoryName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedKillerPerkCategoryListView);
                GetKillerPerkCategoryData();
            }
        }); }

        private RelayCommand _deleteSurvivorCategoryPerkCommand;
        public RelayCommand DeleteSurvivorCategoryPerkCommand { get => _deleteSurvivorCategoryPerkCommand ??= new(async obj =>
        {
            if (SelectedSurvivorPerkCategoryListView == null) return;

            if (MessageHelper.MessageDelete(SelectedSurvivorPerkCategoryListView.CategoryName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedSurvivorPerkCategoryListView);
                GetSurvivorPerkCategoryData();
            }
        }); }

        private RelayCommand _updateCategoryPerkCommand;
        public RelayCommand UpdateCategoryPerkCommand { get => _updateCategoryPerkCommand ??= new(obj => { UpdateCategoryPerk(); }); } 
        
        private RelayCommand _updatePerkCommand;
        public RelayCommand UpdatePerkCommand { get => _updatePerkCommand ??= new(obj => { UpdatePerk(); }); }

        private RelayCommand _selectImagePerkCommand;
        public RelayCommand SelectImagePerkCommand { get => _selectImagePerkCommand ??= new(obj => { SelectImagePerk(); }); } 
        
        private RelayCommand _clearImageCommand;
        public RelayCommand ClearImageCommand { get => _clearImageCommand ??= new(obj => { ImagePerk = null; }); }   

        private RelayCommand _openAddCategoryCommand;
        public RelayCommand OpenAddCategoryCommand { get => _openAddCategoryCommand ??= new(obj => { IsAddCategoryPopupOpen = true; }); } 

        #endregion

        #region Методы получение данных

        private void GetCharacterList()
        {
            if (SelectedRole == null) { return; }

            else if (SelectedRole.RoleName == "Убийца")
            {
                CharacterList.Clear();

                foreach (var item in KillerList)
                {
                    var character = new Character() { IdCharacter = item.IdKiller, NameCharacter = item.KillerName, ImageCharacter = item.KillerImage };
                    CharacterList.Add(character);
                }

                ComboBoxCharacterIsActive = true;
            }

            else if (SelectedRole.RoleName == "Выживший")
            {
                CharacterList.Clear();

                foreach (var item in SurvivorList)
                {
                    var character = new Character() { IdCharacter = item.IdSurvivor, NameCharacter = item.SurvivorName, ImageCharacter = item.SurvivorImage };
                    CharacterList.Add(character);
                }

                ComboBoxCharacterIsActive = true;
            }
        }

        private void GetPerkData()
        {
            PerkList.Clear();

            if (SelectedRole == null) { return; }

            else if (SelectedRole.RoleName == "Убийца")
            {
                PerkList.Clear();
                GetKillerPerkData();
            }

            else if (SelectedRole.RoleName == "Выживший")
            {
                PerkList.Clear();
                GetSurvivorPerkData();
            }
        }

        private async void GetRoleData()
        {
            var items = await _dataService.GetAllDataAsync<Role>();
            RoleList.Clear();

            foreach (var item in items)
            {
                RoleList.Add(item);
            }

            await Task.Delay(2000);

            SelectedRole = RoleList.FirstOrDefault();
        }

        private async void GetKillerData()
        {
            var items = await _dataService.GetAllDataAsync<Killer>();
            KillerList.Clear();

            foreach (var item in items)
            {
                KillerList.Add(item);
            }
        }

        private async void GetSurvivorData()
        {
            var items = await _dataService.GetAllDataAsync<Survivor>();
            SurvivorList.Clear();

            foreach (var item in items)
            {
                SurvivorList.Add(item);
            }
        }

        private async void GetKillerPerkData()
        {
            var items = await _dataService.GetAllDataAsync<KillerPerk>(x => x.Where(x => x.IdKiller == SelectedCharacter.IdCharacter));

            foreach (var item in items)
            {
                var perks = new Perk()
                {
                    IdCharacter = item.IdKiller,
                    IdPerk = item.IdKillerPerk,
                    PerkName = item.PerkName,
                    PerkImage = item.PerkImage,
                    IdCategory = item.IdCategory,
                    PerkDescription = item.PerkDescription
                };

                PerkList.Add(perks);
            }
        }

        private async void GetSurvivorPerkData()
        {
            var items = await _dataService.GetAllDataAsync<SurvivorPerk>(x => x.Where(sp => sp.IdSurvivor == SelectedCharacter.IdCharacter));
            foreach (var item in items)
            {
                var perks = new Perk()
                {
                    IdCharacter = item.IdSurvivor,
                    IdPerk = item.IdSurvivorPerk,
                    PerkName = item.PerkName,
                    PerkImage = item.PerkImage,
                    IdCategory = item.IdCategory,
                    PerkDescription = item.PerkDescription
                };

                PerkList.Add(perks);
            }
        }

        private async void GetSurvivorPerkCategoryData()
        {
            SurvivorPerkCategoryList.Clear();
            var items = await _dataService.GetAllDataAsync<SurvivorPerkCategory>();

            foreach (var item in items.OrderBy(x => x.CategoryName))
            {
                SurvivorPerkCategoryList.Add(item);
            }
        }

        private async void GetKillerPerkCategoryData()
        {
            KillerPerkCategoryList.Clear();
            var items = await _dataService.GetAllDataAsync<KillerPerkCategory>();

            foreach (var item in items.OrderBy(x => x.CategoryName))
            {
                KillerPerkCategoryList.Add(item);
            }
        }

        #endregion

        #region Методы добавление данных

        private void AddPerk()
        {
            if (SelectedRole == null) { _dialogService.ShowMessage("Вы не выбрали игровую роль"); return; }
            if (SelectedCharacter == null) { _dialogService.ShowMessage("Вы не выбрали персонажа"); return; }
            if (SelectedRole.RoleName == "Убийца") { AddKillerPerk(); }
            if (SelectedRole.RoleName == "Выживший") { AddSurvivorPerk(); }
        }

        private async void AddSurvivorPerk()
        {
            var newPerk = new SurvivorPerk() { IdSurvivor = SelectedCharacter.IdCharacter, PerkName = PerkNameTextBox, PerkImage = ImagePerk, IdCategory = SelectedSurvivorPerkCategory.IdSurvivorPerkCategory, PerkDescription = PerkDescriptionTextBox };

            if (string.IsNullOrWhiteSpace(PerkNameTextBox)) return;

            bool exists = await _dataService.ExistsAsync<SurvivorPerk>(x => x.PerkName.ToLower() == newPerk.PerkName.ToLower());

            if (exists || string.IsNullOrWhiteSpace(PerkNameTextBox)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync(newPerk);
                GetPerkData();
                PerkNameTextBox = string.Empty;
                ImagePerk = null;
                SelectedSurvivorPerkCategory = null;
            }
        }

        private async void AddKillerPerk()
        {
            var newPerk = new KillerPerk() { IdKiller = SelectedCharacter.IdCharacter, PerkName = PerkNameTextBox, PerkImage = ImagePerk, IdCategory = SelectedKillerPerkCategory.IdKillerPerkCategory , PerkDescription = PerkDescriptionTextBox };

            if (string.IsNullOrWhiteSpace(PerkNameTextBox)) return;

            bool exists = await _dataService.ExistsAsync<KillerPerk>(kp => kp.PerkName.ToLower() == newPerk.PerkName.ToLower());

            if (exists || string.IsNullOrWhiteSpace(PerkNameTextBox)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync(newPerk);
                GetPerkData();
                PerkNameTextBox = string.Empty;
                ImagePerk = null;
                SelectedKillerPerkCategory = null;
            }
        }

        private async void AddCategoryPerk()
        {
            if (SelectedRole.RoleName == "Убийца")
            {
                var newCategory = new KillerPerkCategory() { CategoryName = CategoryName };

                if (string.IsNullOrWhiteSpace(CategoryName)) return;

                bool exists = await _dataService.ExistsAsync<KillerPerkCategory>(kp => kp.CategoryName.ToLower() == newCategory.CategoryName.ToLower());

                if (exists || string.IsNullOrWhiteSpace(CategoryName)) MessageHelper.MessageExist();
                else
                {
                    await _dataService.AddAsync(newCategory);
                    GetKillerPerkCategoryData();
                    CategoryName = string.Empty;
                }
            }
            if (SelectedRole.RoleName == "Выживший")
            {
                var newCategory = new SurvivorPerkCategory() { CategoryName = CategoryName };

                if (string.IsNullOrWhiteSpace(CategoryName)) return;

                bool exists = await _dataService.ExistsAsync<SurvivorPerkCategory>(kp => kp.CategoryName.ToLower() == newCategory.CategoryName.ToLower());

                if (exists || string.IsNullOrWhiteSpace(CategoryName)) MessageHelper.MessageExist();
                else
                {
                    await _dataService.AddAsync(newCategory);
                    GetSurvivorPerkCategoryData();
                    CategoryName = string.Empty;
                }
            }
        }

        #endregion

        #region Методы удаление данных

        private void DeletePerk()
        {
            if (SelectedPerk == null) { return; }
            if (SelectedRole.RoleName == "Убийца") { DeleteKillerPerk(); }
            if (SelectedRole.RoleName == "Выживший") { DeleteSurvivorPerk(); }
        }

        private async void DeleteKillerPerk()
        {
            var itemToDelete = _dataService.FindAsync<KillerPerk>(SelectedPerk.IdPerk); 

            if (itemToDelete != null)
            {
                await _dataService.RemoveAsync(itemToDelete);
                GetPerkData();
            }
        }

        private async void DeleteSurvivorPerk()
        {
            var itemToDelete = await _dataService.FindAsync<SurvivorPerk>(SelectedPerk.IdPerk);
            if (itemToDelete != null)
            {
                await _dataService.RemoveAsync(itemToDelete);
                GetPerkData();
            }
        }

        #endregion

        #region Методы обновление данных

        private void UpdatePerk()
        {
            if (SelectedPerk == null) { return; }
            if (SelectedRole.RoleName == "Общая") { _dialogService.ShowMessage("Вы не выбрали роль"); return; }
            if (SelectedCharacter == null) { _dialogService.ShowMessage("Вы не выбрали персонажа"); return; }

            if (SelectedRole.RoleName == "Убийца") { UpdateKillerPerk(); }
            if (SelectedRole.RoleName == "Выживший") { UpdateSurvivorPerk(); }
        }
        
        private void UpdateCategoryPerk()
        {
            if (SelectedRole.RoleName == "Убийца") UpdateKillerCategoryPerk();
            if (SelectedRole.RoleName == "Выживший") UpdateSurvivorCategoryPerk();
        }

        private async void UpdateKillerPerk()
        {
            if (SelectedPerk == null) { return; }

            var entityToUpdate = await _dataService.FindAsync<KillerPerk>(SelectedPerk.IdPerk);

            if (entityToUpdate != null)
            {
                if (MessageHelper.MessageUpdate(SelectedPerk.PerkName, PerkNameTextBox, SelectedPerk.PerkDescription, PerkDescriptionTextBox) == MessageButtons.Yes)
                {
                    entityToUpdate.PerkName = PerkNameTextBox;
                    entityToUpdate.PerkImage = ImagePerk;
                    entityToUpdate.IdCategory = SelectedKillerPerkCategory.IdKillerPerkCategory;
                    entityToUpdate.PerkDescription = PerkDescriptionTextBox;
                    await _dataService.UpdateAsync(entityToUpdate);

                    GetPerkData();

                    PerkNameTextBox = string.Empty;
                    PerkDescriptionTextBox = string.Empty;
                    ImagePerk = null;
                    SelectedPerk = null;
                    SelectedKillerPerkCategory = null;
                }
            }
        }

        private async void UpdateKillerCategoryPerk()
        {
            if (SelectedKillerPerkCategoryListView == null) { return; }

            var entityToUpdate = await _dataService.FindAsync<KillerPerkCategory>(SelectedKillerPerkCategoryListView.IdKillerPerkCategory);

            if (entityToUpdate != null)
            {
                if (MessageHelper.MessageUpdate(SelectedKillerPerkCategoryListView.CategoryName, CategoryName, "", "") == MessageButtons.Yes)
                {
                    entityToUpdate.CategoryName = CategoryName;
                    await _dataService.UpdateAsync(entityToUpdate);

                    GetKillerPerkCategoryData();

                    CategoryName = string.Empty;
                }
            }
        }

        private async void UpdateSurvivorPerk()
        {
            if (SelectedPerk == null) { return; }

            var entityToUpdate = await _dataService.FindAsync<SurvivorPerk>(SelectedPerk.IdPerk);

            if (entityToUpdate != null)
            {
                if (MessageHelper.MessageUpdate(SelectedPerk.PerkName, PerkNameTextBox, SelectedPerk.PerkDescription, PerkDescriptionTextBox) == MessageButtons.Yes)
                {
                    entityToUpdate.PerkName = PerkNameTextBox;
                    entityToUpdate.PerkImage = ImagePerk;
                    entityToUpdate.IdCategory = SelectedSurvivorPerkCategory.IdSurvivorPerkCategory;
                    entityToUpdate.PerkDescription = PerkDescriptionTextBox;
                    await _dataService.UpdateAsync(entityToUpdate);

                    GetPerkData();

                    PerkNameTextBox = string.Empty;
                    PerkDescriptionTextBox = string.Empty;
                    ImagePerk = null;
                    SelectedPerk = null;
                    SelectedSurvivorPerkCategory = null;
                }
            }
        }

        private async void UpdateSurvivorCategoryPerk()
        {
            if (SelectedSurvivorPerkCategoryListView == null) { return; }

            var entityToUpdate = await _dataService.FindAsync<SurvivorPerkCategory>(SelectedSurvivorPerkCategoryListView.IdSurvivorPerkCategory);

            if (entityToUpdate != null)
            {
                if (MessageHelper.MessageUpdate(SelectedSurvivorPerkCategoryListView.CategoryName, CategoryName, "", "") == MessageButtons.Yes)
                {
                    entityToUpdate.CategoryName = CategoryName;
                    await _dataService.UpdateAsync(entityToUpdate);

                    GetSurvivorPerkCategoryData();

                    CategoryName = string.Empty;
                }
            }
        }

        #endregion

        #region Методы

        private void SelectImagePerk()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Image image = Image.FromFile(openFileDialog.FileName))
                {
                    ImagePerk = ImageHelper.ImageToByteArray(image);
                }
            }
        }

        #endregion
    }
}