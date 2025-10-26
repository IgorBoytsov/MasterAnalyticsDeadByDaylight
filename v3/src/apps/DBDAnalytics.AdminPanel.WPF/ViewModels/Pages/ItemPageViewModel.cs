using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Item;
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.ItemAddon;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Rarity;
using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.FIleStorageService.Client;
using DBDAnalytics.Shared.Contracts.Constants;
using DBDAnalytics.Shared.Contracts.Responses;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
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
    internal sealed class ItemPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IItemService _itemService;
        private readonly IItemAddonApiServiceFactory _itemAddonApiServiceFactory;
        private readonly IRarityService _rarityService;
        private readonly IFileDialogService _fileDialogService;
        private readonly IFileStorageService _fileStorageService;

        public ItemPageViewModel(
            IItemService itemService,
            IItemAddonApiServiceFactory itemAddonApiServiceFactory,
            IRarityService rarityService,
            IFileDialogService fileDialogService,
            IFileStorageService fileStorageService)
        {
            _itemService = itemService;
            _itemAddonApiServiceFactory = itemAddonApiServiceFactory;
            _rarityService = rarityService;
            _fileDialogService = fileDialogService;
            _fileStorageService = fileStorageService;

            PrepareItemNew();
            PrepareAddonNew();

            InitializeCommand();
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsBusy = true;

            try
            {
                await GetAllRaritiesAsync();

                await GetAllItemsAsync();

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

        public ObservableCollection<ItemViewModel> Items { get; private set; } = [];
        public ObservableCollection<ItemAddonViewModel> Addons { get; private set; } = [];

        public ObservableCollection<ItemViewModel> LocalItems { get; private set; } = [];
        public ObservableCollection<ItemAddonViewModel> LocalAddons { get; private set; } = [];

        public ObservableCollection<RarityResponse> Rarities { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController MenuItemPopup { get; } = new PopupController();
        public IPopupController MenuAddonPopup { get; } = new PopupController();

        public IPopupController MenuLocalItemPopup { get; } = new PopupController();
        public IPopupController MenuLocalAddonPopup { get; } = new PopupController();

        #region Свойство: [SelectedItem] Метод: [OnSelectedItemPropertyChanged]

        private ItemViewModel? _selectedItem;
        public ItemViewModel? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem is not null)
                    _selectedItem.PropertyChanged -= OnSelectedItemPropertyChanged;

                SetProperty(ref _selectedItem, value);

                if (IsProcessAddedItem == false)
                    _ = GetAddonsForItem();

                if (_selectedItem is not null)
                    _selectedItem.PropertyChanged += OnSelectedItemPropertyChanged;
            }
        }

        private void OnSelectedItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateItemCommandAsync?.RaiseCanExecuteChanged();
            RevertItemChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [SelectedLocalItem] Метод: [OnSelectedLocalItemPropertyChanged]

        private ItemViewModel? _selectedLocalItem;
        public ItemViewModel? SelectedLocalItem
        {
            get => _selectedLocalItem;
            set
            {
                if (_selectedLocalItem is not null)
                    _selectedLocalItem.PropertyChanged -= OnSelectedLocalItemPropertyChanged;

                SetProperty(ref _selectedLocalItem, value);

                if (_selectedLocalItem is not null)
                    _selectedLocalItem.PropertyChanged += OnSelectedLocalItemPropertyChanged;
            }
        }

        private void OnSelectedLocalItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Свойство: [NewItem] Метод: [OnNewItemPropertyChanged]

        private ItemViewModel? _newItem;
        public ItemViewModel? NewItem
        {
            get => _newItem;
            set
            {
                if (_newItem is not null)
                    _newItem.PropertyChanged -= OnNewItemPropertyChanged;

                SetProperty(ref _newItem, value);

                if (_newItem is not null)
                    _newItem.PropertyChanged += OnNewItemPropertyChanged;
            }
        }

        private void OnNewItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Свойство: [SelectedAddon] Метод: [OnSelectedAddonPropertyChanged]

        private ItemAddonViewModel? _selectedAddon;
        public ItemAddonViewModel? SelectedAddon
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
            UpdateAddonCommandAsync?.RaiseCanExecuteChanged();
            RevertAddonChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [SelectedLocalAddon] Метод: [OnSelectedLocalAddonPropertyChanged]

        private ItemAddonViewModel? _selectedLocalAddon;
        public ItemAddonViewModel? SelectedLocalAddon
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

        private ItemAddonViewModel? _newAddon;
        public ItemAddonViewModel? NewAddon
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

        }

        #endregion

        #region Свойство: [IsProcessAddedItem] Добавляеться ли в данный момент предмет или нет

        private bool IsProcessAddedItem = false;

        #endregion

        /*--Visibility Modal--*/

        #region Свойства: [ModalVisibility]: Отображение модальных окон

        private Visibility _itemAddModalVisibility = Visibility.Collapsed;
        public Visibility ItemAddModalVisibility
        {
            get => _itemAddModalVisibility;
            set => SetProperty(ref _itemAddModalVisibility, value);
        }

        private Visibility _itemEditModalVisibility = Visibility.Collapsed;
        public Visibility ItemEditModalVisibility
        {
            get => _itemEditModalVisibility;
            set => SetProperty(ref _itemEditModalVisibility, value);
        }

        private Visibility _localItemEditModalVisibility = Visibility.Collapsed;
        public Visibility LocalItemEditModalVisibility
        {
            get => _localItemEditModalVisibility;
            set => SetProperty(ref _localItemEditModalVisibility, value);
        }

        private Visibility _addonAddModalVisibility = Visibility.Collapsed;
        public Visibility AddonAddModalVisibility
        {
            get => _addonAddModalVisibility;
            set => SetProperty(ref _addonAddModalVisibility, value);
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

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            AddLocalItemCommand = new RelayCommand(Execute_AddLocalItemCommand, CanExecute_AddLocalItemCommand);
            AddLocalAddonCommand = new RelayCommand(Execute_AddLocalAddonCommand, CanExecute_AddLocalAddonCommand);

            CreateItemCommandAsync = new RelayCommandAsync(Execute_CreateItemCommandAsync, CanExecute_CreateItemCommandAsync);

            DeleteLocalItemCommand = new RelayCommand<ItemViewModel>(Execute_DeleteLocalItemCommand, CanExecute_DeleteLocalItemCommand);
            DeleteLocalAddonCommand = new RelayCommand<ItemAddonViewModel>(Execute_DeleteLocalAddonCommand, CanExecute_DeleteLocalAddonCommand);

            DeleteItemCommandAsync = new RelayCommandAsync<ItemViewModel>(Execute_DeleteItemCommandAsync, CanExecute_DeleteItemCommandAsync);
            DeleteAddonCommandAsync = new RelayCommandAsync<ItemAddonViewModel>(Execute_DeleteAddonCommandAsync, CanExecute_DeleteAddonCommandAsync);

            UpdateItemCommandAsync = new RelayCommandAsync<ItemViewModel>(Execute_UpdateItemCommandAsync, CanExecute_UpdateItemCommandAsync);
            UpdateAddonCommandAsync = new RelayCommandAsync<ItemAddonViewModel>(Execute_UpdateAddonCommandAsync, CanExecute_UpdateAddonCommandAsync);

            SelectItemImageCommand = new RelayCommand<ItemViewModel>(Execute_SelectItemImageCommand, CanExecute_SelectItemImageCommand);
            SelectAddonImageCommand = new RelayCommand<ItemAddonViewModel>(Execute_SelectAddonImageCommand, CanExecute_SelectAddonImageCommand);

            ManagementItemAddCommand = new RelayCommand<Visibility>(Execute_ManagementItemAddCommand, CanExecute_ManagementItemAddCommand);
            ManagementItemEditCommand = new RelayCommand<Visibility>(Execute_ManagementItemEditCommand, CanExecute_ManagementItemEditCommand);
            ManagementLocalItemEditCommand = new RelayCommand<Visibility>(Execute_ManagementLocalItemEditCommand, CanExecute_ManagementLocalItemEditCommand);

            ManagementAddonAddCommand = new RelayCommand<Visibility>(Execute_ManagementAddonAddCommand, CanExecute_ManagementAddonAddCommand);
            ManagementAddonEditCommand = new RelayCommand<Visibility>(Execute_ManagementAddonEditCommand, CanExecute_ManagementAddonEditCommand);
            ManagementLocalAddonEditCommand = new RelayCommand<Visibility>(Execute_ManagementLocalAddonEditCommand, CanExecute_ManagementLocalAddonEditCommand);

            RevertItemChangeCommand = new RelayCommand<ItemViewModel>(Execute_RevertItemChangeCommand, CanExecute_RevertItemChangeCommand);
            RevertAddonChangeCommand = new RelayCommand<ItemAddonViewModel>(Execute_RevertAddonChangeCommand, CanExecute_RevertAddonChangeCommand);
        }

        /*--Предметы--*/

        #region Команда [AddLocalItemCommand]: Добавляет предмет локально

        public RelayCommand? AddLocalItemCommand { get; private set; }

        private void Execute_AddLocalItemCommand()
        {
            if (NewItem is null)
                return;

            NewItem.SetLocalID(Guid.NewGuid());
            LocalItems.Add(NewItem);
            ManagementItemAddCommand?.Execute(Visibility.Collapsed);
            PrepareItemNew();
        }

        private bool CanExecute_AddLocalItemCommand() => true;

        #endregion

        #region Команда [CreateItemCommand]: Добавлят полную запись об предмете

        public RelayCommandAsync? CreateItemCommandAsync { get; private set; }

        private async Task Execute_CreateItemCommandAsync()
        {
            IsBusy = true;
            IsProcessAddedItem = true;

            try
            {
                var newItemVM = LocalItems.FirstOrDefault();

                if (newItemVM is null)
                {
                    MessageBox.Show("Вы не добавили предмет");
                    return;
                }

                var result = await _itemService.AddAsync(
                    new ClientAddItemRequest(
                        newItemVM.OldId, newItemVM.Name, newItemVM.ImagePath, 
                        [.. LocalAddons.Select(a => new ClientItemAddonsData(a.OldId, a.Name, a.ImagePath, a.Name, a.Rarity?.Id))]));

                if (!result.IsSuccess)
                {
                    MessageBox.Show($"Не удалось добавить запись: {result.StringMessage}");
                    return;
                }

                var addedItem = new ItemViewModel(new ItemSoloResponse(result.Value.Id, result.Value.OldId, result.Value.Name, result.Value.ImageKey));
                addedItem.SetImage(SelectedLocalItem?.Image, string.Empty);

                Items.Add(addedItem);
                SelectedItem = addedItem;

                Addons.Clear();

                foreach (var addon in result.Value.ItemAddons)
                {
                    if (addon is null)
                        continue;

                    var addonVM = new ItemAddonViewModel(addon, Rarities);

                    Addons.Add(addonVM);
                }

                LocalItems.Clear();
                LocalAddons.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании записи: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                IsProcessAddedItem = false;
            }
        }

        private bool CanExecute_CreateItemCommandAsync() => true;

        #endregion

        #region Команда [DeleteLocalItemCommand]: Удаляет локального предмета

        public RelayCommand<ItemViewModel>? DeleteLocalItemCommand { get; private set; }

        private void Execute_DeleteLocalItemCommand(ItemViewModel model)
        {
            LocalItems.Remove(model);
            MenuLocalItemPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_DeleteLocalItemCommand(ItemViewModel model) => true;

        #endregion
        
        
        #region Команда [SelectItemImageCommand]: Выбор изображение киллера

        public RelayCommand<ItemViewModel>? SelectItemImageCommand { get; private set; }

        private void Execute_SelectItemImageCommand(ItemViewModel model)
        {
            var path = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");
            model?.SetImage(ImageHelper.ImageFromFilePath(path!), path);
        }

        private bool CanExecute_SelectItemImageCommand(ItemViewModel model) => true;

        #endregion


        #region Команда [UpdateItemCommandAsync]: Обновление записи предмета

        public RelayCommandAsync<ItemViewModel>? UpdateItemCommandAsync { get; private set; }

        private async Task Execute_UpdateItemCommandAsync(ItemViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _itemService.UpdateAsync(new ClientUpdateItemRequest(model.Id, model.Name, model.ImagePath!, model.Name));

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

        private bool CanExecute_UpdateItemCommandAsync(ItemViewModel model)
        {
            //if (model is not null)
            //    return model.HasChanges;

            //return false;

            return true;
        }

        #endregion

        #region Команда [DeletetItemCommandAsync]: Удаление записи об предмете и его сущьностях

        public RelayCommandAsync<ItemViewModel>? DeleteItemCommandAsync { get; private set; }

        private async Task Execute_DeleteItemCommandAsync(ItemViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _itemService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    Items.Remove(model);
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

        private bool CanExecute_DeleteItemCommandAsync(ItemViewModel model) => true;

        #endregion

        #region Команда [RevertItemChangeCommand]: Откат изменений у предмета

        public RelayCommand<ItemViewModel>? RevertItemChangeCommand { get; private set; }

        private void Execute_RevertItemChangeCommand(ItemViewModel model)
        {
            model?.RevertChanges();
            MenuItemPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_RevertItemChangeCommand(ItemViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion 


        #region Команда [ManagementItemAddCommand]: Управление модальным окном для добавления предмета

        public RelayCommand<Visibility>? ManagementItemAddCommand { get; private set; }

        private void Execute_ManagementItemAddCommand(Visibility visibility) => ItemAddModalVisibility = visibility;

        private bool CanExecute_ManagementItemAddCommand(Visibility visibility) => true;

        #endregion

        #region Команда [ManagementItemEditCommand]: Управление модальным окном для редактирования предмета

        public RelayCommand<Visibility>? ManagementItemEditCommand { get; private set; }

        private void Execute_ManagementItemEditCommand(Visibility visibility)
        {
            ItemEditModalVisibility = visibility;
            MenuItemPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_ManagementItemEditCommand(Visibility visibility) => true;

        #endregion

        #region Команда [ManagementLocalItemEditCommand]: Управление модальным окном для редактирования предмета

        public RelayCommand<Visibility>? ManagementLocalItemEditCommand { get; private set; }

        private void Execute_ManagementLocalItemEditCommand(Visibility visibility)
        {
            LocalItemEditModalVisibility = visibility;
            MenuLocalItemPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_ManagementLocalItemEditCommand(Visibility visibility) => true;

        #endregion

        /*--Улучшения--*/

        #region Команда [AddLocalAddonCommand]: Добавляет улучшение локально

        public RelayCommand? AddLocalAddonCommand { get; private set; }

        private void Execute_AddLocalAddonCommand()
        {
            if (NewAddon is null)
                return;

            if (SelectedLocalItem is null)
            {
                MessageBox.Show("Вы не выбрали локальный предмет, к которому добавляете улучшение");
                return;
            }

            NewAddon.SetItemLocalId(SelectedLocalItem.LocalId);
            LocalAddons.Add(NewAddon);
            ManagementAddonAddCommand?.Execute(Visibility.Collapsed);
            PrepareAddonNew();
        }

        private bool CanExecute_AddLocalAddonCommand()
        {
            return true;
        }

        #endregion

        #region Команда [DeleteLocalAddonCommand]: Удаляет локальный аддон

        public RelayCommand<ItemAddonViewModel>? DeleteLocalAddonCommand { get; private set; }

        private void Execute_DeleteLocalAddonCommand(ItemAddonViewModel model)
        {
            LocalAddons.Remove(model);
            MenuLocalAddonPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_DeleteLocalAddonCommand(ItemAddonViewModel model) => true;

        #endregion


        #region Команда [SelectItemAddonImageCommand]: Выбор изображение киллера

        public RelayCommand<ItemAddonViewModel>? SelectAddonImageCommand { get; private set; }

        private void Execute_SelectAddonImageCommand(ItemAddonViewModel model)
        {
            var path = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");
            model?.SetImage(ImageHelper.ImageFromFilePath(path!), path);
        }

        private bool CanExecute_SelectAddonImageCommand(ItemAddonViewModel model) => true;

        #endregion


        #region Команда [UpdateAddonCommandAsync]: Обновление записи Addon

        public RelayCommandAsync<ItemAddonViewModel>? UpdateAddonCommandAsync { get; private set; }

        private async Task Execute_UpdateAddonCommandAsync(ItemAddonViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var service = _itemAddonApiServiceFactory.Create(SelectedItem!.Id);

                var result = await service.UpdateAsync(new ClientUpdateItemAddonRequest(model.Id, model.Name, model.ImagePath!, model.Name));

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

        private bool CanExecute_UpdateAddonCommandAsync(ItemAddonViewModel model)
        {
            //if (model is not null)
            //    return model.HasChanges;

            //return false;

            return true;
        }

        #endregion

        #region Команда [DeletetAddonCommandAsync]: Удаление улучшения

        public RelayCommandAsync<ItemAddonViewModel>? DeleteAddonCommandAsync { get; private set; }

        private async Task Execute_DeleteAddonCommandAsync(ItemAddonViewModel model)
        {
            if (model is null)
                return;

            if (SelectedItem is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var service = _itemAddonApiServiceFactory.Create(SelectedItem.Id);

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

        private bool CanExecute_DeleteAddonCommandAsync(ItemAddonViewModel model) => true;

        #endregion

        #region Команда [RevertAddonChangeCommand]: Откат изменений у перка

        public RelayCommand<ItemAddonViewModel>? RevertAddonChangeCommand { get; private set; }

        private void Execute_RevertAddonChangeCommand(ItemAddonViewModel model)
        {
            model?.RevertChanges();
            MenuAddonPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_RevertAddonChangeCommand(ItemAddonViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

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

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Методы [GetAll]: Получение Items, ItemAddons

        private async Task GetAllItemsAsync()
        {
            var items = await _itemService.GetAllAsync();

            foreach (var item in items)
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.Items}/{item.ImageKey}");
                
                var itemVM = new ItemViewModel(item);
                itemVM.SetImage(await ImageHelper.ImageFromUrlAsync(url), string.Empty);

                Items.Add(itemVM);
            }
                
            SelectedItem = Items.FirstOrDefault();
        }

        private async Task GetAddonsForItem()
        {
            if (SelectedItem is null)
            {
                Addons.Clear();
                return;
            }

            var service = _itemAddonApiServiceFactory.Create(SelectedItem!.Id);

            var addons = await service.GetAllAsync();

            Addons.Clear();

            foreach (var addon in addons)
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.ItemAddons(SelectedItem.Name)}/{addon.ImageKey}");

                var addonVM = new ItemAddonViewModel(addon, Rarities);
                addonVM.SetRarity(addon.RarityId);
                addonVM.SetImage(await ImageHelper.ImageFromUrlAsync(url), string.Empty);

                Addons.Add(addonVM);
            }
        }

        private async Task GetAllRaritiesAsync()
        {
            var rarities = await _rarityService.GetAllAsync();

            foreach (var rarity in rarities)
                Rarities.Add(rarity);
        }

        #endregion

        #region Методы [PrepareNew]: Создание пустого ItemViewModel, ItemAddonViewModel для будущего добавление

        private void PrepareItemNew() => NewItem = new ItemViewModel(ItemSoloResponse.Empty);
        private void PrepareAddonNew() => NewAddon = new ItemAddonViewModel(ItemAddonResponse.Empty, Rarities);

        #endregion
    }
}