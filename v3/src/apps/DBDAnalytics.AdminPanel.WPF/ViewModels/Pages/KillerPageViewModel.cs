using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.Killer;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerAddon;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerk;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerkCategory;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Rarity;
using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.FIleStorageService.Client;
using DBDAnalytics.Shared.Contracts.Constants;
using DBDAnalytics.Shared.Contracts.Responses;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.WPF.Commands;
using Shared.WPF.Helpers;
using Shared.WPF.Services;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Pages
{
    internal sealed class KillerPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IKillerService _killerService;
        private readonly IKillerAddonApiServiceFactory _killerAddonApiServiceFactory;
        private readonly IKillerPerkApiServiceFactory _killerPerkApiServiceFactory;
        private readonly IRarityService _rarityService;
        private readonly IKillerPerkCategoryService _killerPerkCategoryService;
        private readonly IFileDialogService _fileDialogService;
        private readonly IFileStorageService _fileStorageService;

        public KillerPageViewModel(
            IKillerService killerService,
            IKillerAddonApiServiceFactory killerAddonApiServiceFactory,
            IKillerPerkApiServiceFactory killerPerkApiServiceFactory,
            IRarityService rarityService,
            IKillerPerkCategoryService killerPerkCategoryService,
            IFileDialogService fileDialogService,
            IFileStorageService fileStorageService)
        {
            _killerService = killerService;
            _killerAddonApiServiceFactory = killerAddonApiServiceFactory;
            _killerPerkApiServiceFactory = killerPerkApiServiceFactory;
            _rarityService = rarityService;
            _killerPerkCategoryService = killerPerkCategoryService;
            _fileDialogService = fileDialogService;
            _fileStorageService = fileStorageService;
            
            PrepareKillerNew();
            PrepareAddonNew();
            PreparePerkNew();

            InitializeCommand();
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsBusy = true;

            try
            {
                await GetAllRarities();
                await GetAllPerkCategories();

                await GetAllKillers();

                IsInitialize = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при инициализации страницы: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /*--Коллекции-------------------------------------------------------------------------------------*/

        public ObservableCollection<KillerViewModel> Killers { get; private set; } = [];
        public ObservableCollection<KillerAddonViewModel> Addons { get; private set; } = [];
        public ObservableCollection<KillerPerkViewModel> Perks { get; private set; } = [];

        public ObservableCollection<KillerViewModel> LocalKillers { get; private set; } = [];
        public ObservableCollection<KillerAddonViewModel> LocalAddons { get; private set; } = [];
        public ObservableCollection<KillerPerkViewModel> LocalPerks { get; private set; } = [];

        public ObservableCollection<RarityResponse> Rarities { get; private set; } = [];
        public ObservableCollection<KillerPerkCategoryResponse> PerkCategories { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController MenuKillerPopup { get; } = new PopupController();
        public IPopupController MenuAddonPopup { get; } = new PopupController();
        public IPopupController MenuPerkPopup { get; } = new PopupController();

        public IPopupController MenuLocalKillerPopup { get; } = new PopupController();
        public IPopupController MenuLocalAddonPopup { get; } = new PopupController();
        public IPopupController MenuLocalPerkPopup { get; } = new PopupController();

        /*--Сущности--*/

        // Киллер
        #region Свойство: [SelectedKiller] Метод: [OnSelectedKillerPropertyChanged]

        private KillerViewModel? _selectedKiller;
        public KillerViewModel? SelectedKiller
        {
            get => _selectedKiller;
            set
            {
                if (_selectedKiller is not null)
                    _selectedKiller.PropertyChanged -= OnSelectedKillerPropertyChanged;

                SetProperty(ref _selectedKiller, value);

                if (IsProcessAddedKiller == false)
                {
                    _ = GetAddonsForKiller();
                    _ = GetPerksForKiller();
                }

                if (_selectedKiller is not null)
                    _selectedKiller.PropertyChanged += OnSelectedKillerPropertyChanged;
            }
        }

        private void OnSelectedKillerPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ManagementKillerEditCommand?.RaiseCanExecuteChanged();
            RevertKillerChangeCommand?.RaiseCanExecuteChanged();
            UpdateKillerCommandAsync?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [SelectedLocalKiller] Метод: [OnSelectedKillerPropertyChanged]

        private KillerViewModel? _selectedLocalKiller;
        public KillerViewModel? SelectedLocalKiller
        {
            get => _selectedLocalKiller;
            set
            {
                if (_selectedLocalKiller is not null)
                    _selectedLocalKiller.PropertyChanged -= OnSelectedLocalKillerPropertyChanged;

                SetProperty(ref _selectedLocalKiller, value);

                if (_selectedLocalKiller is not null)
                    _selectedLocalKiller.PropertyChanged += OnSelectedLocalKillerPropertyChanged;
            }
        }

        private void OnSelectedLocalKillerPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            CreateKillerCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewKiller] Метод: [OnNewKillerPropertyChanged]

        private KillerViewModel? _newKiller;
        public KillerViewModel? NewKiller
        {
            get => _newKiller;
            set
            {
                if (_newKiller is not null)
                    _newKiller.PropertyChanged -= OnNewKillerPropertyChanged;

                SetProperty(ref _newKiller, value);

                if (_newKiller is not null)
                    _newKiller.PropertyChanged += OnNewKillerPropertyChanged;
            }
        }

        private void OnNewKillerPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        // Улучшения
        #region Свойство: [SelectedAddon] Метод: [OnSelectedAddonPropertyChanged]

        private KillerAddonViewModel? _selectedAddon;
        public KillerAddonViewModel? SelectedAddon
        {
            get => _selectedAddon;
            set
            {
                if (_selectedAddon is not null)
                    _selectedAddon.PropertyChanged -= OnSelectedAddonPropertyChanged;

                SetProperty(ref _selectedAddon, value);

                if (_selectedAddon is not null)
                    _selectedAddon.PropertyChanged += OnSelectedAddonPropertyChanged;
            }
        }

        private void OnSelectedAddonPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RevertAddonChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [SelectedLocalAddon] Метод: [OnSelectedLocalAddonPropertyChanged]

        private KillerAddonViewModel? _selectedLocalAddon;
        public KillerAddonViewModel? SelectedLocalAddon
        {
            get => _selectedLocalAddon;
            set
            {
                if (_selectedLocalAddon is not null)
                    _selectedLocalAddon.PropertyChanged -= OnSelectedLocalAddonPropertyChanged;

                SetProperty(ref _selectedLocalAddon, value);

                if (_selectedLocalAddon is not null)
                    _selectedLocalAddon.PropertyChanged += OnSelectedLocalAddonPropertyChanged;
            }
        }

        private void OnSelectedLocalAddonPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Свойство: [MewAddon] Метод: [OnMewAddonPropertyChanged]

        private KillerAddonViewModel? _newAddon;
        public KillerAddonViewModel? NewAddon
        {
            get => _newAddon;
            set
            {
                if (_newAddon is not null)
                    _newAddon.PropertyChanged -= OnMewAddonPropertyChanged;

                SetProperty(ref _newAddon, value);

                if (_newAddon is not null)
                    _newAddon.PropertyChanged += OnMewAddonPropertyChanged;
            }
        }

        private void OnMewAddonPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            AddLocalAddonCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        // Перк
        #region Свойство: [SlectedPerk] Метод: [OnSlectedPerkPropertyChanged]

        private KillerPerkViewModel? _selectedPerk;
        public KillerPerkViewModel? SelectedPerk
        {
            get => _selectedPerk;
            set
            {
                if (_selectedPerk is not null)
                    _selectedPerk.PropertyChanged -= OnSelectedPerkPropertyChanged;

                SetProperty(ref _selectedPerk, value);

                if (_selectedPerk is not null)
                    _selectedPerk.PropertyChanged += OnSelectedPerkPropertyChanged;
            }
        }

        private void OnSelectedPerkPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ManagementPerkEditCommand?.RaiseCanExecuteChanged();
            RevertPerkChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [SlectedLocalPerk] Метод: [OnSlectedLocalPerkPropertyChanged]

        private KillerPerkViewModel? _selectedLocalPerk;
        public KillerPerkViewModel? SelectedLocalPerk
        {
            get => _selectedLocalPerk;
            set
            {
                if (_selectedLocalPerk is not null)
                    _selectedLocalPerk.PropertyChanged -= OnSelectedLocalPerkPropertyChanged;

                SetProperty(ref _selectedLocalPerk, value);

                if (_selectedLocalPerk is not null)
                    _selectedLocalPerk.PropertyChanged += OnSelectedLocalPerkPropertyChanged;
            }
        }

        private void OnSelectedLocalPerkPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            
        }

        #endregion

        #region Свойство: [NewPerk] Метод: [OnNewPerkPropertyChanged]

        private KillerPerkViewModel? _newPerk;
        public KillerPerkViewModel? NewPerk
        {
            get => _newPerk;
            set
            {
                if (_newPerk is not null)
                    _newPerk.PropertyChanged -= OnNewPerkPropertyChanged;

                SetProperty(ref _newPerk, value);

                if (_newPerk is not null)
                    _newPerk.PropertyChanged += OnNewPerkPropertyChanged;
            }
        }

        private void OnNewPerkPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        /*--Флаги--*/

        #region Свойство: [IsProcessAddedKiller] Добавляеться ли в данный момент убийца или нет

        private bool IsProcessAddedKiller = false;

        #endregion

        /*--TabControl--*/

        #region Свойства: [TabControlIndex]: Индексы для TabControl'ов

        private int _tabControlKillerIndex;
        public int TabControlKillerIndex
        {
            get => _tabControlKillerIndex;
            set
            {
                if (SetProperty(ref _tabControlKillerIndex, value))
                {
                    TabControlAddonIndex = value;
                    TabControlPerkIndex = value;
                }
            }
        }

        private int _tabControlAddonIndex;
        public int TabControlAddonIndex
        {
            get => _tabControlAddonIndex;
            set => SetProperty(ref _tabControlAddonIndex, value);
        }

        private int _tabControlPerkIndex;
        public int TabControlPerkIndex
        {
            get => _tabControlPerkIndex;
            set => SetProperty(ref _tabControlPerkIndex, value);
        }

        #endregion

        /*--Visibility Modal--*/

        #region Свойства: [ModalVisibility]: Отображение модальных окон

        private Visibility _killerAddModalVisibility = Visibility.Collapsed;
        public Visibility KillerAddModalVisibility
        {
            get => _killerAddModalVisibility;
            set => SetProperty(ref _killerAddModalVisibility, value);
        }

        private Visibility _killerEditModalVisibility = Visibility.Collapsed;
        public Visibility KillerEditModalVisibility
        {
            get => _killerEditModalVisibility;
            set => SetProperty(ref _killerEditModalVisibility, value);
        }

        private Visibility _localKillerEditModalVisibility = Visibility.Collapsed;
        public Visibility LocalKillerEditModalVisibility
        {
            get => _localKillerEditModalVisibility;
            set => SetProperty(ref _localKillerEditModalVisibility, value);
        }

        private Visibility _addonEditModalVisibility = Visibility.Collapsed;
        public Visibility AddonEditModalVisibility
        {
            get => _addonEditModalVisibility;
            set => SetProperty(ref _addonEditModalVisibility, value);
        }

        private Visibility _localAddonEditModalVisibility = Visibility.Collapsed;
        public Visibility LocalAddonEditModalVisibility
        {
            get => _localAddonEditModalVisibility;
            set => SetProperty(ref _localAddonEditModalVisibility, value);
        }

        private Visibility _addonAddModalVisibility = Visibility.Collapsed;
        public Visibility AddonAddModalVisibility
        {
            get => _addonAddModalVisibility;
            set => SetProperty(ref _addonAddModalVisibility, value);
        }

        private Visibility _perkAddModalVisibility = Visibility.Collapsed;
        public Visibility PerkAddModalVisibility
        {
            get => _perkAddModalVisibility;
            set => SetProperty(ref _perkAddModalVisibility, value);
        }

        private Visibility _perkEditModalVisibility = Visibility.Collapsed;
        public Visibility PerkEditModalVisibility
        {
            get => _perkEditModalVisibility;
            set => SetProperty(ref _perkEditModalVisibility, value);
        }

        private Visibility _localPerkEditModalVisibility = Visibility.Collapsed;
        public Visibility LocalPerkEditModalVisibility
        {
            get => _localPerkEditModalVisibility;
            set => SetProperty(ref _localPerkEditModalVisibility, value);
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            // Киллер
            AddLocalKillerCommand = new RelayCommand(Execute_AddLocalKillerCommand, CanExecute_AddLocalKillerCommand);
            DeleteLocalKillerCommand = new RelayCommand<KillerViewModel>(Execute_DeleteLocalKillerCommand, CanExecute_DeleteLocalKillerCommand);

            SelectKillerImageCommand = new RelayCommand<KillerViewModel>(Execute_SelectKillerImageCommand, CanExecute_SelectKillerImageCommand);
            SelectAbilityImageCommand = new RelayCommand<KillerViewModel>(Execute_SelectAbilityImageCommand, CanExecute_SelectAbilityImageCommand);

            CreateKillerCommand = new RelayCommandAsync(Execute_CreateKillerCommand, CanExecute_CreateKillerCommand);
            UpdateKillerCommandAsync = new RelayCommandAsync<KillerViewModel>(Execute_UpdateKillerCommandAsync, CanExecute_UpdateKillerCommandAsync);
            RevertKillerChangeCommand = new RelayCommand<KillerViewModel>(Execute_RevertKillerChangeCommand, CanExecute_RevertKillerChangeCommand);
            DeleteKillerCommandAsync = new RelayCommandAsync<KillerViewModel>(Execute_DeleteKillerCommandAsync, CanExecute_DeleteKillerCommandAsync);

            ManagementLocalKillerEditCommand = new RelayCommand<Visibility>(Execute_ManagementLocalKillerEditCommand, CanExecute_ManagementLocalKillerEditCommand);
            ManagementKillerEditCommand = new RelayCommand<Visibility>(Execute_ManagementKillerEditCommand, CanExecute_ManagementKillerEditCommand);
            ManagementKillerAddCommand = new RelayCommand<Visibility>(Execute_ManagementKillerAddCommand, CanExecute_ManagementKillerAddCommand);

            // Улучшение
            AddLocalAddonCommand = new RelayCommand(Execute_AddLocalAddonCommand, CanExecute_AddLocalAddonCommand);
            DeleteLocalAddonCommand = new RelayCommand<KillerAddonViewModel>(Execute_DeleteLocalAddonCommand, CanExecute_DeleteLocalAddonCommand);

            SelectAddonImageCommand = new RelayCommand<KillerAddonViewModel>(Execute_SelectAddonImageCommand, CanExecute_SelectAddonImageCommand);

            UpdateAddonCommandAsync = new RelayCommandAsync<KillerAddonViewModel>(Execute_UpdateAddonCommandAsync, CanExecute_UpdateAddonCommandAsync);
            RevertAddonChangeCommand = new RelayCommand<KillerAddonViewModel>(Execute_RevertAddonChangeCommand, CanExecute_RevertAddonChangeCommand);
            DeleteAddonCommandAsync = new RelayCommandAsync<KillerAddonViewModel>(Execute_DeleteAddonCommandAsync, CanExecute_DeleteAddonCommandAsync);

            ManagementLocalAddonEditCommand = new RelayCommand<Visibility>(Execute_ManagementLocalAddonEditCommand, CanExecute_ManagementLocalAddonEditCommand);
            ManagementAddonAddCommand = new RelayCommand<Visibility>(Execute_ManagementAddonAddCommand, CanExecute_ManagementAddonAddCommand);
            ManagementAddonEditCommand = new RelayCommand<Visibility>(Execute_ManagementAddonEditCommand, CanExecute_ManagementAddonEditCommand);

            // Перк
            AddLocalPerkCommand = new RelayCommand(Execute_AddLocalPerkCommand, CanExecute_AddLocalPerkCommand);
            DeleteLocalPerkCommand = new RelayCommand<KillerPerkViewModel>(Execute_DeleteLocalPerkCommand, CanExecute_DeleteLocalPerkCommand);
            
            SelectPerkImageCommand = new RelayCommand<KillerPerkViewModel>(Execute_SelectPerkImageCommand, CanExecute_SelectPerkImageCommand);

            UpdatePerkCommandAsync = new RelayCommandAsync<KillerPerkViewModel>(Execute_UpdatePerkCommandAsync, CanExecute_UpdatePerkCommandAsync);
            RevertPerkChangeCommand = new RelayCommand<KillerPerkViewModel>(Execute_RevertPerkChangeCommand, CanExecute_RevertPerkChangeCommand);
            DeletePerkCommandAsync = new RelayCommandAsync<KillerPerkViewModel>(Execute_DeletePerkCommandAsync, CanExecute_DeletePerkCommandAsync);

            ManagementPerkAddCommand = new RelayCommand<Visibility>(Execute_ManagementPerkAddCommand, CanExecute_ManagementPerkAddCommand);
            ManagementPerkEditCommand = new RelayCommand<Visibility>(Execute_ManagementPerkEditCommand, CanExecute_ManagementPerkEditCommand);
            ManagementLocalPerkEditCommand = new RelayCommand<Visibility>(Execute_ManagementLocalPerkEditCommand, CanExecute_ManagementLocalPerkEditCommand);
        }

        /*--Киллеры--*/

        #region Команда [AddLocalKillerCommand]: Добавляет киллера локально

        public RelayCommand? AddLocalKillerCommand { get; private set; }

        private void Execute_AddLocalKillerCommand()
        {
            if (NewKiller is null)
                return;

            NewKiller.SetLocalID(Guid.NewGuid());
            LocalKillers.Add(NewKiller);
            ManagementKillerAddCommand?.Execute(Visibility.Collapsed);
            TabControlKillerIndex = 1;

            PrepareKillerNew();
        }

        private bool CanExecute_AddLocalKillerCommand() => true;

        #endregion

        #region Команда [DeleteLocalKillerCommand]: Удаляет локального киллера

        public RelayCommand<KillerViewModel>? DeleteLocalKillerCommand { get; private set; }

        private void Execute_DeleteLocalKillerCommand(KillerViewModel model) => LocalKillers.Remove(model);

        private bool CanExecute_DeleteLocalKillerCommand(KillerViewModel model) => true;

        #endregion


        #region Команда [SelectKillerImageCommand]: Выбор изображение киллера

        public RelayCommand<KillerViewModel>? SelectKillerImageCommand { get; private set; }

        private void Execute_SelectKillerImageCommand(KillerViewModel model)
        {
            var path = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");
            model?.SetKillerImage(ImageHelper.ImageFromFilePath(path!), path);
        }

        private bool CanExecute_SelectKillerImageCommand(KillerViewModel model) => true;

        #endregion

        #region Команда [SelectAbilityImageCommand]: Выбор изображение киллера

        public RelayCommand<KillerViewModel>? SelectAbilityImageCommand { get; private set; }

        private void Execute_SelectAbilityImageCommand(KillerViewModel model)
        {
            var path = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");
            model?.SetAbilityImage(ImageHelper.ImageFromFilePath(path!), path);
        }

        private bool CanExecute_SelectAbilityImageCommand(KillerViewModel model) => true;

        #endregion


        #region Команда [CreateKillerCommand]: Добавлят полную запись об убийце

        public RelayCommandAsync? CreateKillerCommand { get; private set; }

        private async Task Execute_CreateKillerCommand()
        {
            IsBusy = true;
            IsProcessAddedKiller = true;

            try
            {
                if (SelectedLocalKiller is null)
                {
                    MessageBox.Show("Вы не выбрали киллера для добавления");
                    return;
                }

                if (SelectedLocalKiller.KillerImage is null || SelectedLocalKiller.AbilityImage is null)
                {
                    MessageBox.Show("Вы не выбрали изображения для киллера");
                    return;
                }

                var result = await _killerService.AddAsync(
                    new ClientAddKillerRequest(
                        SelectedLocalKiller.OldId, SelectedLocalKiller.Name, SelectedLocalKiller.KillerImagePath, SelectedLocalKiller.AbilityImagePath,
                        [.. LocalAddons.Where(a => a.KillerLocalId == SelectedLocalKiller.LocalId).Select(a => new ClientKillerAddonsData(a.OldId, a.Name, a.ImagePath))],
                        [.. LocalPerks.Where(p => p.KillerLocalId == SelectedLocalKiller.LocalId).Select(p => new ClientKillerPerksData(p.OldId, p.Name, p.ImagePath))]));

                if (!result.IsSuccess)
                {
                    MessageBox.Show($"Не удалось добавить запись: {result.StringMessage}");
                    return;
                }

                var killerUrl = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.KillerPortraits}/{result.Value.KillerImageKey}");
                var abilityUrl = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.KillerAbilities}/{result.Value.AbilityImageKey}");

                var addedKiller = new KillerViewModel(new KillerSoloResponse(result.Value.Id, result.Value.OldId, result.Value.Name, result.Value.KillerImageKey, result.Value.AbilityImageKey));

                addedKiller.SetKillerImage(await ImageHelper.ImageFromUrlAsync(killerUrl), string.Empty);
                addedKiller.SetAbilityImage(await ImageHelper.ImageFromUrlAsync(abilityUrl), string.Empty);

                Killers.Add(addedKiller);

                Addons.Clear();
                Perks.Clear();

                foreach (var addon in result.Value.KillerAddons)
                {
                    if (addon is null)
                        continue;

                    var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.KillerAddons(result.Value.Name)}/{addon.ImageKey}");
                    var addonVM = new KillerAddonViewModel(addon);
                    addonVM.SetImage(await ImageHelper.ImageFromUrlAsync(url), string.Empty);

                    Addons.Add(addonVM);
                }

                foreach (var perk in result.Value.KillerPerks)
                {
                    if (perk is null)
                        continue;

                    var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.KillerPerks(result.Value.Name)}/{perk.ImageKey}");
                    var perkVM = new KillerPerkViewModel(perk);
                    perkVM.SetImage(await ImageHelper.ImageFromUrlAsync(url), string.Empty);

                    Perks.Add(perkVM);
                }

                SelectedKiller = addedKiller;
                TabControlKillerIndex = 0;

                LocalKillers.Remove(SelectedLocalKiller); ;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании записи: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                IsProcessAddedKiller = false;
            }
        }

        private bool CanExecute_CreateKillerCommand() => true;

        #endregion 

        #region Команда [UpdateKillerCommandAsync]: Обновление киллера

        public RelayCommandAsync<KillerViewModel>? UpdateKillerCommandAsync { get; private set; }

        private async Task Execute_UpdateKillerCommandAsync(KillerViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _killerService.UpdateAsync(new ClientUpdateKillerRequest(model.Id, model.Name, model.AbilityImagePath, model.Name, model.KillerImagePath, model.Name));

                if (!result.IsSuccess)
                {
                    MessageBox.Show($"Ошибка обновления: {result.StringMessage}");
                    return;
                }

                model.SetKillerImageKey(result.Value.KillerImageKey);
                model.SetAbilityImageKey(result.Value.AbilityImageKey);

                model.CommitChanges(model.ToModel());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непредвиденная ошибка: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanExecute_UpdateKillerCommandAsync(KillerViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion 

        #region Команда [RevertKillerChangeCommand]: Откат изменений у киллера

        public RelayCommand<KillerViewModel>? RevertKillerChangeCommand { get; private set; }

        private void Execute_RevertKillerChangeCommand(KillerViewModel model)
        {
            model?.RevertChanges();
            MenuKillerPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_RevertKillerChangeCommand(KillerViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion 

        #region Команда [DeletetKillerCommandAsync]: Удаление записи об киллера и его сущьностях

        public RelayCommandAsync<KillerViewModel>? DeleteKillerCommandAsync { get; private set; }

        private async Task Execute_DeleteKillerCommandAsync(KillerViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _killerService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    Killers.Remove(model);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Непредвиденная ошибка: {ex.Message}");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private bool CanExecute_DeleteKillerCommandAsync(KillerViewModel model) => true;

        #endregion


        #region Команда [ManagementLocalKillerEditCommand]: Управление модальным окном для редактирования киллера

        public RelayCommand<Visibility>? ManagementLocalKillerEditCommand { get; private set; }

        private void Execute_ManagementLocalKillerEditCommand(Visibility visibility)
        {
            LocalKillerEditModalVisibility = visibility;
            MenuLocalKillerPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_ManagementLocalKillerEditCommand(Visibility visibility) => true;

        #endregion

        #region Команда [ManagementKillerEditCommand]: Управление модальным окном для редактирования киллера

        public RelayCommand<Visibility>? ManagementKillerEditCommand { get; private set; }

        private void Execute_ManagementKillerEditCommand(Visibility visibility)
        {
            KillerEditModalVisibility = visibility;
            MenuKillerPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_ManagementKillerEditCommand(Visibility visibility) => true;

        #endregion

        #region Команда [ManagementKillerAddCommand]: Управление модальным окном для добавления киллера

        public RelayCommand<Visibility>? ManagementKillerAddCommand { get; private set; }

        private void Execute_ManagementKillerAddCommand(Visibility visibility) => KillerAddModalVisibility = visibility;

        private bool CanExecute_ManagementKillerAddCommand(Visibility visibility) => true;

        #endregion

        /*--Улучшения--*/

        #region Команда [AddLocalAddonCommand]: Добавляет улучшение локально

        public RelayCommand? AddLocalAddonCommand { get; private set; }

        private void Execute_AddLocalAddonCommand()
        {
            if (NewAddon is null)
                return;

            if (SelectedLocalKiller is null)
            {
                MessageBox.Show("Не был выбран киллер к которому нужно создать улучшение.");
                return;
            }

            NewAddon.SetKillerLocalID(SelectedLocalKiller?.LocalId);
            LocalAddons.Add(NewAddon);
            ManagementAddonAddCommand?.Execute(Visibility.Collapsed);
            PrepareAddonNew();
        }

        private bool CanExecute_AddLocalAddonCommand() => !string.IsNullOrWhiteSpace(NewAddon?.Name) && NewAddon?.Image != null;

        #endregion

        #region Команда [DeleteLocalAddonCommand]: Удаляет локальный аддон

        public RelayCommand<KillerAddonViewModel>? DeleteLocalAddonCommand { get; private set; }

        private void Execute_DeleteLocalAddonCommand(KillerAddonViewModel model)
        {
            LocalAddons.Remove(model);
            MenuLocalAddonPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_DeleteLocalAddonCommand(KillerAddonViewModel model) => true;

        #endregion


        #region Команда [SelectAddonImageCommand]: Выбор изображение аддона

        public RelayCommand<KillerAddonViewModel>? SelectAddonImageCommand { get; private set; }

        private void Execute_SelectAddonImageCommand(KillerAddonViewModel model)
        {
            var path = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");
            model?.SetImage(ImageHelper.ImageFromFilePath(path!), path);
        }

        private bool CanExecute_SelectAddonImageCommand(KillerAddonViewModel model) => true;

        #endregion


        #region Команда [UpdateAddonCommandAsync]: Обновление записи Addon

        public RelayCommandAsync<KillerAddonViewModel>? UpdateAddonCommandAsync { get; private set; }

        private async Task Execute_UpdateAddonCommandAsync(KillerAddonViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var service = _killerAddonApiServiceFactory.Create(SelectedKiller!.Id);

                var result = await service.UpdateAsync(new ClientUpdateKillerAddonRequest(model.Id, model.Name, model.ImagePath!, model.Name));

                if (!result.IsSuccess)
                {
                    MessageBox.Show($"Ошибка обновления: {result.StringMessage}");
                    return;
                }

                model.SetImageKey(result.Value);

                model.CommitChanges(model.ToModel());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непредвиденная ошибка: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanExecute_UpdateAddonCommandAsync(KillerAddonViewModel model)
        {
            //if (model is not null)
            //    return model.HasChanges;

            //return false;

            return true;
        }

        #endregion

        #region Команда [RevertAddonChangeCommand]: Откат изменений у улучшения

        public RelayCommand<KillerAddonViewModel>? RevertAddonChangeCommand { get; private set; }

        private void Execute_RevertAddonChangeCommand(KillerAddonViewModel model)
        {
            model?.RevertChanges();
            MenuAddonPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_RevertAddonChangeCommand(KillerAddonViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeletetAddonCommandAsync]: Удаление улучшения

        public RelayCommandAsync<KillerAddonViewModel>? DeleteAddonCommandAsync { get; private set; }

        private async Task Execute_DeleteAddonCommandAsync(KillerAddonViewModel model)
        {
            if (model is null)
                return;

            if (SelectedKiller is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var service = _killerAddonApiServiceFactory.Create(SelectedKiller.Id);

                    var result = await service.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    Addons.Remove(model);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Непредвиденная ошибка: {ex.Message}");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private bool CanExecute_DeleteAddonCommandAsync(KillerAddonViewModel model) => true;

        #endregion


        #region Команда [ManagementAddonAddCommand]: Управление модальным окном для создания улучшения

        public RelayCommand<Visibility>? ManagementAddonAddCommand { get; private set; }

        private void Execute_ManagementAddonAddCommand(Visibility visibility)
        {
            AddonAddModalVisibility = visibility;
            MenuAddonPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_ManagementAddonAddCommand(Visibility visibility) => true;

        #endregion

        #region Команда [ManagementAddonEditCommand]: Управление модальным окном для редактирования улучшения

        public RelayCommand<Visibility>? ManagementAddonEditCommand { get; private set; }

        private void Execute_ManagementAddonEditCommand(Visibility visibility)
        {
            AddonEditModalVisibility = visibility;
            MenuAddonPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_ManagementAddonEditCommand(Visibility visibility) => true;

        #endregion


        #region Команда [ManagementLocalAddonEditCommand]: Управление модальным окном для редактирования локального улучшения

        public RelayCommand<Visibility>? ManagementLocalAddonEditCommand { get; private set; }

        private void Execute_ManagementLocalAddonEditCommand(Visibility visibility)
        {
            LocalAddonEditModalVisibility = visibility;
            MenuLocalAddonPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_ManagementLocalAddonEditCommand(Visibility visibility) => true;

        #endregion

        /*--Перки--*/

        #region Команда [AddLocalPerkCommand]: Добавляет перк локально

        public RelayCommand? AddLocalPerkCommand { get; private set; }

        private void Execute_AddLocalPerkCommand()
        {
            if (NewPerk is null)
                return;

            if (SelectedLocalKiller is null)
            {
                MessageBox.Show("Вы не выбрали локального киллера, кому принадлежит этот перк.");
                return;
            }

            NewPerk.SetKillerLocalID(SelectedLocalKiller?.LocalId);
            LocalPerks.Add(NewPerk);
            ManagementPerkAddCommand?.Execute(Visibility.Collapsed);
            PreparePerkNew();
        }

        private bool CanExecute_AddLocalPerkCommand() => true;

        #endregion

        #region Команда [DeleteLocalPerkCommand]: Удаляет локальный перк

        public RelayCommand<KillerPerkViewModel>? DeleteLocalPerkCommand { get; private set; }

        private void Execute_DeleteLocalPerkCommand(KillerPerkViewModel model)
        {
            LocalPerks.Remove(model);
            MenuLocalPerkPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_DeleteLocalPerkCommand(KillerPerkViewModel model) => true;

        #endregion


        #region Команда [SelectPerkImageCommand]: Выбор изображение перка

        public RelayCommand<KillerPerkViewModel>? SelectPerkImageCommand { get; private set; }

        private void Execute_SelectPerkImageCommand(KillerPerkViewModel model)
        {
            var path = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");
            model?.SetImage(ImageHelper.ImageFromFilePath(path!), path);
        }

        private bool CanExecute_SelectPerkImageCommand(KillerPerkViewModel model) => true;

        #endregion


        #region Команда [UpdatePerkCommandAsync]: Обновление записи Perk

        public RelayCommandAsync<KillerPerkViewModel>? UpdatePerkCommandAsync { get; private set; }

        private async Task Execute_UpdatePerkCommandAsync(KillerPerkViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var service = _killerPerkApiServiceFactory.Create(SelectedKiller!.Id);
                
                var result = await service.UpdateAsync(new ClientUpdateKillerPerkRequest(model.Id, model.Name, model.ImagePath!, model.Name));

                if (!result.IsSuccess)
                {
                    MessageBox.Show($"Ошибка обновления: {result.StringMessage}");
                    return;
                }

                model.SetImageKey(result.Value);

                model.CommitChanges(model.ToModel());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непредвиденная ошибка: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanExecute_UpdatePerkCommandAsync(KillerPerkViewModel model)
        {
            //if (model is not null)
            //    return model.HasChanges;

            //return false;

            return true;
        }

        #endregion

        #region Команда [RevertPerkChangeCommand]: Откат изменений у улучшения

        public RelayCommand<KillerPerkViewModel>? RevertPerkChangeCommand { get; private set; }

        private void Execute_RevertPerkChangeCommand(KillerPerkViewModel model)
        {
            model?.RevertChanges();
            MenuPerkPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_RevertPerkChangeCommand(KillerPerkViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion 

        #region Команда [DeletetPerkCommandAsync]: Удаление перка

        public RelayCommandAsync<KillerPerkViewModel>? DeletePerkCommandAsync { get; private set; }

        private async Task Execute_DeletePerkCommandAsync(KillerPerkViewModel model)
        {
            if (model is null)
                return;

            if (SelectedKiller is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var service = _killerPerkApiServiceFactory.Create(SelectedKiller.Id);

                    var result = await service.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    Perks.Remove(model);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Непредвиденная ошибка: {ex.Message}");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private bool CanExecute_DeletePerkCommandAsync(KillerPerkViewModel model) => true;

        #endregion


        #region Команда [ManagementPerkAddCommand]: Управление модальным окном для создания перка

        public RelayCommand<Visibility>? ManagementPerkAddCommand { get; private set; }

        private void Execute_ManagementPerkAddCommand(Visibility visibility)
        {
            PerkAddModalVisibility = visibility;
            MenuPerkPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_ManagementPerkAddCommand(Visibility visibility) => true;

        #endregion

        #region Команда [ManagementPerkEditCommand]: Управление модальным окном для редактирования перка

        public RelayCommand<Visibility>? ManagementPerkEditCommand { get; private set; }

        private void Execute_ManagementPerkEditCommand(Visibility visibility)
        {
            PerkEditModalVisibility = visibility;
            MenuPerkPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_ManagementPerkEditCommand(Visibility visibility) => true;

        #endregion

        #region Команда [ManagementLocalPerkEditCommand]: Управление модальным окном для редактирования локального перка

        public RelayCommand<Visibility>? ManagementLocalPerkEditCommand { get; private set; }

        private void Execute_ManagementLocalPerkEditCommand(Visibility visibility)
        {
            LocalPerkEditModalVisibility = visibility;
            MenuLocalPerkPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_ManagementLocalPerkEditCommand(Visibility visibility) => true;

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Методы [GetAll]: Получение Killers, KillerAddons, KillerPerks

        private async Task GetAllKillers()
        {
            var killers = await _killerService.GetAllAsync();

            foreach (var killer in killers)
            {
                var killerUrl = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.KillerPortraits}/{killer.KillerImageKey}");
                var abilityUrl = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.KillerAbilities}/{killer.AbilityImageKey}");

                var killerVM = new KillerViewModel(killer);

                killerVM.SetKillerImage(await ImageHelper.ImageFromUrlAsync(killerUrl), killerUrl);
                killerVM.SetAbilityImage(await ImageHelper.ImageFromUrlAsync(abilityUrl), abilityUrl);

                Killers.Add(killerVM);
            }
                

            SelectedKiller = Killers.FirstOrDefault();
        }

        private async Task GetAddonsForKiller()
        {
            if (SelectedKiller is null)
            {
                Addons.Clear();
                return;
            }

            var service = _killerAddonApiServiceFactory.Create(SelectedKiller!.Id);

            var addons = await service.GetAllAsync();

            Addons.Clear();

            foreach (var addon in addons)
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.KillerAddons(SelectedKiller.Name)}/{addon.ImageKey}");
                var addonVM = new KillerAddonViewModel(addon);

                addonVM.SetImage(await ImageHelper.ImageFromUrlAsync(url), string.Empty);

                Addons.Add(addonVM);
            }
        }

        private async Task GetPerksForKiller()
        {
            if (SelectedKiller is null)
            {
                Perks.Clear();
                return;
            }

            var service = _killerPerkApiServiceFactory.Create(SelectedKiller!.Id);

            var perks = await service.GetAllAsync();

            Perks.Clear();

            foreach (var perk in perks)
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.KillerPerks(SelectedKiller.Name)}/{perk.ImageKey}");
                var perkVM = new KillerPerkViewModel(perk);

                perkVM.SetImage(await ImageHelper.ImageFromUrlAsync(url), string.Empty);

                Perks.Add(perkVM);
            }
        }

        #endregion

        #region Методы [GetAll]: Получение Ririties, PerkCategories

        private async Task GetAllRarities()
        {
            var rarities = await _rarityService.GetAllAsync();

            foreach (var rarity in rarities)
                Rarities.Add(rarity);
        }

        private async Task GetAllPerkCategories()
        {
            var categories = await _killerPerkCategoryService.GetAllAsync();

            foreach (var category in categories)
                PerkCategories.Add(category);
        }

        #endregion

        #region Метод [PrepareNew]: Создание пустого KillerViewModel, KillerAddonViewModel, KillerPerkViewModel для будущего добавление

        private void PrepareKillerNew() => NewKiller = new KillerViewModel(KillerSoloResponse.Empty);
        private void PrepareAddonNew() => NewAddon = new KillerAddonViewModel(KillerAddonResponse.Empty);
        private void PreparePerkNew() => NewPerk = new KillerPerkViewModel(KillerPerkResponse.Empty);

        #endregion
    }
}