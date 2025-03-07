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
    internal class InteractionGameModeVM : BaseVM, IUpdatable
    {
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IGameModeService _gameModeService;

        public InteractionGameModeVM(IWindowNavigationService windowNavigationService, IGameModeService gameModeService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _gameModeService = gameModeService;

            GetGameMode();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            
        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<GameModeDTO> GameMods { get; private set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Игровые режимы";

        #endregion

        #region Свойства : Selected | GameModeName | GameModeDescription

        private GameModeDTO _selectedGameMode;
        public GameModeDTO SelectedGameMode
        {
            get => _selectedGameMode;
            set
            {
                if (_selectedGameMode != value)
                {
                    _selectedGameMode = value;

                    GameModeName = value?.GameModeName;
                    GameModeDescription = value?.GameModeDescription;

                    OnPropertyChanged();
                }
            }
        }

        private string _gameModeName;
        public string GameModeName
        {
            get => _gameModeName;
            set
            {
                _gameModeName = value;
                OnPropertyChanged();
            }
        }

        private string _gameModeDescription;
        public string GameModeDescription
        {
            get => _gameModeDescription;
            set
            {
                _gameModeDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _addGameModeCommand;
        public RelayCommand AddGameModeCommand { get => _addGameModeCommand ??= new(obj => { AddGameMode(); }); }

        private RelayCommand _deleteGameModeCommand;
        public RelayCommand DeleteGameModeCommand { get => _deleteGameModeCommand ??= new(obj => { DeleteGameMode(); }); }

        private RelayCommand _updateGameModeCommand;
        public RelayCommand UpdateGameModeCommand { get => _updateGameModeCommand ??= new(obj => { UpdateGameMode(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/
       
        private async void GetGameMode()
        {
            var gameModes = await _gameModeService.GetAllAsync();

            foreach (var gameMode in gameModes)
                GameMods.Add(gameMode);   
        }

        #region CRUD

        // TODO : Изменить MessageBox на кастомное окно
        private async void AddGameMode()
        {
            var newEventDTO = await _gameModeService.CreateAsync(GameModeName, GameModeDescription);

            if (newEventDTO.Message != string.Empty)
            {
                MessageBox.Show(newEventDTO.Message);
                return;
            }
            else
                GameMods.Add(newEventDTO.GameModeDTO);
        }

        private async void DeleteGameMode()
        {
            if (SelectedGameMode == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _gameModeService.DeleteAsync(SelectedGameMode.IdGameMode);

                if (IsDeleted == true)
                    GameMods.Remove(SelectedGameMode);
                else
                    MessageBox.Show(Message);
            } 
        }

        private async void UpdateGameMode()
        {
            if (SelectedGameMode == null)
                return;

            var (GameModeDTO, Message) = await _gameModeService.UpdateAsync(SelectedGameMode.IdGameMode, GameModeName, GameModeDescription);

            if (Message == string.Empty)
                GameMods.ReplaceItem(SelectedGameMode, GameModeDTO);
            else
            {
                if (MessageBox.Show(Message + "Вы точно хотите произвести обновление?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var forcedGameEventDTO = await _gameModeService.ForcedUpdateAsync(SelectedGameMode.IdGameMode, GameModeName, GameModeDescription);
                    GameMods.ReplaceItem(SelectedGameMode, forcedGameEventDTO);
                }
            }
        }

        #endregion
    }
}
