using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Helpers;
using DBDAnalytics.WPF.Interfaces;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;

namespace DBDAnalytics.WPF.ViewModels.WindowVM
{
    internal class InteractionKillerVM : BaseVM, IUpdatable
    {
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IKillerService _killerService;
        private readonly IKillerAddonService _killerAddonService;
        private readonly IRarityService _rarityService;
        private readonly IKillerPerkService _killerPerkService;
        private readonly IKillerPerkCategoryService _killerPerkCategoryService;

        public InteractionKillerVM(IWindowNavigationService windowNavigationService,
                                   IKillerService killerService,
                                   IKillerAddonService killerAddonService,
                                   IRarityService rarityService,
                                   IKillerPerkService killerPerkService,
                                   IKillerPerkCategoryService killerPerkCategoryService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _killerService = killerService;
            _killerAddonService = killerAddonService;
            _rarityService = rarityService;
            _killerPerkService = killerPerkService;
            _killerPerkCategoryService = killerPerkCategoryService;

            Title = "Управление данными киллера.";

            GetKillersLoadout();
            GetRarities();
            GetPerkCategories();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            #region Добавление записей

            if (typeParameter == TypeParameter.AddAndNotification)
            {
                if (parameter is RarityDTO newRarity)
                    Rarities.Add(newRarity);

                if (parameter is KillerPerkCategoryDTO newCategory)
                    PerkCategories.Add(newCategory);
            }

            #endregion

            #region Обновление записей

            if (typeParameter == TypeParameter.UpdateAndNotification)
            {
                if (parameter is RarityDTO updateRarity)
                    Rarities.ReplaceItem(Rarities.FirstOrDefault(x => x.IdRarity == updateRarity.IdRarity), updateRarity);

                if (parameter is KillerPerkCategoryDTO updateCategory)
                    PerkCategories.ReplaceItem(PerkCategories.FirstOrDefault(x => x.IdKillerPerkCategory == updateCategory.IdKillerPerkCategory), updateCategory);
            }

            #endregion

            #region Удаление записей

            if (typeParameter == TypeParameter.DeleteAndNotification)
            {
                if (parameter is RarityDTO deleteRarity)
                    Rarities.Remove(Rarities.FirstOrDefault(x => x.IdRarity == deleteRarity.IdRarity));

                if (parameter is KillerPerkCategoryDTO deleteCategory)
                    PerkCategories.Remove(PerkCategories.FirstOrDefault(x => x.IdKillerPerkCategory == deleteCategory.IdKillerPerkCategory));
            }

            #endregion
        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекция

        public ObservableCollection<KillerLoadoutDTO> KillersLoadout { get; set; } = [];

        public ObservableCollection<KillerPerkCategoryDTO> PerkCategories { get; set; } = [];

        public ObservableCollection<RarityDTO> Rarities { get; set; } = [];

        #endregion

        #region Свойство : Title

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

        #region Свойства : Selected

        private KillerLoadoutDTO _selectedKillerLoadout;
        public KillerLoadoutDTO SelectedKillerLoadout
        {
            get => _selectedKillerLoadout;
            set
            {
                _selectedKillerLoadout = value;
                if (value == null)
                    return;

                KillerName = value.KillerName;
                KillerImage = value.KillerImage;

                ClearInputDataAddon();
                ClearInputDataPerk();

                OnPropertyChanged();
            }
        }

        private KillerAddonDTO _selectedKillerAddon;
        public KillerAddonDTO SelectedKillerAddon
        {
            get => _selectedKillerAddon;
            set
            {
                _selectedKillerAddon = value;
                if (value == null) 
                    return;

                AddonName = value.AddonName;
                AddonImage = value.AddonImage;
                AddonDescription = value.AddonDescription;
                SelectedRarity = Rarities.FirstOrDefault(x => x.IdRarity == value.IdRarity);

                OnPropertyChanged();
            }
        }

        private KillerPerkDTO _selectedKillerPerk;
        public KillerPerkDTO SelectedKillerPerk
        {
            get => _selectedKillerPerk;
            set
            {
                _selectedKillerPerk = value;
                if (value == null)
                    return;

                PerkName = value.PerkName;
                PerkImage = value.PerkImage;
                PerkDescription = value.PerkDescription;
                SelectedKillerPerkCategory = PerkCategories.FirstOrDefault(x => x.IdKillerPerkCategory == value.IdCategory);

                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Киллера

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

        private byte[] _killerImage;
        public byte[] KillerImage
        {
            get => _killerImage;
            set
            {
                _killerImage = value;
                OnPropertyChanged();
            }
        }

        private byte[] _killerAbilityImage;
        public byte[] KillerAbilityImage
        {
            get => _killerAbilityImage;
            set
            {
                _killerAbilityImage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Улучшения киллера

        private RarityDTO _selectedRarity;
        public RarityDTO SelectedRarity
        {
            get => _selectedRarity;
            set
            {
                _selectedRarity = value;

                OnPropertyChanged();
            }
        }

        private string _addonName;
        public string AddonName
        {
            get => _addonName;
            set
            {
                _addonName = value;
                OnPropertyChanged();
            }
        }

        private byte[] _addonImage;
        public byte[] AddonImage
        {
            get => _addonImage;
            set
            {
                _addonImage = value;
                OnPropertyChanged();
            }
        }

        private string _addonDescription;
        public string AddonDescription
        {
            get => _addonDescription;
            set
            {
                _addonDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Перк киллера

        private KillerPerkCategoryDTO _selectedKillerPerkCategory;
        public KillerPerkCategoryDTO SelectedKillerPerkCategory
        {
            get => _selectedKillerPerkCategory;
            set
            {
                _selectedKillerPerkCategory = value;

                OnPropertyChanged();
            }
        }

        private string _perkName;
        public string PerkName
        {
            get => _perkName;
            set
            {
                _perkName = value;
                OnPropertyChanged();
            }
        }
        
        private string _perkDescription;
        public string PerkDescription
        {
            get => _perkDescription;
            set
            {
                _perkDescription = value;
                OnPropertyChanged();
            }
        }

        private byte[] _perkImage;
        public byte[] PerkImage
        {
            get => _perkImage;
            set
            {
                _perkImage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _createKillerCommand;
        public RelayCommand CreateKillerCommand { get => _createKillerCommand ??= new(obj => { CreateKiller(); }); }

        private RelayCommand _updateKillerCommand;
        public RelayCommand UpdateKillerCommand { get => _updateKillerCommand ??= new(obj => { UpdateKiller(); }); }

        private RelayCommand _deleteKillerCommand;
        public RelayCommand DeleteKillerCommand { get => _deleteKillerCommand ??= new(obj => { DeleteKiller(); }); }


        private RelayCommand _createKillerAddonCommand;
        public RelayCommand CreateKillerAddonCommand { get => _createKillerAddonCommand ??= new(obj => { CreateAddon(); }); }

        private RelayCommand _updateKillerAddonCommand;
        public RelayCommand UpdateKillerAddonCommand { get => _updateKillerAddonCommand ??= new(obj => { UpdateAddon();}); }

        private RelayCommand _deleteKillerAddonCommand;
        public RelayCommand DeleteKillerAddonCommand { get => _deleteKillerAddonCommand ??= new(obj => { DeleteAddon(); }); }


        private RelayCommand _createKillerPerkCommand;
        public RelayCommand CreateKillerPerkCommand { get => _createKillerPerkCommand ??= new(obj => { CreatePerk(); }); }

        private RelayCommand _updateKillerPerkCommand;
        public RelayCommand UpdateKillerPerkCommand { get => _updateKillerPerkCommand ??= new(obj => { UpdatePerk(); }); }

        private RelayCommand _deleteKillerPerkCommand;
        public RelayCommand DeleteKillerPerkCommand { get => _deleteKillerPerkCommand ??= new(obj => { DeletePerk(); }); }

        #endregion

        #region Добавление зависимых данных

        private RelayCommand _addRarityCommand;
        public RelayCommand AddRarityCommand { get => _addRarityCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionRarity, parameter: null, TypeParameter.None, IsOpenDialog: true); }); }

        private RelayCommand _addKillerPerkCategoryCommand;
        public RelayCommand AddKillerPerkCategoryCommand { get => _addKillerPerkCategoryCommand ??= new(obj => { _windowNavigationService.OpenWindow(WindowName.InteractionKillerPerkCategory, parameter: null, TypeParameter.None, IsOpenDialog: true); }); }

        #endregion

        #region Выбор изображения | Очистка выбранного изображениея

        private RelayCommand _selectKillerImageCommand;
        public RelayCommand SelectKillerImageCommand { get => _selectKillerImageCommand ??= new(obj => { SelectKillerImage(); }); }

        private RelayCommand _clearKillerImageCommand;
        public RelayCommand ClearKillerImageCommand { get => _clearKillerImageCommand ??= new(obj => { KillerImage = null; }); }


        private RelayCommand _selectKillerAddonImageCommand;
        public RelayCommand SelectKillerAddonImageCommand { get => _selectKillerAddonImageCommand ??= new(obj => { SelectKillerAddonImage(); }); }

        private RelayCommand _clearKillerAddonImageCommand;
        public RelayCommand ClearKillerAddonImageCommand { get => _clearKillerAddonImageCommand ??= new(obj => { AddonImage = null; }); }


        private RelayCommand _selectKillerPerkImageCommand;
        public RelayCommand SelectKillerPerkImageCommand { get => _selectKillerPerkImageCommand ??= new(obj => { SelectPerkImage(); }); }

        private RelayCommand _clearKillerPerkIImageCommand;
        public RelayCommand ClearKillerPerkImageCommand { get => _clearKillerPerkIImageCommand ??= new(obj => { PerkImage = null; }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Получение данных

        private async void GetKillersLoadout()
        {
            var killersLoadout = await _killerService.GetKillersWithAddonsAndPerksAsync();

            foreach (var item in killersLoadout)
                KillersLoadout.Add(item);
        }

        private async void GetRarities()
        {
            var rarities = await _rarityService.GetAllAsync();

            foreach (var item in rarities)
                Rarities.Add(item);
        }

        private async void GetPerkCategories()
        {
            var perkCategories = await _killerPerkCategoryService.GetAllAsync();

            foreach (var item in perkCategories)
                PerkCategories.Add(item);
        }

        #endregion

        //TODO : Заменить MessageBox на кастомное окно
        #region CRUD : Киллер

        private async void CreateKiller()
        {
            var (Killer, Message) = await _killerService.CreateAsync(KillerName, KillerImage, KillerAbilityImage);

            if (!string.IsNullOrWhiteSpace(Message))
            {
                MessageBox.Show(Message);
                return;
            }
            else
            {
                KillersLoadout.Add(new KillerLoadoutDTO
                {
                    IdKiller = Killer.IdKiller,
                    KillerName = Killer.KillerName,
                    KillerImage = Killer.KillerImage,
                    KillerAbilityImage = Killer.KillerAbilityImage,
                    KillerAddons = new ObservableCollection<KillerAddonDTO>(),
                    KillerPerks = new ObservableCollection<KillerPerkDTO>()
                });

                NotificationTransmittingValue(WindowName.AddMatch, Killer, TypeParameter.AddAndNotification);
                ClearInputDataKiller();
            }
        }

        private async void UpdateKiller()
        {
            if (SelectedKillerLoadout == null)
                return;

            var (Killer, Message) = await _killerService.UpdateAsync(SelectedKillerLoadout.IdKiller, KillerName, KillerImage, KillerAbilityImage);

            if (Message == string.Empty)
            {
                KillersLoadout.ReplaceItem(SelectedKillerLoadout,
                                           new KillerLoadoutDTO
                                           {
                                               IdKiller = Killer.IdKiller,
                                               KillerName = Killer.KillerName,
                                               KillerImage = Killer.KillerImage,
                                               KillerAbilityImage = Killer.KillerAbilityImage,
                                               KillerAddons = SelectedKillerLoadout.KillerAddons,
                                               KillerPerks = SelectedKillerLoadout.KillerPerks
                                           });

                NotificationTransmittingValue(WindowName.AddMatch, Killer, TypeParameter.UpdateAndNotification);
                ClearInputDataKiller();
            }
            else
            {
                if (MessageBox.Show(Message + "Вы точно хотите обновить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var forcedKillersLoadoutDTO = await _killerService.ForcedUpdateAsync(SelectedKillerLoadout.IdKiller, KillerName, KillerImage, KillerAbilityImage);
                    KillersLoadout.ReplaceItem(SelectedKillerLoadout,
                                               new KillerLoadoutDTO
                                               {
                                                   IdKiller = forcedKillersLoadoutDTO.IdKiller,
                                                   KillerName = forcedKillersLoadoutDTO.KillerName,
                                                   KillerImage = forcedKillersLoadoutDTO.KillerImage,
                                                   KillerAbilityImage = forcedKillersLoadoutDTO.KillerAbilityImage,
                                                   KillerAddons = SelectedKillerLoadout.KillerAddons,
                                                   KillerPerks = SelectedKillerLoadout.KillerPerks
                                               });

                    NotificationTransmittingValue(WindowName.AddMatch, forcedKillersLoadoutDTO, TypeParameter.UpdateAndNotification);
                    ClearInputDataKiller();
                }
            }
        }

        private async void DeleteKiller()
        {
            if (SelectedKillerLoadout == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _killerService.DeleteAsync(SelectedKillerLoadout.IdKiller);

                if (!IsDeleted)
                {
                    MessageBox.Show(Message);
                    return;
                }
                else
                {
                    NotificationTransmittingValue(WindowName.AddMatch, new KillerDTO { IdKiller = SelectedKillerLoadout.IdKiller }, TypeParameter.DeleteAndNotification);
                    KillersLoadout.Remove(SelectedKillerLoadout);
                    ClearInputDataKiller();
                }
            }
        }

        #endregion

        #region CRUD : Улучшение

        private async void CreateAddon()
        {
            if (SelectedKillerLoadout == null)
                return;

            if (SelectedRarity == null)
            {
                MessageBox.Show("Выберите качество");
                return;
            }

            var (Addon, Message) = await _killerAddonService.CreateAsync(SelectedKillerLoadout.IdKiller, SelectedRarity.IdRarity, AddonName, AddonImage, AddonDescription);

            if (!string.IsNullOrWhiteSpace(Message))
            {
                MessageBox.Show(Message);
                return;
            }
            else
            {
                SelectedKillerLoadout.KillerAddons.Add(Addon);
                NotificationTransmittingValue(WindowName.AddMatch, Addon, TypeParameter.AddAndNotification);
                ClearInputDataAddon();
            }
        }

        private async void UpdateAddon()
        {
            if (SelectedKillerLoadout == null || SelectedKillerAddon == null)
                return;

            var (Addon, Message) = await _killerAddonService.UpdateAsync(SelectedKillerAddon.IdKillerAddon,
                                                                         SelectedKillerLoadout.IdKiller, 
                                                                         SelectedRarity.IdRarity,
                                                                         AddonName,
                                                                         AddonImage,
                                                                         AddonDescription);

            if (Message == string.Empty)
            {
                SelectedKillerLoadout.KillerAddons.ReplaceItem(SelectedKillerAddon, Addon);

                NotificationTransmittingValue(WindowName.AddMatch, Addon, TypeParameter.UpdateAndNotification);
                ClearInputDataAddon();
            }
        }

        private async void DeleteAddon()
        {
            if (SelectedKillerLoadout == null || SelectedKillerAddon == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _killerAddonService.DeleteAsync(SelectedKillerAddon.IdKillerAddon);

                if (!IsDeleted)
                {
                    MessageBox.Show(Message);
                    return;
                }
                else
                {
                    NotificationTransmittingValue(WindowName.AddMatch, SelectedKillerAddon, TypeParameter.DeleteAndNotification);
                    SelectedKillerLoadout.KillerAddons.Remove(SelectedKillerAddon);
                    ClearInputDataAddon();
                }
            }
        }

        #endregion

        #region CRUD : Перки

        private async void CreatePerk()
        {
            if (SelectedKillerLoadout == null)
                return;

            if (SelectedKillerPerkCategory == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }
                
            var (Perk, Message) = await _killerPerkService.CreateAsync(SelectedKillerLoadout.IdKiller, PerkName, PerkImage, SelectedKillerPerkCategory.IdKillerPerkCategory, PerkDescription);

            if (!string.IsNullOrWhiteSpace(Message))
            {
                MessageBox.Show(Message);
                return;
            }
            else
            {
                SelectedKillerLoadout.KillerPerks.Add(Perk);
                NotificationTransmittingValue(WindowName.AddMatch, Perk, TypeParameter.AddAndNotification);
                ClearInputDataPerk();
            }
        }

        private async void UpdatePerk()
        {
            if (SelectedKillerLoadout == null || SelectedKillerPerk == null)
                return;

            var (Perk, Message) = await _killerPerkService.UpdateAsync(SelectedKillerPerk.IdKillerPerk, SelectedKillerLoadout.IdKiller, PerkName, PerkImage, SelectedKillerPerkCategory.IdKillerPerkCategory, PerkDescription);

            if (Message == string.Empty)
            {
                SelectedKillerLoadout.KillerPerks.ReplaceItem(SelectedKillerPerk, Perk);
                NotificationTransmittingValue(WindowName.AddMatch, Perk, TypeParameter.UpdateAndNotification);
                ClearInputDataPerk();
            }
        }

        private async void DeletePerk()
        {
            if (SelectedKillerLoadout == null || SelectedKillerPerk == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _killerPerkService.DeleteAsync(SelectedKillerPerk.IdKillerPerk);

                if (!IsDeleted)
                {
                    MessageBox.Show(Message);
                    return;
                }
                else
                {
                    NotificationTransmittingValue(WindowName.AddMatch, SelectedKillerPerk, TypeParameter.DeleteAndNotification);
                    SelectedKillerLoadout.KillerPerks.Remove(SelectedKillerPerk);
                    ClearInputDataPerk();
                }
            }
        }

        #endregion

        //TODO : Заменить прямой вызов OpenFileDialog на вызов из сервиса
        #region Выбор изображения

        private void SelectKillerImage()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                KillerImage = ImageHelper.ImageToByteArray(openFileDialog.FileName);
            }
        }

        private void SelectKillerAddonImage()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                AddonImage = ImageHelper.ImageToByteArray(openFileDialog.FileName);
            }
        }

        private void SelectPerkImage()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                PerkImage = ImageHelper.ImageToByteArray(openFileDialog.FileName);
            }
        }

        #endregion

        #region Очистка полей

        private void ClearInputDataKiller()
        {
            KillerName = string.Empty;
            KillerImage = null;
            KillerAbilityImage = null;
        }

        private void ClearInputDataAddon()
        {
            SelectedRarity = null;
            AddonName = string.Empty;
            AddonDescription = string.Empty;
            AddonImage = null;
        }

        private void ClearInputDataPerk()
        {
            SelectedKillerPerkCategory = null;
            PerkName = string.Empty;
            PerkDescription = string.Empty;
            PerkImage = null;
        }

        #endregion

    }
}