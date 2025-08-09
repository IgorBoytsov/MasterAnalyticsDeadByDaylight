using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    public class AddKillerWindowViewModel : BaseViewModel, IUpdatable
    {

        #region Свойства для Киллера

        public ObservableCollection<Killer> KillerList { get; set; } = [];

        private Killer _selectedKillerItem;
        public Killer SelectedKillerItem
        {
            get => _selectedKillerItem;
            set
            {
                if (value == null) return;

                _selectedKillerItem = value;
                KillerName = value.KillerName;
                ImageKiller = value.KillerImage;
                ImageKillerAbility = value.KillerAbilityImage;

                KillerAddonName = string.Empty;
                ImageKillerAddon = null;
                GetKillerAddonData();
            }
        }

        private string _killerName;
        public string KillerName
        {
            get => _killerName;
            set
            {
                _killerName = value;
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

        public ObservableCollection<KillerAddon> KillerAddonList { get; set; } = [];

        private KillerAddon _selectedKillerAddonItem;
        public KillerAddon SelectedKillerAddonItem
        {
            get => _selectedKillerAddonItem;
            set
            {
                if (value == null) return;

                _selectedKillerAddonItem = value;
                KillerAddonName = value.AddonName;
                ImageKillerAddon = value.AddonImage;
                SelectedRarity = RarityList.FirstOrDefault(x => x.IdRarity == value.IdRarity);
            }
        }

        private string _killerAddonName;
        public string KillerAddonName
        {
            get => _killerAddonName;
            set
            {
                _killerAddonName = value;
                OnPropertyChanged();
            }
        }

        private string _killerAddonDescription;
        public string KillerAddonDescription
        {
            get => _killerAddonDescription;
            set
            {
                _killerAddonDescription = value;
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

        #endregion

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

        private readonly IServiceProvider _serviceProvider;

        private readonly ICustomDialogService _dialogService;
        private readonly IDataService _dataService;

        public AddKillerWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dialogService = _serviceProvider.GetService<ICustomDialogService>();
            _dataService = _serviceProvider.GetService<IDataService>();

            GetRarityData();
            GetKillerData();
            GetKillerAddonData();
        }

        public void Update(object value)
        {
            throw new NotImplementedException();
        }

        #region Команды 

        private RelayCommand _selectImageKillerCommand;
        public RelayCommand SelectImageKillerCommand { get => _selectImageKillerCommand ??= new(obj => { SelectImageKiller(); }); }
        
        private RelayCommand _clearKillerImageCommand;
        public RelayCommand ClearKillerImageCommand { get => _clearKillerImageCommand ??= new(obj => { ImageKiller = null; }); }

        private RelayCommand _clearKillerAbilityImageCommand;
        public RelayCommand ClearKillerAbilityImageCommand { get => _clearKillerAbilityImageCommand ??= new(obj => { ImageKillerAbility = null; }); }

        private RelayCommand _selectImageKillerAbilityCommand;
        public RelayCommand SelectImageKillerAbilityCommand { get => _selectImageKillerAbilityCommand ??= new(obj => { SelectImageKillerAbility(); }); }

        private RelayCommand _selectImageKillerAddonCommand;
        public RelayCommand SelectImageKillerAddonCommand { get => _selectImageKillerAddonCommand ??= new(obj => { SelectImageKillerAddon(); }); }    
        
        private RelayCommand _clearAddonImageCommand;
        public RelayCommand ClearAddonImageCommand { get => _clearAddonImageCommand ??= new(obj => { ImageKillerAddon = null; }); }

        private RelayCommand _saveKillerCommand;
        public RelayCommand SaveKillerCommand { get => _saveKillerCommand ??= new(obj => { AddKiller(); }); }

        private RelayCommand _updateKillerCommand;
        public RelayCommand UpdateKillerCommand { get => _updateKillerCommand ??= new(obj => { UpdateKiller(); }); }

        private RelayCommand _saveKillerAddonCommand;
        public RelayCommand SaveKillerAddonCommand { get => _saveKillerAddonCommand ??= new(obj => { AddKillerAddon(); }); }

        private RelayCommand _updateKillerAddonCommand;
        public RelayCommand UpdateKillerAddonCommand { get => _updateKillerAddonCommand ??= new(obj => { UpdateKillerAddon(); }); }


        private RelayCommand _deleteKillerCommand;
        public RelayCommand DeleteKillerCommand { get => _deleteKillerCommand ??= new(async obj => 
        {
            if (SelectedKillerItem == null) return;

            if (MessageHelper.MessageDelete(SelectedKillerItem.KillerName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedKillerItem);
                GetKillerData();
            }
        }); }

        private RelayCommand _deleteKillerAddonCommand;
        public RelayCommand DeleteKillerAddonCommand { get => _deleteKillerAddonCommand ??= new(async obj => 
        {
            if (SelectedKillerAddonItem == null) return;
            if (MessageHelper.MessageDelete(SelectedKillerAddonItem.AddonName) == MessageButtons.Yes)
            {
                await _dataService.RemoveAsync(SelectedKillerAddonItem);
                GetKillerAddonData();
            }
        }); }

        #endregion

        #region Методы получение данных 

        private async void GetKillerData()
        {
            KillerList.Clear();
            var killers = await _dataService.GetAllDataAsync<Killer>();
            foreach (var item in killers.Skip(1))
            {
                KillerList.Add(item);
            }
            KillerList.Add(killers.FirstOrDefault(x => x.KillerName == "Общий"));
        }

        private async void GetKillerAddonData()
        {
            KillerAddonList.Clear();
            if (SelectedKillerItem == null) KillerAddonList.Clear();
            else
            {
                var addon = await _dataService.GetAllDataAsync<KillerAddon>(x => x.Where(x => x.IdKiller == SelectedKillerItem.IdKiller).OrderBy(x => x.IdRarity));

                foreach (var item in addon)
                {
                    KillerAddonList.Add(item);
                }
                SelectedKillerAddonItem = KillerAddonList.FirstOrDefault(x => x.IdKiller == SelectedKillerItem.IdKiller);
            }
        }

        private async void GetRarityData()
        {
            var rarities = await _dataService.GetAllDataAsync<Rarity>();

            foreach (var item in rarities)
            {
                RarityList.Add(item);
            }
        }

        #endregion

        #region Методы добавление данных

        private async void AddKiller()
        {
            var newKiller = new Killer { KillerName = KillerName, KillerImage = ImageKiller, KillerAbilityImage = ImageKillerAbility };

            bool exists = await _dataService.ExistsAsync<Killer>(x => x.KillerName.ToLower() == newKiller.KillerName.ToLower()); 

            if (exists || string.IsNullOrEmpty(KillerName)) MessageHelper.MessageExist();
            else
            {
                await _dataService.AddAsync(newKiller);;

                KillerList.Clear();
                GetKillerData();

                KillerName = string.Empty;
                ImageKiller = null;
            }
        }

        private async void AddKillerAddon()
        {
            if (SelectedKillerItem == null)
            {
                _dialogService.ShowMessage("Вы не выбрали персонажа, которому будете добавлять аддоны");
                return;
            }
            var newKillerAddon = new KillerAddon { IdKiller = SelectedKillerItem.IdKiller, IdRarity = SelectedRarity.IdRarity ,AddonName = KillerAddonName, AddonImage = ImageKillerAddon, AddonDescription = KillerAddonDescription };

            bool exists = await _dataService.ExistsAsync<KillerAddon>(x => x.AddonName.ToLower() == newKillerAddon.AddonName.ToLower());

            if (exists || string.IsNullOrEmpty(KillerAddonName))
            {
                MessageHelper.MessageExist();
            }
            else
            {
                await _dataService.AddAsync(newKillerAddon);

                KillerAddonList.Clear();
                GetKillerAddonData();

                KillerAddonName = string.Empty;
                KillerAddonDescription = string.Empty;
                ImageKillerAddon = null;
                SelectedRarity = null;
            }
        }

        #endregion

        #region Методы обновление данных 

        private async void UpdateKiller()
        {
            if (SelectedKillerItem == null) return;

            var entityToUpdate = await _dataService.FindAsync<Killer>(SelectedKillerItem.IdKiller);

            if (entityToUpdate != null)
            {
                bool exists = await _dataService.ExistsAsync<Killer>(x => x.KillerName.ToLower() == KillerName.ToLower());

                if (exists)
                {
                    if (_dialogService.ShowMessageButtons(
                       $"Вы точно хотите обновить ее? Если да, то будет произведена замена имени с «{SelectedKillerItem.KillerName}» на «{KillerName}» ?",
                       $"Надпись с именем «{SelectedKillerItem.KillerName}» уже существует.",
                       TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                    {
                        entityToUpdate.KillerName = KillerName;
                        entityToUpdate.KillerImage = ImageKiller;
                        entityToUpdate.KillerAbilityImage = ImageKillerAbility;
                        await _dataService.UpdateAsync(entityToUpdate);

                        KillerList.Clear();
                        GetKillerData();

                        SelectedKillerItem = null;
                        KillerName = string.Empty;
                        ImageKiller = null;
                        ImageKillerAbility = null;
                    }
                }
                else
                {
                    entityToUpdate.KillerName = KillerName;
                    entityToUpdate.KillerImage = ImageKiller;
                    entityToUpdate.KillerAbilityImage = ImageKillerAbility;
                    await _dataService.UpdateAsync(entityToUpdate);
                    
                    KillerList.Clear();
                    GetKillerData();

                    SelectedKillerItem = null;
                    KillerName = string.Empty;
                    ImageKiller = null;
                    ImageKillerAbility = null;
                }
            }
        }

        private async void UpdateKillerAddon()
        {
            if (SelectedKillerAddonItem == null) return;

            var entityToUpdate = await _dataService.FindAsync<KillerAddon>(SelectedKillerAddonItem.IdKillerAddon);

            if (entityToUpdate != null)
            {
                bool exists = await _dataService.ExistsAsync<KillerAddon>(x => x.AddonName.ToLower() == KillerAddonName.ToLower());

                if (exists)
                {
                    if (_dialogService.ShowMessageButtons(
                       $"Вы точно хотите обновить ее? Если да, то будет произведена замена имени с «{SelectedKillerAddonItem.AddonName}» на «{KillerName}» ?",
                       $"Надпись с именем «{SelectedKillerAddonItem.AddonName}» уже существует.",
                       TypeMessage.Notification, MessageButtons.YesNoCancel) == MessageButtons.Yes)
                    {
                        entityToUpdate.IdKiller = SelectedKillerItem.IdKiller;
                        entityToUpdate.IdRarity = SelectedRarity.IdRarity;
                        entityToUpdate.AddonName = KillerAddonName;
                        entityToUpdate.AddonImage = ImageKillerAddon;
                        entityToUpdate.AddonDescription = KillerAddonDescription;
                        await _dataService.UpdateAsync<KillerAddon>(entityToUpdate);

                        GetKillerAddonData();
                        KillerAddonName = string.Empty;
                        ImageKillerAddon = null;
                        SelectedRarity = null;
                    }
                }
                else
                {
                    entityToUpdate.IdKiller = SelectedKillerItem.IdKiller;
                    entityToUpdate.AddonName = KillerAddonName;
                    entityToUpdate.AddonImage = ImageKillerAddon;
                    entityToUpdate.AddonDescription = KillerAddonDescription;
                    await _dataService.UpdateAsync<KillerAddon>(entityToUpdate);
                    GetKillerAddonData();

                    KillerAddonName = string.Empty;
                    ImageKillerAddon = null;
                    SelectedRarity = null;
                }
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
