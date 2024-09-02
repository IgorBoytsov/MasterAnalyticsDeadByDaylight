using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.View.Pages;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    public class AddKillerWindowViewModel : BaseViewModel
    {

        #region Свойства для Киллера

        public ObservableCollection<Killer> KillerList { get; set; } = [];

        private Killer _selectedKillerItem;
        public Killer SelectedKillerItem
        {
            get => _selectedKillerItem;
            set
            {
                if (value != null)
                {
                    _selectedKillerItem = value;
                    KillerName = value.KillerName;
                    ImageKiller = value.KillerImage;
                    ImageKillerAbility = value.KillerAbilityImage;

                    KillerAddonName = string.Empty;
                    ImageKillerAddon = null;
                    GetKillerAddonData();
                }
                else
                {
                    return;
                }
                
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
                if (value != null)
                {
                    _selectedKillerAddonItem = value;
                    KillerAddonName = value.AddonName;
                    ImageKillerAddon = value.AddonImage;
                    SelectedRarity = RarityList.FirstOrDefault(x => x.IdRarity == value.IdRarity);
                }
                else
                {
                    return;
                }
               
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

        private readonly IDialogService _dialogService;

        public AddKillerWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            GetRarityData();
            GetKillerData();
            GetKillerAddonData();
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
            if (SelectedKillerItem == null)
            {
                return;
            }
            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedKillerItem.KillerName}»? При удаление данном записи, могут быть связанные записи в других таблицах, что может привести к проблемам.",
                "Предупреждение об удаление.",
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await DataBaseHelper.DeleteEntityAsync(SelectedKillerItem);
                GetKillerData();
            }
            else
            {
                return;
            }
        }); }

        private RelayCommand _deleteKillerAddonCommand;
        public RelayCommand DeleteKillerAddonCommand { get => _deleteKillerAddonCommand ??= new(async obj => 
        {
            if (SelectedKillerAddonItem == null)
            {
                return;
            }
            if (_dialogService.ShowMessageButtons(
                $"Вы точно хотите удалить «{SelectedKillerAddonItem.AddonName}»?",
                "Предупреждение об удаление.",
                TypeMessage.Warning, MessageButtons.YesNo) == MessageButtons.Yes)
            {
                await DataBaseHelper.DeleteEntityAsync(SelectedKillerAddonItem);
                GetKillerAddonData();
            }
            else
            {
                return;
            }
        }); }

        #endregion

        #region Методы получение данных 

        private async void GetKillerData()
        {
            KillerList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                var killers = await context.Killers.Skip(1).ToListAsync();

                foreach (var item in killers)
                {
                    KillerList.Add(item);
                }
                KillerList.Add(context.Killers.FirstOrDefault(x => x.KillerName == "Общий"));
            }
        }

        private async void GetKillerAddonData()
        {
            KillerAddonList.Clear();
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                List<KillerAddon> Addons = new();

                if (SelectedKillerItem == null)
                {
                    KillerAddonList.Clear();
                }
                else
                {
                    Addons = await context.KillerAddons.Where(ka => ka.IdKiller == SelectedKillerItem.IdKiller).OrderBy(x => x.IdRarity).ToListAsync();
                }

                foreach (var item in Addons)
                {
                    KillerAddonList.Add(item);
                }
                SelectedKillerAddonItem = KillerAddonList.FirstOrDefault(x => x.IdKiller == SelectedKillerItem.IdKiller);
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
        #endregion

        #region Методы добавление данных

        private void AddKiller()
        {
            var newKiller = new Killer { KillerName = KillerName, KillerImage = ImageKiller, KillerAbilityImage = ImageKillerAbility };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.Killers.Any(killer => killer.KillerName.ToLower() == newKiller.KillerName.ToLower());

                if (exists || string.IsNullOrEmpty(KillerName))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали", "Совпадение имени");
                }
                else
                {
                    context.Killers.Add(newKiller);
                    context.SaveChanges();
                    KillerList.Clear();
                    GetKillerData();
                    KillerName = string.Empty;
                    ImageKiller = null;
                }
            }
        }

        private void AddKillerAddon()
        {
            if (SelectedKillerItem == null)
            {
                _dialogService.ShowMessage("Вы не выбрали персонажа, которому будете добавлять аддоны");
                return;
            }
            var newKillerAddon = new KillerAddon { IdKiller = SelectedKillerItem.IdKiller, IdRarity = SelectedRarity.IdRarity ,AddonName = KillerAddonName, AddonImage = ImageKillerAddon, AddonDescription = KillerAddonDescription };

            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                bool exists = context.KillerAddons.Any(KA => KA.AddonName.ToLower() == newKillerAddon.AddonName.ToLower());

                if (exists || string.IsNullOrEmpty(KillerAddonName))
                {
                    _dialogService.ShowMessage("Эта запись уже имеется, либо вы ничего не написали", "Совпадение имени");
                }
                else
                {
                    context.KillerAddons.Add(newKillerAddon);
                    context.SaveChanges();

                    KillerAddonList.Clear();
                    GetKillerAddonData();

                    KillerAddonName = string.Empty;
                    KillerAddonDescription = string.Empty;
                    ImageKillerAddon = null;
                    SelectedRarity = null;
                }
            }
        }

        #endregion

        #region Методы удаление данных

        //private void DeleteKiller()
        //{
        //    using (MasterAnalyticsDeadByDaylightDbContext context = new())
        //    {
        //        var entityToDelete = context.Killers.Find(SelectedKillerItem.IdKiller);
        //        if (entityToDelete != null)
        //        {
        //            context.RemoveRange(entityToDelete.KillerAddons);
        //            context.RemoveRange(entityToDelete.KillerInfos);
        //            context.RemoveRange(entityToDelete.KillerPerks);
        //            //context.RemoveRange(context.KillerAddons.Where(ka => ka.IdKiller == SelectedKillerItem.IdKiller));

        //            context.Remove(entityToDelete);
        //            context.SaveChanges();
        //            KillerList.Clear();
        //            GetKillerData();
        //        }
        //    }
        //}

        //private void DeleteKillerAddon()
        //{
        //    using (MasterAnalyticsDeadByDaylightDbContext context = new())
        //    {
        //        var entityToDelete = context.KillerAddons.Find(SelectedKillerAddonItem.IdKillerAddon);
        //        if (entityToDelete != null)
        //        {
        //            context.Remove(entityToDelete);
        //            context.SaveChanges();
        //            KillerAddonList.Clear();
        //            GetKillerAddonData();
        //        }
        //    }
        //}

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
                    bool exists = context.Killers.Any(x => x.KillerName.ToLower() == KillerName.ToLower());

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
                            context.SaveChanges();

                            KillerList.Clear();
                            GetKillerData();

                            SelectedKillerItem = null;
                            KillerName = string.Empty;
                            ImageKiller = null;
                            ImageKillerAbility = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.KillerName = KillerName;
                        entityToUpdate.KillerImage = ImageKiller;
                        entityToUpdate.KillerAbilityImage = ImageKillerAbility;
                        context.SaveChanges();

                        KillerList.Clear();
                        GetKillerData();

                        SelectedKillerItem = null;
                        KillerName = string.Empty;
                        ImageKiller = null;
                        ImageKillerAbility = null; 
                    }
                }
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
                    bool exists = context.KillerAddons.Any(x => x.AddonName.ToLower() == KillerAddonName.ToLower());

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
                            context.SaveChanges();
                            GetKillerAddonData();

                            KillerAddonName = string.Empty;
                            ImageKillerAddon = null;
                            SelectedRarity = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        entityToUpdate.IdKiller = SelectedKillerItem.IdKiller;
                        entityToUpdate.AddonName = KillerAddonName;
                        entityToUpdate.AddonImage = ImageKillerAddon;
                        entityToUpdate.AddonDescription = KillerAddonDescription;
                        context.SaveChanges();
                        GetKillerAddonData();

                        KillerAddonName = string.Empty;
                        ImageKillerAddon = null;
                        SelectedRarity = null;
                    } 
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
