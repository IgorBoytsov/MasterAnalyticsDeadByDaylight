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
    internal class InteractionAssociationVM : BaseVM, IUpdatable
    {
        private readonly IAssociationService _associationService;
        private readonly IWindowNavigationService _windowNavigationService;

        public InteractionAssociationVM(IWindowNavigationService windowNavigationService, IAssociationService associationService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _associationService = associationService;

            GetAssociations();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            
        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<PlayerAssociationDTO> PlayerAssociations { get; private set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Игровые ассоциации";

        #endregion

        #region Свойства : Selected | PlayerAssociationName | PlayerAssociationDescription

        private PlayerAssociationDTO _selectedPlayerAssociation;
        public PlayerAssociationDTO SelectedPlayerAssociation
        {
            get => _selectedPlayerAssociation;
            set
            {
                if (_selectedPlayerAssociation != value)
                {
                    _selectedPlayerAssociation = value;

                    PlayerAssociationName = value?.PlayerAssociationName;
                    PlayerAssociationDescription = value?.PlayerAssociationDescription;

                    OnPropertyChanged();
                }
            }
        }

        private string _playerAssociationName;
        public string PlayerAssociationName
        {
            get => _playerAssociationName;
            set
            {
                _playerAssociationName = value;
                OnPropertyChanged();
            }
        }

        private string _playerAssociationDescription;
        public string PlayerAssociationDescription
        {
            get => _playerAssociationDescription;
            set
            {
                _playerAssociationDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _addPlayerAssociationCommand;
        public RelayCommand AddPlayerAssociationCommand { get => _addPlayerAssociationCommand ??= new(obj => { AddPlayerAssociation(); }); }

        private RelayCommand _deletePlayerAssociationCommand;
        public RelayCommand DeletePlayerAssociationCommand { get => _deletePlayerAssociationCommand ??= new(obj => { DeletePlayerAssociation(); }); }

        private RelayCommand _updatePlayerAssociationCommand;
        public RelayCommand UpdatePlayerAssociationCommand { get => _updatePlayerAssociationCommand ??= new(obj => { UpdatePlayerAssociation(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        private async void GetAssociations()
        {
            var playerAssociations = await _associationService.GetAllAsync();

            foreach (var playerAssociation in playerAssociations)
                PlayerAssociations.Add(playerAssociation);
        }

        #region CRUD

        // TODO : Изменить MessageBox на кастомное окно
        private async void AddPlayerAssociation()
        {
            var newPlayerAssociationDTO = await _associationService.CreateAsync(PlayerAssociationName, PlayerAssociationDescription);

            if (newPlayerAssociationDTO.Message != string.Empty)
            {
                MessageBox.Show(newPlayerAssociationDTO.Message);
                return;
            }
            else
                PlayerAssociations.Add(newPlayerAssociationDTO.PlayerAssociationDTO);
        }

        private async void DeletePlayerAssociation()
        {
            if (SelectedPlayerAssociation == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _associationService.DeleteAsync(SelectedPlayerAssociation.IdPlayerAssociation);

                if (IsDeleted == true)
                    PlayerAssociations.Remove(SelectedPlayerAssociation);
                else
                    MessageBox.Show(Message);
            }
        }

        private async void UpdatePlayerAssociation()
        {
            if (SelectedPlayerAssociation == null)
                return;

            var (PlayerAssociationDTO, Message) = await _associationService.UpdateAsync(SelectedPlayerAssociation.IdPlayerAssociation, PlayerAssociationName, PlayerAssociationDescription);

            if (Message == string.Empty)
                PlayerAssociations.ReplaceItem(SelectedPlayerAssociation, PlayerAssociationDTO);
            else
            {
                if (MessageBox.Show(Message + "Вы точно хотите произвести обновление?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var forcedPlayerAssociationDTO = await _associationService.ForcedUpdateAsync(SelectedPlayerAssociation.IdPlayerAssociation, PlayerAssociationName, PlayerAssociationDescription);
                    PlayerAssociations.ReplaceItem(SelectedPlayerAssociation, forcedPlayerAssociationDTO);
                }
            }
        }

        #endregion
    }
}