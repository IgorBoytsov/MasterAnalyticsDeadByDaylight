using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    public class AddPerkWindowViewModel : BaseViewModel
    {
        #region Свойства

        public ObservableCollection<Killer> KillerList { get; set; }

        public ObservableCollection<Survivor> SurvivorList { get; set; }

        public ObservableCollection<Role> RoleList { get; set; }

        public ObservableCollection<Character> CharacterList { get; set; }

        public ObservableCollection<Perk> PerkList { get; set; }

        public ObservableCollection<KillerPerk> KillerPerkList { get; set; }

        public ObservableCollection<SurvivorPerk> SurvivorPerkList { get; set; }

        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                if (value == null) { return; }
                GetCharacterList();
                SelectedCharacter = CharacterList.First();
                OnPropertyChanged();
            }
        }

        private Character _selectedCharacter;
        public Character SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                _selectedCharacter = value;
                if (value == null) { return; }
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
                _selectedPerk = value;
                if (value == null) { return; }
                PerkNameTextBox = value.PerkName;
                ImagePerk = value.PerkImage;
                PerkDescriptionTextBox = value.PerkDescription;
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

        #endregion

        public AddPerkWindowViewModel()
        {
            KillerList = new ObservableCollection<Killer>();
            KillerPerkList = new ObservableCollection<KillerPerk>();
            SurvivorList = new ObservableCollection<Survivor>();
            SurvivorPerkList = new ObservableCollection<SurvivorPerk>();
            RoleList = new ObservableCollection<Role>();
            CharacterList = new ObservableCollection<Character>();
            PerkList = new ObservableCollection<Perk>();

            GetKillerData();
            GetSurvivorData();
            GetRoleData();
            GetPerkData();
        }

        #region Команды

        private RelayCommand _addPerkCommand;
        public RelayCommand AddPerkCommand { get => _addPerkCommand ??= new(obj => { AddPerk(); }); }

        private RelayCommand _deletePerkCommand;
        public RelayCommand DeletePerkCommand { get => _deletePerkCommand ??= new(obj => { DeletePerk(); }); }

        private RelayCommand _updatePerkCommand;
        public RelayCommand UpdatePerkCommand { get => _updatePerkCommand ??= new(obj => { UpdatePerk(); }); }

        private RelayCommand _selectImagePerkCommand;
        public RelayCommand SelectImagePerkCommand { get => _selectImagePerkCommand ??= new(obj => { SelectImagePerk(); }); }

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
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
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
        }

        private async void GetRoleData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var items = await context.Roles.ToListAsync();
                RoleList.Clear();

                foreach (var item in items)
                {
                    RoleList.Add(item);
                }
            }
        }

        private async void GetKillerData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var items = await context.Killers.ToListAsync();
                KillerList.Clear();

                foreach (var item in items)
                {
                    KillerList.Add(item);
                }
            }
        }

        private async void GetSurvivorData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var items = await context.Survivors.ToListAsync();
                SurvivorList.Clear();

                foreach (var item in items)
                {
                    SurvivorList.Add(item);
                }
            }
        }

        private async void GetKillerPerkData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedCharacter == null)
                {
                    var items = await context.KillerPerks.ToListAsync();

                    foreach (var item in items)
                    {
                        var perks = new Perk()
                        {
                            IdCharacter = item.IdKiller,
                            IdPerk = item.IdKillerPerk,
                            PerkName = item.PerkName,
                            PerkImage = item.PerkImage,
                            PerkDescription = item.PerkDescription
                        };

                        PerkList.Add(perks);
                    }
                }
                else
                {
                    var items = await context.KillerPerks.Where(kp => kp.IdKiller == SelectedCharacter.IdCharacter).ToListAsync();

                    foreach (var item in items)
                    {
                        var perks = new Perk()
                        {
                            IdCharacter = item.IdKiller,
                            IdPerk = item.IdKillerPerk,
                            PerkName = item.PerkName,
                            PerkImage = item.PerkImage,
                            PerkDescription = item.PerkDescription
                        };

                        PerkList.Add(perks);
                    }
                }
            }
        }

        private async void GetSurvivorPerkData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedCharacter == null)
                {
                    var items = await context.SurvivorPerks.ToListAsync();
                    foreach (var item in items)
                    {
                        var perks = new Perk()
                        {
                            IdCharacter = item.IdSurvivor,
                            IdPerk = item.IdSurvivorPerk,
                            PerkName = item.PerkName,
                            PerkImage = item.PerkImage,
                            PerkDescription = item.PerkDescription
                        };

                        PerkList.Add(perks);
                    }
                }
                else
                {
                    var items = await context.SurvivorPerks.Where(sp => sp.IdSurvivor == SelectedCharacter.IdCharacter).ToListAsync();
                    foreach (var item in items)
                    {
                        var perks = new Perk()
                        {
                            IdCharacter = item.IdSurvivor,
                            IdPerk = item.IdSurvivorPerk,
                            PerkName = item.PerkName,
                            PerkImage = item.PerkImage,
                            PerkDescription = item.PerkDescription
                        };

                        PerkList.Add(perks);
                    }
                }
            }
        }

        #endregion

        #region Методы добавление данных

        private void AddPerk()
        {
            if (SelectedRole == null) { MessageBox.Show("Вы не выбрали игровую роль"); return; }
            if (SelectedCharacter == null) { MessageBox.Show("Вы не выбрали персонажа"); return; }
            if (SelectedRole.RoleName == "Убийца") { AddKillerPerk(); }
            if (SelectedRole.RoleName == "Выживший") { AddSurvivorPerk(); }
        }

        private void AddSurvivorPerk()
        {
            var newPerk = new SurvivorPerk() { IdSurvivor = SelectedCharacter.IdCharacter, PerkName = PerkNameTextBox, PerkImage = ImagePerk, PerkDescription = PerkDescriptionTextBox };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (string.IsNullOrWhiteSpace(PerkNameTextBox)) { return; }

                bool exists = context.SurvivorPerks.Any(sp => sp.PerkName.ToLower() == newPerk.PerkName.ToLower());

                if (exists || string.IsNullOrWhiteSpace(PerkNameTextBox))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.SurvivorPerks.Add(newPerk);
                    context.SaveChanges();
                    GetPerkData();
                    PerkNameTextBox = string.Empty;
                    ImagePerk = null;
                }
            }
        }

        private void AddKillerPerk()
        {
            var newPerk = new KillerPerk() { IdKiller = SelectedCharacter.IdCharacter, PerkName = PerkNameTextBox, PerkImage = ImagePerk, PerkDescription = PerkDescriptionTextBox };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (string.IsNullOrWhiteSpace(PerkNameTextBox)) { return; }
                bool exists = context.KillerPerks.Any(kp => kp.PerkName.ToLower() == newPerk.PerkName.ToLower());

                if (exists || string.IsNullOrWhiteSpace(PerkNameTextBox))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.KillerPerks.Add(newPerk);
                    context.SaveChanges();
                    GetPerkData();
                    PerkNameTextBox = string.Empty;
                    ImagePerk = null;
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

        private void DeleteKillerPerk()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var itemToDelete = context.KillerPerks.Find(SelectedPerk.IdPerk);
                if (itemToDelete != null)
                {
                    context.KillerPerks.Remove(itemToDelete);
                    context.SaveChanges();
                    GetPerkData();
                }
            }
        }

        private void DeleteSurvivorPerk()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var itemToDelete = context.SurvivorPerks.Find(SelectedPerk.IdPerk);
                if (itemToDelete != null)
                {
                    context.SurvivorPerks.Remove(itemToDelete);
                    context.SaveChanges();
                    GetPerkData();
                }
            }
        }

        #endregion

        #region Методы обновление данных

        private void UpdatePerk()
        {
            if (SelectedPerk == null) { return; }
            if (SelectedRole.RoleName == "Общая") { MessageBox.Show("Вы не выбрали роль"); return; }
            if (SelectedCharacter == null) { MessageBox.Show("Вы не выбрали персонажа"); return; }

            if (SelectedRole.RoleName == "Убийца") { UpdateKillerPerk(); }
            if (SelectedRole.RoleName == "Выживший") { UpdateSurvivorPerk(); }
        }

        private void UpdateKillerPerk()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedPerk == null) { return; }

                var entityToUpdate = context.KillerPerks.Find(SelectedPerk.IdPerk);

                if (entityToUpdate != null)
                {
                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedPerk.PerkName} на {PerkNameTextBox} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.PerkName = PerkNameTextBox;
                        entityToUpdate.PerkImage = ImagePerk;
                        entityToUpdate.PerkDescription = PerkDescriptionTextBox;
                        context.SaveChanges();

                        GetPerkData();

                        PerkNameTextBox = string.Empty;
                        PerkDescriptionTextBox = string.Empty;
                        ImagePerk = null;
                        SelectedPerk = null;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
            }
        }

        private void UpdateSurvivorPerk()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedPerk == null) { return; }

                var entityToUpdate = context.SurvivorPerks.Find(SelectedPerk.IdPerk);

                if (entityToUpdate != null)
                {
                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedPerk.PerkName} на {PerkNameTextBox} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.PerkName = PerkNameTextBox;
                        entityToUpdate.PerkImage = ImagePerk;
                        entityToUpdate.PerkDescription = PerkDescriptionTextBox;
                        context.SaveChanges();

                        GetPerkData();

                        PerkNameTextBox = string.Empty;
                        PerkDescriptionTextBox = string.Empty;
                        ImagePerk = null;
                        SelectedPerk = null;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
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