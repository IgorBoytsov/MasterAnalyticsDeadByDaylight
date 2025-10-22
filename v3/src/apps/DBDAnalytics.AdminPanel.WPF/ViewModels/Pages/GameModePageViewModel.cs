using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Matches.GameMode;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Update;
using DBDAnalytics.Shared.Contracts.Responses.Match;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Pages
{
    internal sealed class GameModePageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IGameModeService _gameModeService;

        public GameModePageViewModel(IGameModeService gameEventService)
        {
            _gameModeService = gameEventService;

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

        public ObservableCollection<GameModeViewModel> GameModes { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        #region Свойство: [SelectedGameMode] Метод: [OnSelectedGameModePropertyChanged]

        private GameModeViewModel? _selectedGameMode;
        public GameModeViewModel? SelectedGameMode
        {
            get => _selectedGameMode;
            set
            {
                if (_selectedGameMode is not null)
                    _selectedGameMode.PropertyChanged -= OnSelectedGameModePropertyChanged;

                SetProperty(ref _selectedGameMode, value);

                if (_selectedGameMode is not null)
                    _selectedGameMode.PropertyChanged += OnSelectedGameModePropertyChanged;

                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnSelectedGameModePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewGameMode] Метод: [OnNewGameModePropertyChanged] 

        private GameModeViewModel? _newGameMode;
        public GameModeViewModel? NewGameMode
        {
            get => _newGameMode;
            private set
            {
                if (_newGameMode is not null)
                    _newGameMode.PropertyChanged -= OnNewGameModePropertyChanged;

                SetProperty(ref _newGameMode, value);

                if (_newGameMode is not null)
                    _newGameMode.PropertyChanged += OnNewGameModePropertyChanged;

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnNewGameModePropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<GameModeViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommand);
            DeleteCommandAsync = new RelayCommandAsync<GameModeViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommand);
            RevertChangeCommand = new RelayCommand<GameModeViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);
        }

        #region Команда [CreateCommandAsync]: Создание GameMode

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewGameMode is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewGameMode.ToModel();

                var response = await _gameModeService.AddAsync(newModel);

                if (response is null)
                {
                    MessageBox.Show("Не удалось добавить запись");
                    return;
                }

                GameModes.Add(new GameModeViewModel(response.Value!));

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

        private bool CanExecute_CreateCommandAsync() => NewGameMode != null && !string.IsNullOrWhiteSpace(NewGameMode.Name) && NewGameMode.OldId >= 0;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление GameMode

        public RelayCommandAsync<GameModeViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(GameModeViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _gameModeService.UpdateAsync(model.Id, new UpdateGameModeRequest(model.Name));

                if (!result.IsSuccess)
                    MessageBox.Show($"Ошибка обновления: {result.StringMessage}");

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

        private bool CanExecute_UpdateCommand(GameModeViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление GameMode

        public RelayCommandAsync<GameModeViewModel>? DeleteCommandAsync { get; private set; }

        private async Task Execute_DeleteCommandAsync(GameModeViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _gameModeService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    GameModes.Remove(model);
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

        private bool CanExecute_DeleteCommand(GameModeViewModel model) => model is not null;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<GameModeViewModel>? RevertChangeCommand { get; private set; }

        private void Execute_RevertChangeCommand(GameModeViewModel model) => model?.RevertChanges();

        private bool CanExecute_RevertChangeCommand(GameModeViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [PrepareNew]: Создание пустого GameModeViewModel для будущего добавление

        private void PrepareNew() => NewGameMode = new GameModeViewModel(GameModeResponse.Empty);

        #endregion

        #region Метод [GetAll]: Получение GameModes

        public async Task GetAll()
        {
            var gameModes = await _gameModeService.GetAllAsync();

            foreach (var gameMode in gameModes)
                GameModes.Add(new GameModeViewModel(gameMode));
        }

        #endregion
    }
}