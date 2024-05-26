using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel
{
    public class AddKillerWindowViewModel : BaseViewModel
    {

        #region Свойства для Киллера

        public ObservableCollection<Killer> KillerList {  get; set; }

        private Killer _selectedKillerItem;
        public Killer SelectedKillerItem
        {
            get => _selectedKillerItem;
            set
            {
                _selectedKillerItem = value;
                if (value == null) { return; }
                TextBoxKillerName = value.KillerName;
                ComboBoxSelectedKillerName = value;
                ImageKiller = value.KillerImage;
                ImageKillerAbility = value.KillerAbilityImage;
                //KillerAddonList.Clear();

                TextBoxKillerAddonName = string.Empty;
                ImageKillerAddon = null;  
                GetKillerAddonData();
            }
        }

        private string _textBoxKillerName;
        public string TextBoxKillerName
        {
            get => _textBoxKillerName;
            set
            {
                _textBoxKillerName = value;
                OnPropertyChanged();
            }
        }

        private byte[] _imageKiller;
        public byte[] ImageKiller
        {
            get => _imageKiller;
            set
            {
                _imageKiller = value;
                OnPropertyChanged();
            }
        }

        private byte[] _imageKillerAbility;
        public byte[] ImageKillerAbility
        {
            get => _imageKillerAbility;
            set
            {
                _imageKillerAbility = value;
                OnPropertyChanged();
            }
        }


        private byte[] _imageKillerAddon;
        public byte[] ImageKillerAddon
        {
            get => _imageKillerAddon;
            set
            {
                _imageKillerAddon = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства для Аддонов

        public ObservableCollection<KillerAddon> KillerAddonList { get; set; }

        private KillerAddon _selectedKillerAddonItem;
        public KillerAddon SelectedKillerAddonItem
        {
            get => _selectedKillerAddonItem;
            set
            {
                _selectedKillerAddonItem = value;
                if (value == null) { return; }
                TextBoxKillerAddonName = value.AddonName;
                ImageKillerAddon = value.AddonImage;
            }
        }

        private Killer _comboBoxSelectedKillerName;
        public Killer ComboBoxSelectedKillerName
        {
            get => _comboBoxSelectedKillerName;
            set
            {
                _comboBoxSelectedKillerName = value;
                if (value == null) { return; }
                //KillerAddonList.Clear();
                GetKillerAddonData();
                OnPropertyChanged();
            }
        }

        private string _textBoxKillerAddonName;
        public string TextBoxKillerAddonName
        {
            get => _textBoxKillerAddonName;
            set
            {
                _textBoxKillerAddonName = value;
                OnPropertyChanged();
            }
        }

        private string _textboxKillerAddonDescription;
        public string TextboxKillerAddonDescription
        {
            get => _textboxKillerAddonDescription;
            set
            {
                _textboxKillerAddonDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public AddKillerWindowViewModel() 
        {
            KillerList = new ObservableCollection<Killer>();
            KillerAddonList = new ObservableCollection<KillerAddon>();

            GetKillerData();
            GetKillerAddonData();
        }

        #region Команды 

        private RelayCommand _selectImageKillerCommand;
        public RelayCommand SelectImageKillerCommand { get => _selectImageKillerCommand ??= new(obj => { SelectImageKiller(); }); }

        private RelayCommand _selectImageKillerAbilityCommand;
        public RelayCommand SelectImageKillerAbilityCommand { get => _selectImageKillerAbilityCommand ??= new(obj => { SelectImageKillerAbility();}); }

        private RelayCommand _selectImageKillerAddonCommand;
        public RelayCommand SelectImageKillerAddonCommand { get => _selectImageKillerAddonCommand ??= new(obj => { SelectImageKillerAddon(); }); }

        private RelayCommand _saveKillerCommand;
        public RelayCommand SaveKillerCommand { get => _saveKillerCommand ??= new(obj => { AddKiller(); }); }

        private RelayCommand _updateKillerCommand;
        public RelayCommand UpdateKillerCommand { get => _updateKillerCommand ??= new(obj => { UpdateKiller(); }); }

        private RelayCommand _saveKillerAddonCommand;
        public RelayCommand SaveKillerAddonCommand { get => _saveKillerAddonCommand ??= new(obj => { AddKillerAddon(); }); }

        private RelayCommand _updateKillerAddonCommand;
        public RelayCommand UpdateKillerAddonCommand { get => _updateKillerAddonCommand ??= new(obj => { UpdateKillerAddon(); }); }


        private RelayCommand _deleteKillerCommand;
        public RelayCommand DeleteKillerCommand { get => _deleteKillerCommand ??= new(obj => { DeleteKiller(); }); }

        private RelayCommand _deleteKillerAddonCommand;
        public RelayCommand DeleteKillerAddonCommand { get => _deleteKillerAddonCommand ??= new(obj => { DeleteKillerAddon(); }); }


        private RelayCommand _nullImageKillerCommand;
        public RelayCommand NullImageKillerCommand { get => _nullImageKillerCommand ??= new(obj => { ImageKiller = null; }); }

        private RelayCommand _nullImageKillerAbilityCommand;
        public RelayCommand NullImageKillerAbilityCommand { get => _nullImageKillerAbilityCommand ??= new(obj => { ImageKillerAbility = null; }); }

        private RelayCommand _nullImageKillerAddonCommand;
        public RelayCommand NullImageKillerAddonCommand { get => _nullImageKillerAddonCommand ??= new(obj => { ImageKillerAddon = null; }); }

        #endregion

        #region Методы получение данных 

        private async void GetKillerData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var killers = await context.Killers.ToListAsync();
                //var killers = await context.Killers.Skip(1).ToListAsync();

                foreach (var item in killers)
                {
                    KillerList.Add(item);
                }
            }
        }

        private async void GetKillerAddonData()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                List<KillerAddon> Addons = new();

                if (SelectedKillerItem == null)
                {
                    KillerAddonList.Clear();
                }
                else
                {
                    Addons = await context.KillerAddons.Where(ka => ka.IdKiller == SelectedKillerItem.IdKiller).ToListAsync();
                }

                KillerAddonList.Clear();

                foreach (var item in Addons)
                {
                    KillerAddonList.Add(item);
                }
            }
        }

        #endregion

        #region Методы добавление данных

        private void AddKiller()
        {
            var newKiller = new Killer { KillerName = TextBoxKillerName, KillerImage = ImageKiller, KillerAbilityImage = ImageKillerAbility };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Killers.Any(killer => killer.KillerName.ToLower() == newKiller.KillerName.ToLower());

                if (exists || string.IsNullOrEmpty(TextBoxKillerName))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.Killers.Add(newKiller);
                    context.SaveChanges();
                    KillerList.Clear();
                    GetKillerData();
                    TextBoxKillerName = string.Empty;
                    ImageKiller = null;
                    ImageKillerAbility = null;
                }
            }
        }

        private void AddKillerAddon()
        {
            var newKillerAddon = new KillerAddon { IdKiller = ComboBoxSelectedKillerName.IdKiller, AddonName = TextBoxKillerAddonName, AddonImage = ImageKillerAddon, AddonDescription = TextboxKillerAddonDescription };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.KillerAddons.Any(KA => KA.AddonName.ToLower() == newKillerAddon.AddonName.ToLower());

                if (exists || string.IsNullOrEmpty(TextBoxKillerAddonName))
                {
                    MessageBox.Show("Эта запись уже имеется, либо вы ничего не написали");
                }
                else
                {
                    context.KillerAddons.Add(newKillerAddon);
                    context.SaveChanges();

                    KillerAddonList.Clear();
                    GetKillerAddonData();

                    TextBoxKillerAddonName = string.Empty;
                    TextboxKillerAddonDescription = string.Empty;
                    ImageKillerAddon = null;
                }
            }
        }

        #endregion

        #region Методы удаление данных

        private void DeleteKiller()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var entityToDelete = context.Killers.Find(SelectedKillerItem.IdKiller);
                if (entityToDelete != null)
                {
                    context.RemoveRange(entityToDelete.KillerAddons);
                    context.RemoveRange(entityToDelete.KillerInfos);
                    context.RemoveRange(entityToDelete.KillerPerks);
                    //context.RemoveRange(context.KillerAddons.Where(ka => ka.IdKiller == SelectedKillerItem.IdKiller));

                    context.Remove(entityToDelete);
                    context.SaveChanges();
                    KillerList.Clear();
                    GetKillerData();
                }
            }            
        }

        private void DeleteKillerAddon()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var entityToDelete = context.KillerAddons.Find(SelectedKillerAddonItem.IdKillerAddon);
                if (entityToDelete != null)
                {
                    context.Remove(entityToDelete);
                    context.SaveChanges();
                    KillerAddonList.Clear();
                    GetKillerAddonData();
                }
            }
        }

        #endregion

        #region Методы обновление данных 

        private void UpdateKiller()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedKillerItem == null)
                {
                    return;
                }

                var entityToUpdate = context.Killers.Find(SelectedKillerItem.IdKiller);

                if (entityToUpdate != null)
                {
                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedKillerItem.KillerName} на {TextBoxKillerName} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.KillerName = TextBoxKillerName;
                        entityToUpdate.KillerImage = ImageKiller;
                        entityToUpdate.KillerAbilityImage = ImageKillerAbility;
                        context.SaveChanges();

                        KillerList.Clear();
                        GetKillerData();

                        SelectedKillerItem = null;
                        TextBoxKillerName = string.Empty;
                        ImageKiller = null;
                        ImageKillerAbility = null;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
            }
        }

        private void UpdateKillerAddon()
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                if (SelectedKillerAddonItem == null)
                {
                    return;
                }

                var entityToUpdate = context.KillerAddons.Find(SelectedKillerAddonItem.IdKillerAddon);

                if (entityToUpdate != null)
                {
                    if (MessageBox.Show($"Вы точно хотите изменить {SelectedKillerAddonItem.AddonName} на {TextBoxKillerAddonName} ?",
                        "Предупреждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.IdKiller = ComboBoxSelectedKillerName.IdKiller;
                        entityToUpdate.AddonName = TextBoxKillerAddonName;
                        entityToUpdate.AddonImage = ImageKillerAddon;
                        entityToUpdate.AddonDescription = TextboxKillerAddonDescription;

                        context.SaveChanges();

                        //KillerAddonList.Clear();
                        GetKillerAddonData();

                        TextBoxKillerAddonName = string.Empty;
                        ImageKillerAddon = null;
                    }
                }
                else { MessageBox.Show("Нечего обновлять"); }
            }
        }

        #endregion

        #region Методы выбора изображения 

        private void SelectImageKiller()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Image image = Image.FromFile(openFileDialog.FileName))
                {
                    ImageKiller = ImageToByteArray(image);
                }
            }
        }

        private void SelectImageKillerAbility()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Image image = Image.FromFile(openFileDialog.FileName))
                {
                    ImageKillerAbility = ImageToByteArray(image);
                }
            }
        }

        private void SelectImageKillerAddon()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Image image = Image.FromFile(openFileDialog.FileName))
                {
                    ImageKillerAddon = ImageToByteArray(image);
                }
            }
        }

        private static byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream memoryStream = new())
            {
                image.Save(memoryStream, ImageFormat.Png);

                return memoryStream.ToArray();
            }
        }

        #endregion
    }
}
