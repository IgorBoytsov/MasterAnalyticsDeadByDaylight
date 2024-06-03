using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel
{
    public class AddOfferingWindowViewModel : BaseViewModel
    {
        #region Свойства

        public ObservableCollection<Role> RoleList { get; set; }

        public ObservableCollection<Offering> OfferingList { get; set; }

        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                if (value == null) { return; }
                OfferingList.Clear();
                GetOfferingData();

                SearchTextBox = string.Empty;
                OfferingNameTextBox = string.Empty;
                OfferingDescriptionTextBox = string.Empty;
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
                _selectedOffering = value;
                if (value == null) { return; }
                OfferingNameTextBox = value.OfferingName;
                OfferingDescriptionTextBox = value.OfferingDescription;
                ImageOffering = value.OfferingImage;
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
        public byte[] ImageOffering
        {
            get => _imageOffering;
            set
            {
                _imageOffering = value;
                OnPropertyChanged();
            }
        }
     
        private string _offeringNameTextBox;
        public string OfferingNameTextBox
        {
            get => _offeringNameTextBox;
            set
            {
                _offeringNameTextBox = value;
                OnPropertyChanged();
            }
        }

        private string _offeringDescriptionTextBox;
        public string OfferingDescriptionTextBox
        {
            get => _offeringDescriptionTextBox;
            set
            {
                _offeringDescriptionTextBox = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Rarity> RarityList { get; set; }

        private Rarity _comboBoxSelectedRarity;
        public Rarity ComboBoxSelectedRarity
        {
            get => _comboBoxSelectedRarity;
            set
            {
                _comboBoxSelectedRarity = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public AddOfferingWindowViewModel() 
        {
            RoleList = new ObservableCollection<Role>();
            OfferingList = new ObservableCollection<Offering>();
            RarityList = new ObservableCollection<Rarity>();

            GetOfferingData();
            GetRoleData();
            GetRaritiData();

            SelectedRole = RoleList.FirstOrDefault();
        }

        #region Команды

        private RelayCommand _addOfferingCommand;
        public RelayCommand AddOfferingCommand { get => _addOfferingCommand ??= new(obj => { AddOffering(); }); }     

        private RelayCommand _deleteOfferingCommand;
        public RelayCommand DeleteOfferingCommand { get => _deleteOfferingCommand ??= new(obj => { DeleteOffering(); }); }

        private RelayCommand _updateOfferingCommand;
        public RelayCommand UpdateOfferingCommand { get => _updateOfferingCommand ??= new(obj => { UpdateOffering(); }); }

        private RelayCommand _selectImageOfferingCommand;
        public RelayCommand SelectImageOfferingCommand { get => _selectImageOfferingCommand ??= new(obj => { SelectImageOffering(); }); }

        #endregion

        #region Методы

        private async void GetOfferingData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {

                List<Offering> offering = new();

                if (SelectedRole == null)
                {
                    OfferingList.Clear();
                }
                else
                {
                    offering = await context.Offerings.Include(rarity => rarity.IdRarityNavigation).Where(ia => ia.IdRole == SelectedRole.IdRole).ToListAsync();
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
            }
        }

        private async void GetRaritiData()
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
            var newOffering = new Offering() { IdRole = SelectedRole.IdRole, OfferingName = OfferingNameTextBox, OfferingDescription = OfferingDescriptionTextBox, IdRarity = ComboBoxSelectedRarity.IdRarity};

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (string.IsNullOrWhiteSpace(OfferingNameTextBox)) { return; }
                bool exists = context.Offerings.Where(off => off.IdRole == SelectedRole.IdRole).Any(off => off.OfferingName.ToLower() == newOffering.OfferingName.ToLower());

                if (exists || string.IsNullOrWhiteSpace(OfferingNameTextBox))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.Offerings.Add(newOffering);
                    context.SaveChanges();
                    OfferingList.Clear();
                    GetOfferingData();
                    OfferingNameTextBox = string.Empty;
                    OfferingDescriptionTextBox = string.Empty;
                    SelectedOffering = null;
                }
            }
        }

        private void DeleteOffering()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var itemToDelete = context.Offerings.Find(SelectedOffering.IdOffering);
                if (itemToDelete != null)
                {
                    context.Offerings.Remove(itemToDelete);
                    context.SaveChanges();
                    OfferingList.Clear();
                    GetOfferingData();
                }
            }
        }

        private void UpdateOffering()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedOffering == null) { return; }

                var entityToUpdate = context.Offerings.Find(SelectedOffering.IdOffering);

                if (entityToUpdate != null)
                {
                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedOffering.OfferingName} на {OfferingNameTextBox} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.OfferingName = OfferingNameTextBox;
                        entityToUpdate.OfferingDescription = OfferingDescriptionTextBox;
                        entityToUpdate.OfferingImage = ImageOffering;
                        entityToUpdate.IdRole = SelectedRole.IdRole;
                        entityToUpdate.IdRarity = ComboBoxSelectedRarity.IdRarity;
                        context.SaveChanges();
                        OfferingList.Clear();
                        GetOfferingData();

                        OfferingNameTextBox = string.Empty;
                        OfferingDescriptionTextBox = string.Empty;
                        ImageOffering = null;
                        SelectedOffering = null;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
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
                    ImageOffering = ImageHelper.ImageToByteArray(image);
                }
            }
        }

        #endregion
    }
}
