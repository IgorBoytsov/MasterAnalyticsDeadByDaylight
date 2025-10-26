using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map;
using DBDAnalytics.CatalogService.Client.ApiClients.Matches.Measurement;
using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.FIleStorageService.Client;
using DBDAnalytics.Shared.Contracts.Constants;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Update;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using Shared.WPF.Commands;
using Shared.WPF.Helpers;
using Shared.WPF.Services;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Pages
{
    internal sealed class MeasurementPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IMeasurementService _measurementService ;
        private readonly IMeasurementMapApiServiceFactory _mapApiServiceFactory ;
        private readonly IFileDialogService _fileDialogService;
        private readonly IFileStorageService _fileStorageService;

        public MeasurementPageViewModel(
            IMeasurementService measurementService,
            IMeasurementMapApiServiceFactory mapApiServiceFactory,
            IFileDialogService fileDialogService,
            IFileStorageService fileStorageService)
        {
            _measurementService = measurementService;
            _mapApiServiceFactory = mapApiServiceFactory;
            _fileDialogService = fileDialogService;
            _fileStorageService = fileStorageService;

            PrepareNewMeasurement();
            PrepareNewMap();

            LocalMapsView = CollectionViewSource.GetDefaultView(_localMaps);
            LocalMapsView.Filter = FilterLocalMaps;

            InitializeCommand();
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsBusy = true;

            try
            {
                await GetAllMeasurements();

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

        public ObservableCollection<MeasurementViewModel> Measurements { get; private set; } = [];
        public ObservableCollection<MapViewModel> Maps { get; private set; } = [];

        public ObservableCollection<MeasurementViewModel> LocalMeasurements { get; private set; } = [];
        private ObservableCollection<MapViewModel> _localMaps { get; set; } = [];

        public ICollectionView LocalMapsView { get; private set; } = null!;

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController MenuMapPopup { get; } = new PopupController();
        public IPopupController EditMapPopup { get; } = new PopupController();

        public IPopupController MenuLocalMapPopup { get; } = new PopupController();
        public IPopupController EditLocalMapPopup { get; } = new PopupController();

        public IPopupController MenuMeasurementPopup { get; } = new PopupController();
        public IPopupController EditLocalMeasurementPopup { get; } = new PopupController();

        public IPopupController EditMeasurementPopup { get; } = new PopupController();

        /*--Измерения--*/

        #region Свойство [SelectedMeasurement] Метод: [OnSelectedMeasurementPropertyChanged]

        private MeasurementViewModel? _selectedMeasurement;
        public MeasurementViewModel? SelectedMeasurement
        {
            get => _selectedMeasurement;
            set
            {
                if (_selectedMeasurement is not null)
                    _selectedMeasurement.PropertyChanged -= OnSelectedMeasurementPropertyChanged;

                SetProperty(ref _selectedMeasurement, value);

                _ = GetMapsForMeasurement();

                if (_selectedMeasurement is not null)
                    _selectedMeasurement.PropertyChanged += OnSelectedMeasurementPropertyChanged;
            }
        }

        private void OnSelectedMeasurementPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateMeasurementCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeMeasurementCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство [SelectedLocalMeasurement] Метод: [OnSelectedLocalMeasurementPropertyChanged]

        private MeasurementViewModel? _selectedLocalMeasurement;
        public MeasurementViewModel? SelectedLocalMeasurement
        {
            get => _selectedLocalMeasurement;
            set
            {
                if (_selectedLocalMeasurement is not null)
                    _selectedLocalMeasurement.PropertyChanged -= OnSelectedLocalMeasurementPropertyChanged;

                SetProperty(ref _selectedLocalMeasurement, value);

                LocalMapsView.Refresh();

                if (_selectedLocalMeasurement is not null)
                    _selectedLocalMeasurement.PropertyChanged += OnSelectedLocalMeasurementPropertyChanged;
            }
        }

        private void OnSelectedLocalMeasurementPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            CreateLocalMapCommand?.RaiseCanExecuteChanged();
            SaveDataCommandAsync?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство [NewMeasurement] Метод: [OnNewMeasurementPropertyChanged]

        private MeasurementViewModel? _newMeasurement;
        public MeasurementViewModel? NewMeasurement
        {
            get => _newMeasurement;
            set
            {
                if (_newMeasurement is not null)
                    _newMeasurement.PropertyChanged -= OnNewMeasurementPropertyChanged;

                SetProperty(ref _newMeasurement, value);

                _ = GetMapsForMeasurement();

                if (_newMeasurement is not null)
                    _newMeasurement.PropertyChanged += OnNewMeasurementPropertyChanged;
            }
        }

        private void OnNewMeasurementPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            CreateLocalMeasurementCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        /*--Карты--*/

        #region Свойство [SelectedMap] Метод: [OnSelectedMapPropertyChanged]

        private MapViewModel? _selectedMap;
        public MapViewModel? SelectedMap
        {
            get => _selectedMap;
            set
            {
                if (_selectedMap is not null)
                    _selectedMap.PropertyChanged -= OnSelectedMapPropertyChanged;

                SetProperty(ref _selectedMap, value);

                if (_selectedMap is not null)
                    _selectedMap.PropertyChanged += OnSelectedMapPropertyChanged;
            }
        }

        private void OnSelectedMapPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RevertMapChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство [SelectedLocalMap] Метод: [OnSelectedLocalMapPropertyChanged]

        private MapViewModel? _selectedLocalMap;
        public MapViewModel? SelectedLocalMap
        {
            get => _selectedLocalMap;
            set
            {
                if (_selectedLocalMap is not null)
                    _selectedLocalMap.PropertyChanged -= OnSelectedLocalMapPropertyChanged;

                SetProperty(ref _selectedLocalMap, value);

                if (_selectedLocalMap is not null)
                    _selectedLocalMap.PropertyChanged += OnSelectedLocalMapPropertyChanged;
            }
        }

        private void OnSelectedLocalMapPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Свойство [NewMap] Метод: [OnNewMapPropertyChanged]

        private MapViewModel? _newMap;
        public MapViewModel? NewMap
        {
            get => _newMap;
            set
            {
                if (_newMap is not null)
                    _newMap.PropertyChanged -= OnNewMapPropertyChanged;

                SetProperty(ref _newMap, value);

                if (_newMap is not null)
                    _newMap.PropertyChanged += OnNewMapPropertyChanged;
            }
        }

        private void OnNewMapPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            CreateLocalMapCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        /*--TabControl Index--*/

        #region Свойство [SelectedTabControlIndexMeasurement]: Отслеживает какую вкладку выбрали у измерений

        private int _selectedTabControlIndexMeasurement;
        public int SelectedTabControlIndexMeasurement
        {
            get => _selectedTabControlIndexMeasurement;
            set
            {
                SetProperty(ref _selectedTabControlIndexMeasurement, value);
                SelectedTabControlIndexMap = value;
            }
        }

        #endregion

        #region Свойство [SelectedTabControlIndexMap]: Отслеживает какую вкладку выбрали у карт

        private int _selectedTabControlIndexMap;
        public int SelectedTabControlIndexMap
        {
            get => _selectedTabControlIndexMap;
            set => SetProperty(ref _selectedTabControlIndexMap, value);
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateLocalMeasurementCommand = new RelayCommand(Execute_CreateLocalMeasurementCommand, CanExecute_CreateLocalMeasurementCommand);
            DeleteLocalMeasurementCommand = new RelayCommand<MeasurementViewModel>(Execute_DeleteLocalMeasurementCommand, CanExecute_DeleteLocalMeasurementCommand);

            CreateLocalMapCommand = new RelayCommand(Execute_CreateLocalMapCommand, CanExecute_CreateLocalMapCommand);
            DeleteLocalMapCommand = new RelayCommand<MapViewModel>(Execute_DeleteLocalMapCommand, CanExecute_DeleteLocalMapCommand);

            SelectNewImageCommand = new RelayCommand(Execute_SelectNewImageCommand, CanExecute_SelectNewImageCommand);
            SelectSelectedLocalMapImageCommand = new RelayCommand(Execute_SelectSelectedLocalMapImageCommand, CanExecute_SelectSelectedLocalMapImageCommand);
            SelectedMapImageCommand = new RelayCommand(Execute_SelectedMapImageCommand, CanExecute_SelectedMapImageCommand);

            SaveDataCommandAsync = new RelayCommandAsync(Execute_SaveDataCommandAsync, CanExecute_SaveDataCommandAsync);

            UpdateMapCommandAsync = new RelayCommandAsync<MapViewModel>(Execute_UpdateMapCommandAsync, CanExecute_UpdateMapCommandAsync);

            DeleteMeasurementCommandAsync = new RelayCommandAsync<MeasurementViewModel>(Execute_DeleteMeasurementCommandAsync, CanExecute_DeleteMeasurementCommandAsync);
            UpdateMeasurementCommandAsync = new RelayCommandAsync<MeasurementViewModel>(Execute_UpdateMeasurementCommandAsync, CanExecute_UpdateMeasurementCommandAsync);
            RevertChangeMeasurementCommand = new RelayCommand<MeasurementViewModel>(Execute_RevertChangeMeasurementCommand, CanExecute_RevertChangeMeasurementCommand);

            DeleteMapCommandAsync = new RelayCommandAsync<MapViewModel>(Execute_DeleteMapCommandAsync, CanExecute_DeleteMapCommandAsync);
            RevertMapChangeCommand = new RelayCommand<MapViewModel>(Execute_RevertMapChangeCommand, CanExecute_RevertMapChangeCommand);
            CopyAllInfoMapCommand = new RelayCommand<MapViewModel>(Execute_CopyAllInfoMapCommand, CanExecute_CopyAllInfoMapCommand);

            ShowEditAndHideMenuLocalMapCommand = new RelayCommand<object>(Execute_ShowEditAndHideMenuLocalMapCommand, CanExecute_ShowEditAndHideMenuLocalMapCommand);
            ShowEditAndHideMenuMapCommand = new RelayCommand<object>(Execute_ShowEditAndHideMenuMapCommand, CanExecute_ShowEditAndHideMenuMapCommand);
        }

        /*--Локальные действия--*/

        #region Команда [CreateLocalMeasurementCommand]: Создание Measurement в памяти

        public RelayCommand? CreateLocalMeasurementCommand { get; private set; }

        private void Execute_CreateLocalMeasurementCommand()
        {
            if (NewMeasurement is null)
                return;

            NewMeasurement.SetLocalID(Guid.NewGuid());

            LocalMeasurements.Add(NewMeasurement);

            NewMeasurement.ChangeSaveStatus(false);

            PrepareNewMeasurement();
        }

        private bool CanExecute_CreateLocalMeasurementCommand()
            => NewMeasurement != null &&
                !string.IsNullOrWhiteSpace(NewMeasurement.Name) && NewMeasurement.OldId >= 0;

        #endregion

        #region Команда [DeleteLocalMeasurementCommand]: Удаление локального измерения

        public RelayCommand<MeasurementViewModel>? DeleteLocalMeasurementCommand { get; private set; }

        private void Execute_DeleteLocalMeasurementCommand(MeasurementViewModel model)
        {
            if (MessageBox.Show($"Вы точно хотите удалить локальное {model.Name} измерение? Вместе с ним удаляться и локальные карты", "Предупреждение.", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                LocalMeasurements.Remove(model);
            }
        }

        private bool CanExecute_DeleteLocalMeasurementCommand(MeasurementViewModel model) => true;

        #endregion

        #region Команда [CreateLocalMapCommand]: Создание Map в памяти

        public RelayCommand? CreateLocalMapCommand { get; private set; }

        private void Execute_CreateLocalMapCommand()
        {
            if (NewMap is null || SelectedLocalMeasurement is null)
                return;

            NewMap.SetMeasurementLocalId(SelectedLocalMeasurement.LocalId!);

            _localMaps.Add(NewMap);
            NewMap.ChangeSaveStatus(false);

            PrepareNewMap();
        }

        private bool CanExecute_CreateLocalMapCommand() => 
            NewMap != null &&
            !string.IsNullOrWhiteSpace(NewMap.Name) && 
            NewMap.OldId >= 0 &&
            SelectedMeasurement is not null;

        #endregion

        #region Команда [DeleteLocalMapCommand]: Удаление локальной карты

        public RelayCommand<MapViewModel>? DeleteLocalMapCommand { get; private set; }

        private void Execute_DeleteLocalMapCommand(MapViewModel model)
        {
            _localMaps.Remove(model);
            MenuLocalMapPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_DeleteLocalMapCommand(MapViewModel model) => true;

        #endregion

        /*--Серверные действия--*/

        #region Команда [SaveDataCommandAsync]: Сохранение созданных в памяти измерений и карт  

        public RelayCommandAsync? SaveDataCommandAsync { get; private set; }

        private async Task Execute_SaveDataCommandAsync()
        {
            if (SelectedLocalMeasurement is null)
            {
                MessageBox.Show("Вы не выбрали измерение для добавления.");
                return;
            }

            IsBusy = true;

            try
            {
                var addMeasurement = new ClientAddMeasurementRequest(
                    OldId: SelectedLocalMeasurement!.OldId, 
                    Name: SelectedLocalMeasurement.Name,
                    Maps: [.. _localMaps.Where(m => m.MeasurementLocalId == SelectedLocalMeasurement.LocalId).Select(cmd => new ClientMapData(cmd.OldId, cmd.Name, cmd.ImagePath, cmd.Name))]);

                var result = await _measurementService.AddAsync(addMeasurement);
               
                if (!result.IsSuccess)
                {
                    MessageBox.Show("Не удалось добавить запись");
                    return;
                }

                var addedMeasurement = new MeasurementViewModel(new MeasurementSoloResponse(result.Value.Id, result.Value.OldId, result.Value.Name));
                addedMeasurement.ChangeSaveStatus(true);

                Measurements.Add(addedMeasurement);

                LocalMeasurements.Remove(SelectedLocalMeasurement);

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

        private bool CanExecute_SaveDataCommandAsync() => true;

        #endregion

        //Измерения

        #region Команда [DeleteMeasurementCommandAsync]: Удаление измерения с картами

        public RelayCommandAsync<MeasurementViewModel>? DeleteMeasurementCommandAsync { get; private set; }

        private async Task Execute_DeleteMeasurementCommandAsync(MeasurementViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _measurementService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    var measurementId = model.Id;

                    Measurements.Remove(model);

                    foreach (var map in Maps)
                    {
                        if (map.MeasurementId == measurementId)
                            Maps.Remove(map);
                    }

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

        private bool CanExecute_DeleteMeasurementCommandAsync(MeasurementViewModel model) => true;

        #endregion

        #region Команда [UpdateMeasurementCommandAsync]: Обновление измерения

        public RelayCommandAsync<MeasurementViewModel>? UpdateMeasurementCommandAsync { get; private set; }

        private async Task Execute_UpdateMeasurementCommandAsync(MeasurementViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _measurementService.UpdateAsync(model.Id, new UpdateMeasurementRequest(model.Name));

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

        private bool CanExecute_UpdateMeasurementCommandAsync(MeasurementViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [RevertChangeMeasurementCommand]: Откат изменений у измерения

        public RelayCommand<MeasurementViewModel>? RevertChangeMeasurementCommand { get; private set; }

        private void Execute_RevertChangeMeasurementCommand(MeasurementViewModel model)
        {
            model?.RevertChanges();
            MenuMapPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_RevertChangeMeasurementCommand(MeasurementViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion 

        // Карты

        #region Команда [DeleteMapCommandAsync]: Удаление карты

        public RelayCommandAsync<MapViewModel>? DeleteMapCommandAsync { get; private set; }

        private async Task Execute_DeleteMapCommandAsync(MapViewModel model)
        {
            if (model is null)
                return;

            if (SelectedMeasurement is null)
                MessageBox.Show("Не выбрано измерение");

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var mapService = _mapApiServiceFactory.Create(SelectedMeasurement!.Id);

                    var result = await mapService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    Maps.Remove(model);
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

        private bool CanExecute_DeleteMapCommandAsync(MapViewModel model) => true;

        #endregion

        #region Команда [UpdateMapCommandAsync]: Обновление записи Map

        public RelayCommandAsync<MapViewModel>? UpdateMapCommandAsync { get; private set; }

        private async Task Execute_UpdateMapCommandAsync(MapViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var service = _mapApiServiceFactory.Create(SelectedMeasurement!.Id);

                var result = await service.UpdateAsync(new ClientUpdateMapRequest(model.Id, model.Name, model.ImagePath!, model.Name));

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

        private bool CanExecute_UpdateMapCommandAsync(MapViewModel model)
        {
            //if (model is not null)
            //    return model.HasChanges;

            //return false;

            return true;
        }

        #endregion

        #region Команда [RevertMapChangeCommand]: Откат изменений у карты

        public RelayCommand<MapViewModel>? RevertMapChangeCommand { get; private set; }

        private void Execute_RevertMapChangeCommand(MapViewModel model)
        {
            model?.RevertChanges();
            MenuMapPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_RevertMapChangeCommand(MapViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [CopyAllInfoMapCommand]: Копирования всей иформации

        public RelayCommand<MapViewModel>? CopyAllInfoMapCommand { get; private set; }

        private void Execute_CopyAllInfoMapCommand(MapViewModel model)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"ID: {model.Id}");
            sb.AppendLine($"Имя: {model.Name}");
            sb.AppendLine($"Измерения: {SelectedMeasurement?.Name} (ID: {model.MeasurementId}");
            sb.AppendLine($"Старое ID: {model.OldId}");
            sb.AppendLine($"ImageKey: {model.ImageKey}");

            Clipboard.SetText(sb.ToString());

            MenuMapPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_CopyAllInfoMapCommand(MapViewModel model) => true;

        #endregion

        /*--Выбор изображений--*/

        #region Команда [SelectNewImageCommand]: Выбор изображения для создания карты

        public RelayCommand? SelectNewImageCommand { get; private set; }

        private void Execute_SelectNewImageCommand()
        {
            var path = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");

            if (path is null)
                return;

            NewMap?.SetImage(ImageHelper.ImageFromFilePath(path), path);
            MenuMapPopup.HideCommand.Execute(null);
        }

        private bool CanExecute_SelectNewImageCommand() => true;

        #endregion

        #region Команда [SelectSelectedLocalMapImageCommand]: Выбор изображения для создания карты

        public RelayCommand? SelectSelectedLocalMapImageCommand { get; private set; }

        private void Execute_SelectSelectedLocalMapImageCommand()
        {
            var path = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");

            if (path is null)
                return;

            SelectedLocalMap?.SetImage(ImageHelper.ImageFromFilePath(path), path); 
        }

        private bool CanExecute_SelectSelectedLocalMapImageCommand() => true;

        #endregion

        #region Команда [SelectedMapImageCommand]: Выбор изображения для создания карты

        public RelayCommand? SelectedMapImageCommand { get; private set; }

        private void Execute_SelectedMapImageCommand()
        {
            var path = _fileDialogService.OpenFileDialog("Выберите файл", "Image Files|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*");

            if (path is null)
                return;

            SelectedMap?.SetImage(ImageHelper.ImageFromFilePath(path), path); 
        }

        private bool CanExecute_SelectedMapImageCommand() => true;

        #endregion

        /*--Popups--*/

        #region Команда [ShowEditAndHideMenuLocalMapCommand]: Управление всплывающеми окнами

        public RelayCommand<object>? ShowEditAndHideMenuLocalMapCommand { get; private set; }

        private void Execute_ShowEditAndHideMenuLocalMapCommand(object parameter)
        {
            UIElement? target = MenuLocalMapPopup.PlacementTarget;

            if (target == null)
            {
                MessageBox.Show("Не удалось установить объект к которому нужно прикрепить Popup");
                return;
            }

            MenuLocalMapPopup.HideCommand.Execute(null);
            EditLocalMapPopup.ShowCommand.Execute(target);
        }

        private bool CanExecute_ShowEditAndHideMenuLocalMapCommand(object parameter) => true;

        #endregion

        #region Команда [ShowEditAndHideMenuMapCommand]: Управление всплывающеми окнами

        public RelayCommand<object>? ShowEditAndHideMenuMapCommand { get; private set; }

        private void Execute_ShowEditAndHideMenuMapCommand(object parameter)
        {
            UIElement? target = MenuMapPopup.PlacementTarget;

            if (target == null)
            {
                MessageBox.Show("Не удалось установить объект к которому нужно прикрепить Popup");
                return;
            }

            MenuMapPopup.HideCommand.Execute(null);
            EditMapPopup.ShowCommand.Execute(target);
        }

        private bool CanExecute_ShowEditAndHideMenuMapCommand(object parameter) => true;

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Методы [GetAll]: Получение Measurements, Maps

        public async Task GetAllMeasurements()
        {
            var measurements = await _measurementService.GetAllAsync();

            foreach (var measurement in measurements)
            {
                var measurementVM = new MeasurementViewModel(measurement);
                measurementVM.ChangeSaveStatus(true);

                Measurements.Add(measurementVM);
            }

            SelectedMeasurement = Measurements.FirstOrDefault();
        }

        public async Task GetMapsForMeasurement()
        {
            if (SelectedMeasurement is null)
            {
                Maps.Clear();
                return;
            }

            Maps.Clear();

            var mapService = _mapApiServiceFactory.Create(SelectedMeasurement.Id);

            var maps = await mapService.GetAllAsync();

            foreach (var map in maps)
            {
                var url = await _fileStorageService.GetPresignedLinkAsync($"{FileStoragePaths.Maps}/{map.ImageKey}");

                var mapVm = new MapViewModel(map);
                mapVm.SetImage(await ImageHelper.ImageFromUrlAsync(url), string.Empty);
                mapVm.ChangeSaveStatus(true);

                Maps.Add(mapVm);
            }
               
        }

        #endregion

        #region Методы [PrepareNewM]: Создание пустого MeasurementViewModel, MapViewModel для будущего добавление

        private void PrepareNewMeasurement() => NewMeasurement = new MeasurementViewModel(MeasurementSoloResponse.Empty);

        private void PrepareNewMap() => NewMap = new MapViewModel(MapResponse.Empty);

        #endregion

        /*--Фильтрация--*/

        #region Метод [FilterLocalMaps]: Фильтрует локальные карты по выбранному локальному измерению

        private bool FilterLocalMaps(object item)
        {

            if (SelectedLocalMeasurement == null)
                return false;

            if (item is MapViewModel map)
                return map.MeasurementLocalId == SelectedLocalMeasurement.LocalId;

            return false;
        }

        #endregion
    }
}