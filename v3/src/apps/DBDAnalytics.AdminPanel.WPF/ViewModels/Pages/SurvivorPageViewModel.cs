using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.Survivor;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerk;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerkCategory;
using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.FIleStorageService.Client;
using DBDAnalytics.Shared.Contracts.Constants;
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
    internal sealed class SurvivorPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly ISurvivorService _survivorService;
        private readonly ISurvivorPerkApiServiceFactory _survivorPerkApiServiceFactory;
        private readonly ISurvivorPerkCategoryService _survivorPerkCategoryService;
        private readonly IFileDialogService _fileDialogService;
        private readonly IFileStorageService _fileStorageService;

        public SurvivorPageViewModel(
            ISurvivorService survivorService,
            ISurvivorPerkApiServiceFactory survivorPerkApiServiceFactory,
            ISurvivorPerkCategoryService survivorPerkCategoryService,
            IFileDialogService fileDialogService,
            IFileStorageService fileStorageService)
        {
            _survivorService = survivorService;
            _survivorPerkApiServiceFactory = survivorPerkApiServiceFactory;
            _survivorPerkCategoryService = survivorPerkCategoryService;
            _fileDialogService = fileDialogService;
            _fileStorageService = fileStorageService;

            PrepareSurvivorNew();
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
                await GetAllPerkCategoriesAsync();

                await GetAllSurvivorsAsync();

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

        public ObservableCollection<SurvivorViewModel> Survivors { get; private set; } = [];
        public ObservableCollection<SurvivorPerkViewModel> Perks { get; private set; } = [];

        public ObservableCollection<SurvivorViewModel> LocalSurvivors { get; private set; } = [];
        public ObservableCollection<SurvivorPerkViewModel> LocalPerks { get; private set; } = [];

        public ObservableCollection<SurvivorPerkCategoryResponse> PerkCategories { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController MenuSurvivorPopup { get; } = new PopupController();
        public IPopupController MenuPerkPopup { get; } = new PopupController();

        public IPopupController MenuLocalSurvivorPopup { get; } = new PopupController();
        public IPopupController MenuLocalPerkPopup { get; } = new PopupController();

        #region Свойство: [SelectedSurvivor] Метод: [OnSelectedSurvivorPropertyChanged]

        private SurvivorViewModel? _selectedSurvivor;
        public SurvivorViewModel? SelectedSurvivor
        {
            get => _selectedSurvivor;
            set
            {
                if (_selectedSurvivor is not null)
                    _selectedSurvivor.PropertyChanged -= OnSelectedSurvivorPropertyChanged;

                SetProperty(ref _selectedSurvivor, value);

                if (IsProcessAddedSurvivor == false)
                    _ = GetPerksForSurvivor();

                if (_selectedSurvivor is not null)
                    _selectedSurvivor.PropertyChanged += OnSelectedSurvivorPropertyChanged;
            }
        }

        private void OnSelectedSurvivorPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateSurvivorCommandAsync?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [SelectedLocalSurvivor] Метод: [OnSelectedLocalSurvivorPropertyChanged]

        private SurvivorViewModel? _selectedLocalSurvivor;
        public SurvivorViewModel? SelectedLocalSurvivor
        {
            get => _selectedLocalSurvivor;
            set
            {
                if (_selectedLocalSurvivor is not null)
                    _selectedLocalSurvivor.PropertyChanged -= OnSelectedLocalSurvivorPropertyChanged;

                SetProperty(ref _selectedLocalSurvivor, value);

                if (_selectedLocalSurvivor is not null)
                    _selectedLocalSurvivor.PropertyChanged += OnSelectedLocalSurvivorPropertyChanged;
            }
        }

        private void OnSelectedLocalSurvivorPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Свойство: [NewSurvivor] Метод: [OnNewSurvivorPropertyChanged]

        private SurvivorViewModel? _newSurvivor;
        public SurvivorViewModel? NewSurvivor
        {
            get => _newSurvivor;
            set
            {
                if (_newSurvivor is not null)
                    _newSurvivor.PropertyChanged -= OnNewSurvivorPropertyChanged;

                SetProperty(ref _newSurvivor, value);

                if (_newSurvivor is not null)
                    _newSurvivor.PropertyChanged += OnNewSurvivorPropertyChanged;
            }
        }

        private void OnNewSurvivorPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Свойство: [SelectedPerk] Метод: [OnSelectedPerkPropertyChanged]

        private SurvivorPerkViewModel? _selectedPerk;
        public SurvivorPerkViewModel? SelectedPerk
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
            RevertPerkChangeCommand?.RaiseCanExecuteChanged();
            UpdateSurvivorPerkCommandAsync?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [SelectedLocalPerk] Метод: [OnSelectedLocalPerkPropertyChanged]

        private SurvivorPerkViewModel? _selectedLocalPerk;
        public SurvivorPerkViewModel? SelectedLocalPerk
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

        #region Свойство: [MewPerk] Метод: [OnMewPerkPropertyChanged]

        private SurvivorPerkViewModel? _newPerk;
        public SurvivorPerkViewModel? NewPerk
        {
            get => _newPerk;
            set
            {
                if (_newPerk is not null)
                    _newPerk.PropertyChanged -= OnMewPerkPropertyChanged;

                SetProperty(ref _newPerk, value);

                if (_newPerk is not null)
                    _newPerk.PropertyChanged += OnMewPerkPropertyChanged;
            }
        }

        private void OnMewPerkPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Свойство: [IsProcessAddedSurvivor] Добавляеться ли в данный момент выживший или нет

        private bool IsProcessAddedSurvivor = false;

        #endregion

        /*--Visibility Modal--*/

        #region Свойства: [ModalVisibility]: Отображение модальных окон

        private Visibility _survivorAddModalVisibility = Visibility.Collapsed;
        public Visibility SurvivorAddModalVisibility
        {
            get => _survivorAddModalVisibility;
            set => SetProperty(ref _survivorAddModalVisibility, value);
        }

        private Visibility _survivorEditModalVisibility = Visibility.Collapsed;
        public Visibility SurvivorEditModalVisibility
        {
            get => _survivorEditModalVisibility;
            set => SetProperty(ref _survivorEditModalVisibility, value);
        }

        private Visibility _localSurvivorEditModalVisibility = Visibility.Collapsed;
        public Visibility LocalSurvivorEditModalVisibility
        {
            get => _localSurvivorEditModalVisibility;
            set => SetProperty(ref _localSurvivorEditModalVisibility, value);
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
            AddLocalSurvivorCommand = new RelayCommand(Execute_AddLocalSurvivorCommand, CanExecute_AddLocalSurvivorCommand);
            AddLocalPerkCommand = new RelayCommand(Execute_AddLocalPerkCommand, CanExecute_AddLocalPerkCommand);

            CreateSurvivorCommandAsync = new RelayCommandAsync(Execute_CreateSurvivorCommandAsync, CanExecute_CreateSurvivorCommandAsync);

            UpdateSurvivorCommandAsync = new RelayCommandAsync<SurvivorViewModel>(Execute_UpdateSurvivorCommandAsync, CanExecute_UpdateSurvivorCommandAsync);

            DeleteLocalSurvivorCommand = new RelayCommand<SurvivorViewModel>(Execute_DeleteLocalSurvivorCommand, CanExecute_DeleteLocalSurvivorCommand);
            DeleteLocalPerkCommand = new RelayCommand<SurvivorPerkViewModel>(Execute_DeleteLocalPerkCommand, CanExecute_DeleteLocalPerkCommand);

            DeleteSurvivorCommandAsync = new RelayCommandAsync<SurvivorViewModel>(Execute_DeleteSurvivorCommandAsync, CanExecute_DeleteSurvivorCommandAsync);
            UpdateSurvivorPerkCommandAsync = new RelayCommandAsync<SurvivorPerkViewModel>(Execute_UpdateSurvivorPerkCommandAsync, CanExecute_UpdateSurvivorPerkCommandAsync);
            DeletePerkCommandAsync = new RelayCommandAsync<SurvivorPerkViewModel>(Execute_DeletePerkCommandAsync, CanExecute_DeletePerkCommandAsync);

            SelectSurvivorImageCommand = new RelayCommand<SurvivorViewModel>(Execute_SelectSurvivorImageCommand, CanExecute_SelectSurvivorImageCommand);
            SelectPerkImageCommand = new RelayCommand<SurvivorPerkViewModel>(Execute_SelectPerkImageCommand, CanExecute_SelectPerkImageCommand);

            ManagementSurvivorAddCommand = new RelayCommand<Visibility>(Execute_ManagementSurvivorAddCommand, CanExecute_ManagementSurvivorAddCommand);
            ManagementSurvivorEditCommand = new RelayCommand<Visibility>(Execute_ManagementSurvivorEditCommand, CanExecute_ManagementSurvivorEditCommand);
            ManagementLocalSurvivorEditCommand = new RelayCommand<Visibility>(Execute_ManagementLocalSurvivorEditCommand, CanExecute_ManagementLocalSurvivorEditCommand);

            ManagementPerkAddCommand = new RelayCommand<Visibility>(Execute_ManagementPerkAddCommand, CanExecute_ManagementPerkAddCommand);
            ManagementPerkEditCommand = new RelayCommand<Visibility>(Execute_ManagementPerkEditCommand, CanExecute_ManagementPerkEditCommand);
            ManagementLocalPerkEditCommand = new RelayCommand<Visibility>(Execute_ManagementLocalPerkEditCommand, CanExecute_ManagementLocalPerkEditCommand);

            RevertSurvivorChangeCommand = new RelayCommand<SurvivorViewModel>(Execute_RevertSurvivorChangeCommand, CanExecute_RevertSurvivorChangeCommand);
            RevertPerkChangeCommand = new RelayCommand<SurvivorPerkViewModel>(Execute_RevertPerkChangeCommand, CanExecute_RevertPerkChangeCommand);
        }

        /*--Выжившие--*/

        #region Команда [AddLocalSurvivorCommand]: Добавляет выжившего локально

        public RelayCommand? AddLocalSurvivorCommand { get; private set; }

        private void Execute_AddLocalSurvivorCommand()
        {
            if (NewSurvivor is null)
                return;

            NewSurvivor.SetLocalID(Guid.NewGuid());
            LocalSurvivors.Add(NewSurvivor);
            ManagementSurvivorAddCommand?.Execute(Visibility.Collapsed);
            PrepareSurvivorNew();
        }

        private bool CanExecute_AddLocalSurvivorCommand()
        {
            return true;
        }

        #endregion

        #region Команда [CreateSurvivorCommandAsync]: Добавлят полную запись об выжившем

        public RelayCommandAsync? CreateSurvivorCommandAsync { get; private set; }

        private async Task Execute_CreateSurvivorCommandAsync()
        {
            IsBusy = true;
            IsProcessAddedSurvivor = true;

            try
            {
                var newSurvivorVM = LocalSurvivors.FirstOrDefault();

                if (newSurvivorVM is null)
                {
                    MessageBox.Show("Вы не добавили предмет");
                    return;
                }

                var result = await _survivorService.AddAsync(
                    new ClientAddSurvivorRequest(
                        newSurvivorVM.OldId, newSurvivorVM.Name, newSurvivorVM.ImagePath,
                        [.. LocalPerks.Select(a => new ClientSurvivorPerkData(a.OldId, a.Name, a.ImagePath, a.Category?.Id))]));

                if (!result.IsSuccess)
                {
                    MessageBox.Show($"Не удалось добавить запись: {result.StringMessage}");
                    return;
                }

                var addedSurvivor = new SurvivorViewModel(new SurvivorSoloResponse(result.Value.Id, result.Value.OldId, result.Value.Name, result.Value.ImageKey));
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.SurvivorPortraits}/{addedSurvivor.ImageKey}");

                addedSurvivor.SetImage(await ImageHelper.ImageFromUrlAsync(url), string.Empty);

                Survivors.Add(addedSurvivor);
                SelectedSurvivor = addedSurvivor;

                Perks.Clear();

                foreach (var perk in result.Value.SurvivorPerks)
                {
                    if (perk is null)
                        continue;

                    var addonVM = new SurvivorPerkViewModel(perk, PerkCategories);

                    Perks.Add(addonVM);
                }

                LocalSurvivors.Clear();
                LocalPerks.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании записи: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                IsProcessAddedSurvivor = false;
            }
        }

        private bool CanExecute_CreateSurvivorCommandAsync() => true;

        #endregion

        #region Команда [DeleteLocalSurvivorCommand]: Удаляет локального выжившего

        public RelayCommand<SurvivorViewModel>? DeleteLocalSurvivorCommand { get; private set; }

        private void Execute_DeleteLocalSurvivorCommand(SurvivorViewModel model)
        {
            LocalSurvivors.Remove(model);
            MenuLocalSurvivorPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_DeleteLocalSurvivorCommand(SurvivorViewModel model) => true;

        #endregion


        #region Команда [SelectSurvivorImageCommand]: Выбор изображение выжившего

        public RelayCommand<SurvivorViewModel>? SelectSurvivorImageCommand { get; private set; }

        private void Execute_SelectSurvivorImageCommand(SurvivorViewModel model)
        {
            var path = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");
            model?.SetImage(ImageHelper.ImageFromFilePath(path!), path);
        }

        private bool CanExecute_SelectSurvivorImageCommand(SurvivorViewModel model) => true;

        #endregion


        #region Команда [DeleteSurvivorCommandAsync]: Удаление записи об выджившим и его сущьностях

        public RelayCommandAsync<SurvivorViewModel>? DeleteSurvivorCommandAsync { get; private set; }

        private async Task Execute_DeleteSurvivorCommandAsync(SurvivorViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _survivorService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    Survivors.Remove(model);
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

        private bool CanExecute_DeleteSurvivorCommandAsync(SurvivorViewModel model) => true;

        #endregion

        #region Команда [UpdateSurvivorCommandAsync]: Обновление записи Survivor

        public RelayCommandAsync<SurvivorViewModel>? UpdateSurvivorCommandAsync { get; private set; }

        private async Task Execute_UpdateSurvivorCommandAsync(SurvivorViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _survivorService.UpdateAsync(new ClientUpdateSurvivorRequest(model.Id, model.Name, model.ImagePath!, model.Name));

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

        private bool CanExecute_UpdateSurvivorCommandAsync(SurvivorViewModel model)
        {
            //if (model is not null)
            //    return model.HasChanges;

            //return false;

            return true;
        }

        #endregion

        #region Команда [RevertSurvivorChangeCommand]: Откат изменений у выжившего

        public RelayCommand<SurvivorViewModel>? RevertSurvivorChangeCommand { get; private set; }

        private void Execute_RevertSurvivorChangeCommand(SurvivorViewModel model)
        {
            model?.RevertChanges();
            MenuSurvivorPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_RevertSurvivorChangeCommand(SurvivorViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion 


        #region Команда [ManagementSurvivorAddCommand]: Управление модальным окном для добавления выжившего

        public RelayCommand<Visibility>? ManagementSurvivorAddCommand { get; private set; }

        private void Execute_ManagementSurvivorAddCommand(Visibility visibility) => SurvivorAddModalVisibility = visibility;

        private bool CanExecute_ManagementSurvivorAddCommand(Visibility visibility) => true;

        #endregion

        #region Команда [ManagementSurvivorEditCommand]: Управление модальным окном для редактирования выжившего

        public RelayCommand<Visibility>? ManagementSurvivorEditCommand { get; private set; }

        private void Execute_ManagementSurvivorEditCommand(Visibility visibility)
        {
            SurvivorEditModalVisibility = visibility;
            MenuSurvivorPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_ManagementSurvivorEditCommand(Visibility visibility) => true;

        #endregion

        #region Команда [ManagementLocalSurvivorEditCommand]: Управление модальным окном для редактирования выжившего

        public RelayCommand<Visibility>? ManagementLocalSurvivorEditCommand { get; private set; }

        private void Execute_ManagementLocalSurvivorEditCommand(Visibility visibility)
        {
            LocalSurvivorEditModalVisibility = visibility;
            MenuLocalSurvivorPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_ManagementLocalSurvivorEditCommand(Visibility visibility) => true;

        #endregion

        /*--Перки--*/

        #region Команда [AddLocalPerkCommand]: Добавляет перк локально

        public RelayCommand? AddLocalPerkCommand { get; private set; }

        private void Execute_AddLocalPerkCommand()
        {
            if (NewPerk is null)
                return;

            if (SelectedLocalSurvivor is null)
            {
                MessageBox.Show("Вы не выбрали локального выжившего, к которому добавляете перк");
                return;
            }

            NewPerk.SetSurvivorLocalId(SelectedLocalSurvivor.LocalId);
            LocalPerks.Add(NewPerk);
            ManagementPerkAddCommand?.Execute(Visibility.Collapsed);
            PreparePerkNew();
        }

        private bool CanExecute_AddLocalPerkCommand()
        {
            return true;
        }

        #endregion

        #region Команда [DeleteLocalPerkCommand]: Удаляет локальный перк

        public RelayCommand<SurvivorPerkViewModel>? DeleteLocalPerkCommand { get; private set; }

        private void Execute_DeleteLocalPerkCommand(SurvivorPerkViewModel model)
        {
            LocalPerks.Remove(model);
            MenuLocalPerkPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_DeleteLocalPerkCommand(SurvivorPerkViewModel model) => true;

        #endregion


        #region Команда [SelectPerkImageCommand]: Выбор изображение перка

        public RelayCommand<SurvivorPerkViewModel>? SelectPerkImageCommand { get; private set; }

        private void Execute_SelectPerkImageCommand(SurvivorPerkViewModel model)
        {
            var path = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");
            model?.SetImage(ImageHelper.ImageFromFilePath(path!), path);
        }

        private bool CanExecute_SelectPerkImageCommand(SurvivorPerkViewModel model) => true;

        #endregion


        #region Команда [DeletePerkCommandAsync]: Удаление перка

        public RelayCommandAsync<SurvivorPerkViewModel>? DeletePerkCommandAsync { get; private set; }

        private async Task Execute_DeletePerkCommandAsync(SurvivorPerkViewModel model)
        {
            if (model is null)
                return;

            if (SelectedSurvivor is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var service = _survivorPerkApiServiceFactory.Create(SelectedSurvivor.Id);

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

        private bool CanExecute_DeletePerkCommandAsync(SurvivorPerkViewModel model) => true;

        #endregion

        #region Команда [UpdateSurvivorPerkCommandAsync]: Обновление записи Survivor

        public RelayCommandAsync<SurvivorPerkViewModel>? UpdateSurvivorPerkCommandAsync { get; private set; }

        private async Task Execute_UpdateSurvivorPerkCommandAsync(SurvivorPerkViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var service = _survivorPerkApiServiceFactory.Create(SelectedSurvivor!.Id);

                var result = await service.UpdateAsync(new ClientUpdateSurvivorPerkRequest(model.Id, model.Name, model.ImagePath!, model.Name));

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

        private bool CanExecute_UpdateSurvivorPerkCommandAsync(SurvivorPerkViewModel model)
        {
            //if (model is not null)
            //    return model.HasChanges;

            //return false;

            return true;
        }

        #endregion

        #region Команда [RevertPerkChangeCommand]: Откат изменений у перка

        public RelayCommand<SurvivorPerkViewModel>? RevertPerkChangeCommand { get; private set; }

        private void Execute_RevertPerkChangeCommand(SurvivorPerkViewModel model)
        {
            model?.RevertChanges();
            MenuPerkPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_RevertPerkChangeCommand(SurvivorPerkViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

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

        #region Методы [GetAll]: Получение Survivors, Perks, PerkCategories

        private async Task GetAllSurvivorsAsync()
        {
            var survivors = await _survivorService.GetAllAsync();

            foreach (var survivor in survivors)
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.SurvivorPortraits}/{survivor.ImageKey}");

                var survivorVm = new SurvivorViewModel(survivor);
                survivorVm.SetImage(await ImageHelper.ImageFromUrlAsync(url), string.Empty);

                Survivors.Add(survivorVm);
            }
                
            SelectedSurvivor = Survivors.FirstOrDefault();
        }

        private async Task GetPerksForSurvivor()
        {
            if (SelectedSurvivor is null)
            {
                Perks.Clear();
                return;
            }

            var service = _survivorPerkApiServiceFactory.Create(SelectedSurvivor!.Id);

            var perks = await service.GetAllAsync();

            Perks.Clear();

            foreach (var perk in perks)
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.SurvivorPerks(SelectedSurvivor.Name)}/{perk.ImageKey}");
                var perkVM = new SurvivorPerkViewModel(perk, PerkCategories);

                perkVM.SetImage(await ImageHelper.ImageFromUrlAsync(url), string.Empty);
                perkVM.SetCategory(perk.CategoryId);

                Perks.Add(perkVM);
            }
        }

        private async Task GetAllPerkCategoriesAsync()
        {
            var categories = await _survivorPerkCategoryService.GetAllAsync();

            foreach (var category in categories)
                PerkCategories.Add(category);
        }

        #endregion

        #region Методы [PrepareNew]: Создание пустого SurvivorViewModel, SurvivorPerkViewModel для будущего добавление

        private void PrepareSurvivorNew() => NewSurvivor = new SurvivorViewModel(SurvivorSoloResponse.Empty);
        private void PreparePerkNew() => NewPerk = new SurvivorPerkViewModel(SurvivorPerkResponse.Empty, PerkCategories);

        #endregion
    }
}