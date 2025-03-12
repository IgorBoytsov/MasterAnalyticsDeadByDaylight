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
    internal class InteractionTypeDeathVM : BaseVM, IUpdatable
    {
        private readonly ITypeDeathService _typeDeathService;
        private readonly IWindowNavigationService _windowNavigationService;

        public InteractionTypeDeathVM(IWindowNavigationService windowNavigationService, ITypeDeathService typeDeathService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _typeDeathService = typeDeathService;

            GetTypeDeaths();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {

        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<TypeDeathDTO> TypeDeaths { get; private set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Типи смертей персонажей";

        #endregion

        #region Свойства : Selected | TypeDeathName | TypeDeathDescription

        private TypeDeathDTO _selectedTypeDeath;
        public TypeDeathDTO SelectedTypeDeath
        {
            get => _selectedTypeDeath;
            set
            {
                if (_selectedTypeDeath != value)
                {
                    _selectedTypeDeath = value;

                    TypeDeathName = value?.TypeDeathName;
                    TypeDeathDescription = value?.TypeDeathDescription;

                    OnPropertyChanged();
                }
            }
        }

        private string _typeDeathName;
        public string TypeDeathName
        {
            get => _typeDeathName;
            set
            {
                _typeDeathName = value;
                OnPropertyChanged();
            }
        }

        private string _typeDeathDescription;
        public string TypeDeathDescription
        {
            get => _typeDeathDescription;
            set
            {
                _typeDeathDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _addTypeDeathCommand;
        public RelayCommand AddTypeDeathCommand { get => _addTypeDeathCommand ??= new(obj => { AddTypeDeath(); }); }

        private RelayCommand _deleteTypeDeathCommand;
        public RelayCommand DeleteTypeDeathCommand { get => _deleteTypeDeathCommand ??= new(obj => { DeleteTypeDeath(); }); }

        private RelayCommand _updateTypeDeathCommand;
        public RelayCommand UpdateTypeDeathCommand { get => _updateTypeDeathCommand ??= new(obj => { UpdateTypeDeath(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        private async void GetTypeDeaths()
        {
            var typeDeaths = await _typeDeathService.GetAllAsync();

            foreach (var typeDeath in typeDeaths)
                TypeDeaths.Add(typeDeath);
        }

        #region CRUD

        // TODO : Изменить MessageBox на кастомное окно
        private async void AddTypeDeath()
        {
            var (TypeDeath, Message) = await _typeDeathService.CreateAsync(TypeDeathName, TypeDeathDescription);

            if (Message != string.Empty)
            {
                MessageBox.Show(Message);
                return;
            }
            else
            {
                NotificationTransmittingValue(WindowName.AddMatch, TypeDeath, TypeParameter.AddAndNotification);
                TypeDeaths.Add(TypeDeath);
            }  
        }

        private async void UpdateTypeDeath()
        {
            if (SelectedTypeDeath == null)
                return;

            var (TypeDeathDTO, Message) = await _typeDeathService.UpdateAsync(SelectedTypeDeath.IdTypeDeath, TypeDeathName, TypeDeathDescription);

            if (Message == string.Empty)
            {
                NotificationTransmittingValue(WindowName.AddMatch, TypeDeathDTO, TypeParameter.UpdateAndNotification);
                TypeDeaths.ReplaceItem(SelectedTypeDeath, TypeDeathDTO);
            }
            else
            {
                if (MessageBox.Show(Message + "Вы точно хотите произвести обновление?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var forcedTypeDeathDTO = await _typeDeathService.ForcedUpdateAsync(SelectedTypeDeath.IdTypeDeath, TypeDeathName, TypeDeathDescription);
                    NotificationTransmittingValue(WindowName.AddMatch, forcedTypeDeathDTO, TypeParameter.UpdateAndNotification);
                    TypeDeaths.ReplaceItem(SelectedTypeDeath, forcedTypeDeathDTO);
                }
            }
        }

        private async void DeleteTypeDeath()
        {
            if (SelectedTypeDeath == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _typeDeathService.DeleteAsync(SelectedTypeDeath.IdTypeDeath);

                if (IsDeleted == true)
                {
                    NotificationTransmittingValue(WindowName.AddMatch, SelectedTypeDeath, TypeParameter.DeleteAndNotification);
                    TypeDeaths.Remove(SelectedTypeDeath);
                }
                else
                    MessageBox.Show(Message);
            }
        }

        #endregion
    }
}