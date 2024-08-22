using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.View.Pages;
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

        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (value == null) { return; }
                _selectedRole = value;
                OfferingList.Clear();
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

        IDialogService _dialogService;

        public AddOfferingWindowViewModel(IDialogService service)
        {
            _dialogService = service;
            Title = "Добавление подношение";
            GetOfferingData();
            GetRoleData();
            GetRarityData();
        }

        #region Команды

        private RelayCommand _addOfferingCommand;
        public RelayCommand AddOfferingCommand { get => _addOfferingCommand ??= new(obj => { AddOffering(); }); }

        private RelayCommand _deleteOfferingCommand;
        public RelayCommand DeleteOfferingCommand { get => _deleteOfferingCommand ??= new(async obj => 
        {
            if (SelectedOffering == null)
            {
                return;
            }
            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedOffering.OfferingName}»? При удаление данном записи, могут быть связанные записи в других таблицах, что может привести к проблемам.",
                "Предупреждение об удаление.",
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await DataBaseHelper.DeleteEntityAsync(SelectedOffering);
                GetOfferingData();
            }
            else
            {
                return;
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
            OfferingList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {

                List<Offering> offering = new();

                if (SelectedRole == null)
                {
                    OfferingList.Clear();
                }
                else
                {
                    offering = await context.Offerings.Include(rarity => rarity.IdRarityNavigation).Where(ia => ia.IdRole == SelectedRole.IdRole).OrderBy(x => x.IdRarity).ToListAsync();
                }

                OfferingList.Clear();

                foreach (var item in offering)
                {
                    OfferingList.Add(item);
                }

                //if (SelectedRole == null) 
                //{
                //    return;
                //}

                //var offerings = await context.Offerings.Include(offering => offering.IdRarityNavigation).Where(off => off.IdRole == SelectedRole.IdRole).ToListAsync();             
                //foreach (var item in offerings)
                //{
                //    OfferingList.Add(item);
                //}
            }
        }

        private async void GetRoleData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var offerings = await context.Roles.ToListAsync();

                foreach (var item in offerings)
                {
                    RoleList.Add(item);
                }
                SelectedRole = RoleList.FirstOrDefault();
            }
        }

        private async void GetRarityData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var rarities = await context.Rarities.ToListAsync();

                foreach (var item in rarities)
                {
                    RarityList.Add(item);
                }
            }
        }

        private void AddOffering()
        {

            var newOffering = new Offering() { IdRole = SelectedRole.IdRole, OfferingName = OfferingName, OfferingImage = OfferingImage, OfferingDescription = OfferingDescription, IdRarity = SelectedRarity.IdRarity };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Offerings.Where(off => off.IdRole == SelectedRole.IdRole).Any(off => off.OfferingName.ToLower() == newOffering.OfferingName.ToLower());

                if (exists || string.IsNullOrEmpty(OfferingName))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали", "Совпадение имени");
                }
                else
                {
                    context.Offerings.Add(newOffering);
                    context.SaveChanges();
                    GetOfferingData();
                    OfferingName = string.Empty;
                    OfferingDescription = string.Empty;
                    SelectedOffering = null;
                }
            }
        }

        private void UpdateOffering()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedOffering == null)
                {
                    return;
                }

                var entityToUpdate = context.Offerings.Find(SelectedOffering.IdOffering);

                if (entityToUpdate != null)
                {
                    bool exists = context.Offerings.Where(off => off.IdRole == SelectedRole.IdRole).Any(x => x.OfferingName.ToLower() == OfferingName.ToLower());

                    if (exists)
                    {
                        if (_dialogService.ShowMessageButtons(
                           $"Вы точно хотите обновить ее? Если да, то будет произведена замена имени с «{SelectedOffering.OfferingName}» на «{OfferingName}» ?",
                           $"Надпись с именем «{SelectedOffering.OfferingName}» уже существует.",
                           TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                        {
                            entityToUpdate.OfferingName = OfferingName;
                            entityToUpdate.OfferingDescription = OfferingDescription;
                            entityToUpdate.OfferingImage = OfferingImage;
                            entityToUpdate.IdRole = SelectedRole.IdRole;
                            entityToUpdate.IdRarity = SelectedRarity.IdRarity;
                            context.SaveChanges();
                            GetOfferingData();

                            OfferingName = string.Empty;
                            OfferingDescription = string.Empty;
                            OfferingImage = null;
                            SelectedOffering = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.OfferingName = OfferingName;
                        entityToUpdate.OfferingDescription = OfferingDescription;
                        entityToUpdate.OfferingImage = OfferingImage;
                        entityToUpdate.IdRole = SelectedRole.IdRole;
                        entityToUpdate.IdRarity = SelectedRarity.IdRarity;
                        context.SaveChanges();
                        GetOfferingData();

                        OfferingName = string.Empty;
                        OfferingDescription = string.Empty;
                        OfferingImage = null;
                        SelectedOffering = null;
                    }
                }
            }
        }

        private void SearchOffering()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var search = context.Offerings.Where(off => off.IdRole == SelectedRole.IdRole).Where(off => off.OfferingName.Contains(SearchTextBox));
                OfferingList.Clear();

                foreach (var item in search)
                {
                    OfferingList.Add(item);
                }
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
