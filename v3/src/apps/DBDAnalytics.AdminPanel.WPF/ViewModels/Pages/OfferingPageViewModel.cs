using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Offering;
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.OfferingCategory;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Rarity;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Role;
using DBDAnalytics.FIleStorageService.Client;
using DBDAnalytics.Shared.Contracts.Constants;
using DBDAnalytics.Shared.Contracts.Responses;
using DBDAnalytics.Shared.Contracts.Responses.Offering;
using Shared.WPF.Commands;
using Shared.WPF.Helpers;
using Shared.WPF.Services;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Pages
{
    internal sealed class OfferingPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IOfferingService _offeringService;
        private readonly IRoleService _roleService;
        private readonly IRarityService _rarityService;
        private readonly IOfferingCategoryService _offeringCategoryService;
        private readonly IFileDialogService _fileDialogService;
        private readonly IFileStorageService _fileStorageService;

        public OfferingPageViewModel(
            IOfferingService offeringService,
            IRoleService roleService,
            IRarityService rarityService,
            IOfferingCategoryService offeringCategoryService,
            IFileDialogService fileDialogService,
            IFileStorageService fileStorageService)
        {
            _offeringCategoryService = offeringCategoryService;
            _roleService = roleService;
            _offeringService = offeringService;
            _rarityService = rarityService;
            _fileDialogService = fileDialogService;
            _fileStorageService = fileStorageService;

            PrepareNew();
            InitializeCommand();
        }

        public async Task InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsBusy = true;

            try
            {
                await GetAllRoles();
                await GetAllRarities();
                await GetAllOfferingCategories();

                await GetAllOfferings();

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

        public ObservableCollection<OfferingViewModel> Offerings { get; private set; } = [];

        public ObservableCollection<RoleResponse> Roles { get; private set; } = [];
        public ObservableCollection<RarityResponse> Rarities { get; private set; } = [];
        public ObservableCollection<OfferingCategoryResponse> OfferingCategories { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        public IPopupController MenuPopup { get; } = new PopupController();

        #region Свойство [SelectedOffering] Метод: [OnSelectedOfferingPropertyChanged]

        private OfferingViewModel? _selectedOffering;
        public OfferingViewModel? SelectedOffering
        {
            get => _selectedOffering;
            set
            {
                if (_selectedOffering is not null)
                    _selectedOffering.PropertyChanged -= OnSelectedOfferingPropertyChanged;

                SetProperty(ref _selectedOffering, value);

                if (_selectedOffering is not null)
                    _selectedOffering.PropertyChanged += OnSelectedOfferingPropertyChanged;
            }
        }

        private void OnSelectedOfferingPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RevertChangeCommand?.RaiseCanExecuteChanged();
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            DeleteCommandAsync?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство [NewOffering] Метод: [OnNewOfferingPropertyChanged]

        private OfferingViewModel? _newOffering;
        public OfferingViewModel? NewOffering
        {
            get => _newOffering;
            set
            {
                if (_newOffering is not null)
                    _newOffering.PropertyChanged -= OnNewOfferingPropertyChanged;

                SetProperty(ref _newOffering, value);

                if(_newOffering is not null)
                    _newOffering.PropertyChanged += OnNewOfferingPropertyChanged;
            }
        }

        private void OnNewOfferingPropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        #region Свойство [NewImagePath]: Путь к добовляемому изображению

        private string? _newImagePath;
        public string? NewImagePath
        {
            get => _newImagePath;
            private set => SetProperty(ref _newImagePath, value);
        }

        #endregion

        #region Свойство [UpdateImagePath]: Путь к изменяемому изображению

        private string? _updateImagePath;
        public string? UpdateImagePath
        {
            get => _updateImagePath;
            private set => SetProperty(ref _updateImagePath, value);
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<OfferingViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommandAsync);
            DeleteCommandAsync = new RelayCommandAsync<OfferingViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommandAsync);
            RevertChangeCommand = new RelayCommand<OfferingViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);

            SelectNewImageCommand = new RelayCommand(Execute_SelectNewImageCommand, CanExecute_SelectNewImageCommand);
            SelectUpdateImageCommand = new RelayCommand(Execute_SelectUpdateImageCommand, CanExecute_SelectUpdateImageCommand);

            CopyIDCommand = new RelayCommand<string>(Execute_CopyIDCommand, CanExecute_CopyIDCommand);
            CopyAllInfoOfferingCommand = new RelayCommand<OfferingViewModel>(Execute_CopyAllInfoOfferingCommand, CanExecute_CopyAllInfoOfferingCommand);
            ShowEditAndHideMenuCommand = new RelayCommand<object>(Execute_ShowEditAndHideMenuCommand, CanExecute_ShowEditAndHideMenuCommand);
        }

        /*--CRUD--*/

        #region Команда [CreateCommandAsync]: Создание Offering    

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewOffering is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewOffering.ToModel();

                var response = await _offeringService.AddAsync(newModel.OldId, newModel.Name, newModel.Name, NewImagePath!, newModel.RoleId, newModel.RarityId, newModel.CategoryId);

                if (response is null)
                {
                    MessageBox.Show("Не удалось добавить запись");
                    return;
                }

                var addedOffering = new OfferingViewModel(response.Value!, Roles, Rarities, OfferingCategories);

                SetValuesOffering(addedOffering, response.Value!);
                addedOffering.SetImage(ImageHelper.ImageFromFilePath(NewImagePath!), NewImagePath!);


                Offerings.Add(addedOffering);

                PrepareNew();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании записи: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanExecute_CreateCommandAsync() 
            => NewOffering != null && 
                !string.IsNullOrWhiteSpace(NewOffering.Name) && NewOffering.OldId >= 0 &&
                 NewOffering.Role is not null;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление записи Offering

        public RelayCommandAsync<OfferingViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(OfferingViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _offeringService.UpdateAsync(model.Id, model.Name, model.Name, UpdateImagePath!, model.Role.Id, model.Rarity?.Id, model.Category?.Id);

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

        private bool CanExecute_UpdateCommandAsync(OfferingViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление записи Offering

        public RelayCommandAsync<OfferingViewModel>? DeleteCommandAsync { get; private set; }

        private async Task Execute_DeleteCommandAsync(OfferingViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _offeringService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    Offerings.Remove(model);
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

        private bool CanExecute_DeleteCommandAsync(OfferingViewModel model) => true;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<OfferingViewModel>? RevertChangeCommand { get; private set; }

        private void Execute_RevertChangeCommand(OfferingViewModel model)
        {
            model?.RevertChanges();
            MenuPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_RevertChangeCommand(OfferingViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion 

        /*--Изображение--*/

        #region Команда [SelectNewImageCommand]: Выбор изображения для создания подношения

        public RelayCommand? SelectNewImageCommand { get; private set; }

        private void Execute_SelectNewImageCommand()
        {
            NewImagePath = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");
            NewOffering?.SetImage(ImageHelper.ImageFromFilePath(NewImagePath!), NewImagePath!);
        }

        private bool CanExecute_SelectNewImageCommand() => true;

        #endregion

        #region Команда [SelectUpdateImageCommand]: Выбор изображения для создания подношения

        public RelayCommand? SelectUpdateImageCommand { get; private set; }

        private void Execute_SelectUpdateImageCommand()
        {
            var path = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");

            if (path is not null)
            {
                UpdateImagePath = path;
                SelectedOffering?.SetImage(ImageHelper.ImageFromFilePath(UpdateImagePath!), UpdateImagePath);
            }
        }

        private bool CanExecute_SelectUpdateImageCommand() => true;

        #endregion

        /*--Копирования--*/

        #region Команда [CopyIDCommand]: Копирования ID записи

        public RelayCommand<string>? CopyIDCommand { get; private set; }

        private void Execute_CopyIDCommand(string id) => Clipboard.SetText(id);

        private bool CanExecute_CopyIDCommand(string id) => !string.IsNullOrWhiteSpace(id);

        #endregion

        #region Команда [CopyAllInfoOfferingCommand]: Копирования всей иформации

        public RelayCommand<OfferingViewModel>? CopyAllInfoOfferingCommand { get; private set; }

        private void Execute_CopyAllInfoOfferingCommand(OfferingViewModel model)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"ID: {model.Id}");
            sb.AppendLine($"ImageKey: {model.ImageKey}");
            sb.AppendLine($"Старое ID: {model.OldId}");
            sb.AppendLine($"Имя: {model.Name}");
            sb.AppendLine($"Роль: {model.Role.Name} (ID: {model.RoleId}");
            sb.AppendLine($"Категория: {model.Category?.Name} (ID: {model.CategoryId})");
            sb.AppendLine($"Качество: {model.Rarity?.Name} (ID: {model.RarityId})");

            Clipboard.SetText(sb.ToString());
        }

        private bool CanExecute_CopyAllInfoOfferingCommand(OfferingViewModel model) => true;

        #endregion

        /*--Popups--*/

        #region Команда [ShowEditAndHideMenuCommand]: Управление всплывающеми окнами

        public RelayCommand<object>? ShowEditAndHideMenuCommand { get; private set; }

        private void Execute_ShowEditAndHideMenuCommand(object parameter)
        {
            UIElement? target = MenuPopup.PlacementTarget;

            if (target == null)
            {
                MessageBox.Show("Не удалось установить объект к которому нужно прикрепить Popup");
                return;
            }

            MenuPopup.HideCommand.Execute(null);
            EditPopup.ShowCommand.Execute(target);
        }

        private bool CanExecute_ShowEditAndHideMenuCommand(object parameter) => true;

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [GetAll]: Получение Offerings, Roles, Rarities, OfferingCategories

        public async Task GetAllOfferings()
        {
            var offerings = await _offeringService.GetAllAsync();

            foreach (var offering in offerings)
            {
                string filePath = FileStoragePaths.GetOfferingPathForRole(offering.RoleId);

                var url = await _fileStorageService.GetPresignedLinkAsync($"{filePath}/{offering.ImageKey}");
                var offeringVM = new OfferingViewModel(offering, Roles, Rarities, OfferingCategories);

                offeringVM.SetImage(await ImageHelper.ImageFromUrlAsync(url), string.Empty);
                SetValuesOffering(offeringVM, offering);

                Offerings.Add(offeringVM);
            }     
        }

        public async Task GetAllRoles()
        {
            var roles = await _roleService.GetAllAsync();

            foreach (var role in roles)
                Roles.Add(role);
        }

        public async Task GetAllRarities()
        {
            var rarities = await _rarityService.GetAllAsync();

            foreach (var rarity in rarities)
                Rarities.Add(rarity);
        }

        public async Task GetAllOfferingCategories()
        {
            var offeringCategories = await _offeringCategoryService.GetAllAsync();

            foreach (var offeringCategory in offeringCategories)
                OfferingCategories.Add(offeringCategory);
        }

        #endregion

        #region Метод [PrepareNew]: Создание пустого OfferingViewModel для будущего добавление

        private void PrepareNew() => NewOffering = new OfferingViewModel(OfferingResponse.Empty, Roles, Rarities, OfferingCategories);

        #endregion

        #region Метод: [SetValuesOffering]: Установка зависимых значений для UI

        private void SetValuesOffering(OfferingViewModel model, OfferingResponse source)
        {
            model.SetRole(Roles.FirstOrDefault(r => r.Id == source.RoleId)!);
            model.SetRarity(Rarities.FirstOrDefault(r => r.Id == source.RarityId)!);
            model.SetCategory(OfferingCategories.FirstOrDefault(ofc => ofc.Id == source.CategoryId)!);
        }

        #endregion
    }
}