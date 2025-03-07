using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace DBDAnalytics.WPF.ViewModels.WindowVM
{
    internal class InteractionGameEventVM : BaseVM, IUpdatable
    {
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IGameEventService _gameEventService;

        public InteractionGameEventVM(IWindowNavigationService windowNavigationService, IGameEventService gameEventService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _gameEventService = gameEventService;

            GetGameEvent();
        }
        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {

        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<GameEventDTO> GameEvents { get; private set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Игровые события";

        #endregion

        #region Свойства : Selected | GameEventName | GameEventDescription

        private GameEventDTO _selectedGameEvent;
        public GameEventDTO SelectedGameEvent
        {
            get => _selectedGameEvent;
            set
            {
                if (_selectedGameEvent != value)
                {
                    _selectedGameEvent = value;

                    GameEventName = value?.GameEventName;
                    GameEventDescription = value?.GameEventDescription;

                    OnPropertyChanged();
                }
            }
        }

        private string _gameEventName;
        public string GameEventName
        {
            get => _gameEventName;
            set
            {
                _gameEventName = value;
                OnPropertyChanged();
            }
        }

        private string _gameEventDescription;
        public string GameEventDescription
        {
            get => _gameEventDescription;
            set
            {
                _gameEventDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _addGameEventCommand;
        public RelayCommand AddGameEventCommand { get => _addGameEventCommand ??= new(obj => { AddGameEvent(); });}

        private RelayCommand _deleteGameEventCommand;
        public RelayCommand DeleteGameEventCommand { get => _deleteGameEventCommand ??= new(obj => { DeleteGameEvent(); }); }
        
        private RelayCommand _updateGameEventCommand;
        public RelayCommand UpdateGameEventCommand { get => _updateGameEventCommand ??= new(obj => { UpdateGameEvent(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/
        
        private async void GetGameEvent()
        {
            var gameEvents = await _gameEventService.GetAllAsync();

            foreach (var gameEvent in gameEvents)
                GameEvents.Add(gameEvent);
        
        }

        #region CRUD

        // TODO : Изменить MessageBox на кастомное окно
        private async void AddGameEvent()
        {
            var newEventDTO = await _gameEventService.CreateAsync(GameEventName, GameEventDescription);

            if (newEventDTO.Message != string.Empty)
            {
                MessageBox.Show(newEventDTO.Message);
                return;
            }
            else
                GameEvents.Add(newEventDTO.GameEventDTO);
        }

        private async void DeleteGameEvent()
        {
            if (SelectedGameEvent == null) 
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _gameEventService.DeleteAsync(SelectedGameEvent.IdGameEvent);

                if (IsDeleted == true)
                    GameEvents.Remove(SelectedGameEvent);
                else
                    MessageBox.Show(Message);
            }
        } 
        
        private async void UpdateGameEvent()
        {
            if (SelectedGameEvent == null) 
                return;

            var (GameEventDTO, Message) = await _gameEventService.UpdateAsync(SelectedGameEvent.IdGameEvent, GameEventName, GameEventDescription);

            if (Message == string.Empty)
                GameEvents.ReplaceItem(SelectedGameEvent, GameEventDTO);
            else
            {
                if (MessageBox.Show(Message + "Вы точно хотите обновить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var forcedGameEventDTO = await _gameEventService.ForcedUpdateAsync(SelectedGameEvent.IdGameEvent, GameEventName, GameEventDescription);
                    GameEvents.ReplaceItem(SelectedGameEvent, forcedGameEventDTO);
                }
            }
        }

        #endregion
    }
}