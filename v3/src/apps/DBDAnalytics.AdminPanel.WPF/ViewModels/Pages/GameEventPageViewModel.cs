using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Matches.GameEvent;
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
    internal sealed class GameEventPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IGameEventService _gameEventService;

        public GameEventPageViewModel(IGameEventService gameEventService)
        {
            _gameEventService = gameEventService;

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

        public ObservableCollection<GameEventViewModel> GameEvents { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        #region Свойство: [SelectedGameEvent] Метод: [OnSelectedGameEventPropertyChanged]

        private GameEventViewModel? _selectedGameEvent;
        public GameEventViewModel? SelectedGameEvent
        {
            get => _selectedGameEvent;
            set
            {
                if (_selectedGameEvent is not null)
                    _selectedGameEvent.PropertyChanged -= OnSelectedGameEventPropertyChanged;

                SetProperty(ref _selectedGameEvent, value);

                if (_selectedGameEvent is not null)
                    _selectedGameEvent.PropertyChanged += OnSelectedGameEventPropertyChanged;

                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnSelectedGameEventPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewGameEvent] Метод: [OnNewGameEventPropertyChanged] 

        private GameEventViewModel? _newGameEvent;
        public GameEventViewModel? NewGameEvent
        {
            get => _newGameEvent;
            private set
            {
                if (_newGameEvent is not null)
                    _newGameEvent.PropertyChanged -= OnNewGameEventPropertyChanged;

                SetProperty(ref _newGameEvent, value);

                if (_newGameEvent is not null)
                    _newGameEvent.PropertyChanged += OnNewGameEventPropertyChanged;

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnNewGameEventPropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<GameEventViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommand);
            DeleteCommandAsync = new RelayCommandAsync<GameEventViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommand);
            RevertChangeCommand = new RelayCommand<GameEventViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);
        }

        #region Команда [CreateCommandAsync]: Создание GameEvent

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewGameEvent is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewGameEvent.ToModel();

                var response = await _gameEventService.AddAsync(newModel);

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.StringMessage);
                    return;
                }

                GameEvents.Add(new GameEventViewModel(response.Value!));

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

        private bool CanExecute_CreateCommandAsync() => NewGameEvent != null && !string.IsNullOrWhiteSpace(NewGameEvent.Name) && NewGameEvent.OldId >= 0;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление GameEvent

        public RelayCommandAsync<GameEventViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(GameEventViewModel model)
        {
            if(model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _gameEventService.UpdateAsync(model.Id, new UpdateGameEventRequest(model.Name));

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

        private bool CanExecute_UpdateCommand(GameEventViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление GameEvent

        public RelayCommandAsync<GameEventViewModel>? DeleteCommandAsync { get; private set; }
        
        private async Task Execute_DeleteCommandAsync(GameEventViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _gameEventService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    GameEvents.Remove(model);
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

        private bool CanExecute_DeleteCommand(GameEventViewModel model) => model is not null;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<GameEventViewModel>? RevertChangeCommand {get; private set; }

        private void Execute_RevertChangeCommand(GameEventViewModel model) => model?.RevertChanges();

        private bool CanExecute_RevertChangeCommand(GameEventViewModel model)
        {
            if (model is not null) 
                return model.HasChanges;

            return false;
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [PrepareNew]: Создание пустого GameEventViewModel для будущего добавление

        private void PrepareNew() => NewGameEvent = new GameEventViewModel(GameEventResponse.Empty);

        #endregion

        #region Метод [GetAll]: Получение GameEvents

        public async Task GetAll()
        {
            var gameEvents = await _gameEventService.GetAllAsync();

            foreach (var gameEvent in gameEvents)
                GameEvents.Add(new GameEventViewModel(gameEvent));
        }

        #endregion
    }
}