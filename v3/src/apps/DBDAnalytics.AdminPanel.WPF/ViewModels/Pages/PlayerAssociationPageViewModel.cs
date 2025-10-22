using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Associations;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Update;
using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Pages
{
    internal sealed class PlayerAssociationPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IPlayerAssociationService _playerAssociationService;

        public PlayerAssociationPageViewModel(IPlayerAssociationService playerAssociationService)
        {
            _playerAssociationService = playerAssociationService;

            PrepareNew();
            InitializeCommand();
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsBusy = true;

            try
            {
                await GetAll();

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

        public ObservableCollection<PlayerAssociationViewModel> PlayerAssociations { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        #region Свойство: [SelectedPlayerAssociation] Метод: [OnSelectedPlayerAssociationPropertyChanged]

        private PlayerAssociationViewModel? _selectedPlayerAssociation;
        public PlayerAssociationViewModel? SelectedPlayerAssociation
        {
            get => _selectedPlayerAssociation;
            set
            {
                if (_selectedPlayerAssociation is not null)
                    _selectedPlayerAssociation.PropertyChanged -= OnSelectedPlayerAssociationPropertyChanged;

                SetProperty(ref _selectedPlayerAssociation, value);

                if (_selectedPlayerAssociation is not null)
                    _selectedPlayerAssociation.PropertyChanged += OnSelectedPlayerAssociationPropertyChanged;

                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnSelectedPlayerAssociationPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewPlayerAssociation] Метод: [OnNewPlayerAssociationPropertyChanged] 

        private PlayerAssociationViewModel? _newPlayerAssociation;
        public PlayerAssociationViewModel? NewPlayerAssociation
        {
            get => _newPlayerAssociation;
            private set
            {
                if (_newPlayerAssociation is not null)
                    _newPlayerAssociation.PropertyChanged -= OnNewPlayerAssociationPropertyChanged;

                SetProperty(ref _newPlayerAssociation, value);

                if (_newPlayerAssociation is not null)
                    _newPlayerAssociation.PropertyChanged += OnNewPlayerAssociationPropertyChanged;

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnNewPlayerAssociationPropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<PlayerAssociationViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommand);
            DeleteCommandAsync = new RelayCommandAsync<PlayerAssociationViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommand);
            RevertChangeCommand = new RelayCommand<PlayerAssociationViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);
        }

        #region Команда [CreateCommandAsync]: Создание PlayerAssociation

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewPlayerAssociation is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewPlayerAssociation.ToModel();

                var result = await _playerAssociationService.AddAsync(newModel);

                if (!result.IsSuccess)
                {
                    MessageBox.Show("Не удалось добавить запись");
                    return;
                }

                PlayerAssociations.Add(new PlayerAssociationViewModel(result.Value!));

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

        private bool CanExecute_CreateCommandAsync() => NewPlayerAssociation != null && !string.IsNullOrWhiteSpace(NewPlayerAssociation.Name) && NewPlayerAssociation.OldId >= 0;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление PlayerAssociation

        public RelayCommandAsync<PlayerAssociationViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(PlayerAssociationViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _playerAssociationService.UpdateAsync(model.Id, new UpdatePlayerAssociationRequest(model.Name));

                if (!result.IsSuccess)
                {
                    MessageBox.Show($"Ошибка обновления: {result.StringMessage}");
                    return;
                }

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

        private bool CanExecute_UpdateCommand(PlayerAssociationViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление PlayerAssociation

        public RelayCommandAsync<PlayerAssociationViewModel>? DeleteCommandAsync { get; private set; }

        private async Task Execute_DeleteCommandAsync(PlayerAssociationViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _playerAssociationService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    PlayerAssociations.Remove(model);
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

        private bool CanExecute_DeleteCommand(PlayerAssociationViewModel model) => model is not null;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<PlayerAssociationViewModel>? RevertChangeCommand { get; private set; }

        private void Execute_RevertChangeCommand(PlayerAssociationViewModel model) => model?.RevertChanges();

        private bool CanExecute_RevertChangeCommand(PlayerAssociationViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [PrepareNew]: Создание пустого PlayerAssociationViewModel для будущего добавление

        private void PrepareNew() => NewPlayerAssociation = new PlayerAssociationViewModel(PlayerAssociationResponse.Empty);

        #endregion

        #region Метод [GetAll]: Получение PlayerAssociations

        public async Task GetAll()
        {
            var playerAssociations = await _playerAssociationService.GetAllAsync();

            foreach (var playerAssociation in playerAssociations)
                PlayerAssociations.Add(new PlayerAssociationViewModel(playerAssociation));
        }

        #endregion
    }
}