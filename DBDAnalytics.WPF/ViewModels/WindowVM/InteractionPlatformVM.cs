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
    internal class InteractionPlatformVM : BaseVM, IUpdatable
    {
        private readonly IPlatformService _platformService;
        private readonly IWindowNavigationService _windowNavigationService;

        public InteractionPlatformVM(IWindowNavigationService windowNavigationService, IPlatformService platformService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _platformService = platformService;

            GetPlatforms();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {

        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<PlatformDTO> Platforms { get; private set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Игровые платформы";

        #endregion

        #region Свойства : Selected | PlatformName | PlatformDescription

        private PlatformDTO _selectedPlatform;
        public PlatformDTO SelectedPlatform
        {
            get => _selectedPlatform;
            set
            {
                if (_selectedPlatform != value)
                {
                    _selectedPlatform = value;

                    PlatformName = value?.PlatformName;
                    PlatformDescription = value?.PlatformDescription;

                    OnPropertyChanged();
                }
            }
        }

        private string _platformName;
        public string PlatformName
        {
            get => _platformName;
            set
            {
                _platformName = value;
                OnPropertyChanged();
            }
        }

        private string _platformDescription;
        public string PlatformDescription
        {
            get => _platformDescription;
            set
            {
                _platformDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _addPlatformCommand;
        public RelayCommand AddPlatformCommand { get => _addPlatformCommand ??= new(obj => { AddPlatform(); }); }

        private RelayCommand _deletePlatformCommand;
        public RelayCommand DeletePlatformCommand { get => _deletePlatformCommand ??= new(obj => { DeletePlatform(); }); }

        private RelayCommand _updatePlatformCommand;
        public RelayCommand UpdatePlatformCommand { get => _updatePlatformCommand ??= new(obj => { UpdatePlatform(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region CRUD

        private async void GetPlatforms()
        {
            var platforms = await _platformService.GetAllAsync();

            foreach (var platform in platforms)
                Platforms.Add(platform);
        }

        // TODO : Изменить MessageBox на кастомное окно
        private async void AddPlatform()
        {
            var newPlatformDTO = await _platformService.CreateAsync(PlatformName, PlatformDescription);

            if (newPlatformDTO.Message != string.Empty)
            {
                MessageBox.Show(newPlatformDTO.Message);
                return;
            }
            else
                Platforms.Add(newPlatformDTO.PlatformDTO);
        }

        private async void DeletePlatform()
        {
            if (SelectedPlatform == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _platformService.DeleteAsync(SelectedPlatform.IdPlatform);

                if (IsDeleted == true)
                    Platforms.Remove(SelectedPlatform);
                else
                    MessageBox.Show(Message);
            }   
        }

        private async void UpdatePlatform()
        {
            if (SelectedPlatform == null)
                return;

            var (PlatformDTO, Message) = await _platformService.UpdateAsync(SelectedPlatform.IdPlatform, PlatformName, PlatformDescription);

            if (Message == string.Empty)
                Platforms.ReplaceItem(SelectedPlatform, PlatformDTO);
            else
            {
                if (MessageBox.Show(Message + "Вы точно хотите произвести обновление?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var forcedPlatformDTO = await _platformService.ForcedUpdateAsync(SelectedPlatform.IdPlatform, PlatformName, PlatformDescription);
                    Platforms.ReplaceItem(SelectedPlatform, forcedPlatformDTO);
                }
            }
        }

        #endregion
    }
}
