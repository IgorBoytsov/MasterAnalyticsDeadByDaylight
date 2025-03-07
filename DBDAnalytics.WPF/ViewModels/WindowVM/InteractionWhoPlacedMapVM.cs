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
    internal class InteractionWhoPlacedMapVM : BaseVM, IUpdatable
    {
        private readonly IWhoPlacedMapService _whoPlacedMapService;
        private readonly IWindowNavigationService _windowNavigationService;

        public InteractionWhoPlacedMapVM(IWindowNavigationService windowNavigationService, IWhoPlacedMapService whoPlacedMapService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _whoPlacedMapService = whoPlacedMapService;

            GetWhoPlacedMaps();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {

        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<WhoPlacedMapDTO> WhoPlacedMaps { get; private set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Чья карта была поставлена";

        #endregion

        #region Свойства : Selected | WhoPlacedMapnName 

        private WhoPlacedMapDTO _selectedWhoPlacedMap;
        public WhoPlacedMapDTO SelectedWhoPlacedMap
        {
            get => _selectedWhoPlacedMap;
            set
            {
                if (_selectedWhoPlacedMap != value)
                {
                    _selectedWhoPlacedMap = value;

                    WhoPlacedMapName = value?.WhoPlacedMapName;

                    OnPropertyChanged();
                }
            }
        }

        private string _whoPlacedMapName;
        public string WhoPlacedMapName
        {
            get => _whoPlacedMapName;
            set
            {
                _whoPlacedMapName = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _addWhoPlacedMapCommand;
        public RelayCommand AddWhoPlacedMapCommand { get => _addWhoPlacedMapCommand ??= new(obj => { AddWhoPlacedMap(); }); }

        private RelayCommand _deleteWhoPlacedMapCommand;
        public RelayCommand DeleteWhoPlacedMapCommand { get => _deleteWhoPlacedMapCommand ??= new(obj => { DeleteWhoPlacedMap(); }); }

        private RelayCommand _updateWhoPlacedMapCommand;
        public RelayCommand UpdateWhoPlacedMapCommand { get => _updateWhoPlacedMapCommand ??= new(obj => { UpdateWhoPlacedMap(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region CRUD

        private async void GetWhoPlacedMaps()
        {
            var whoPlacedMaps = await _whoPlacedMapService.GetAllAsync();

            foreach (var whoPlacedMap in whoPlacedMaps)
                WhoPlacedMaps.Add(whoPlacedMap);
        }

        // TODO : Изменить MessageBox на кастомное окно
        private async void AddWhoPlacedMap()
        {
            var newWhoPlacedMapDTO = await _whoPlacedMapService.CreateAsync(WhoPlacedMapName);

            if (newWhoPlacedMapDTO.Message != string.Empty)
            {
                MessageBox.Show(newWhoPlacedMapDTO.Message);
                return;
            }
            else
                WhoPlacedMaps.Add(newWhoPlacedMapDTO.WhoPlacedMapDTO);
        }

        private async void DeleteWhoPlacedMap()
        {
            if (SelectedWhoPlacedMap == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _whoPlacedMapService.DeleteAsync(SelectedWhoPlacedMap.IdWhoPlacedMap);

                if (IsDeleted == true)
                    WhoPlacedMaps.Remove(SelectedWhoPlacedMap);
                else
                    MessageBox.Show(Message);
            }
        }

        private async void UpdateWhoPlacedMap()
        {
            if (SelectedWhoPlacedMap == null)
                return;

            var (WhoPlacedMapDTO, Message) = await _whoPlacedMapService.UpdateAsync(SelectedWhoPlacedMap.IdWhoPlacedMap, WhoPlacedMapName);

            if (Message == string.Empty)
                WhoPlacedMaps.ReplaceItem(SelectedWhoPlacedMap, WhoPlacedMapDTO);
            else
                MessageBox.Show(Message);
        }

        #endregion
    }
}