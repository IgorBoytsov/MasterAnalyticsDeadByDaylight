using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.WhoPlacedMap;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Update;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Pages
{
    internal sealed class WhoPlacedMapPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IWhoPlacedMapService _whoPlacedMapService;

        public WhoPlacedMapPageViewModel(IWhoPlacedMapService whoPlacedMapService)
        {
            _whoPlacedMapService = whoPlacedMapService;

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

        public ObservableCollection<WhoPlacedMapViewModel> WhoPlacedMaps { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        #region Свойство: [SelectedWhoPlacedMap] Метод: [OnSelectedWhoPlacedMapPropertyChanged]

        private WhoPlacedMapViewModel? _selectedWhoPlacedMap;
        public WhoPlacedMapViewModel? SelectedWhoPlacedMap
        {
            get => _selectedWhoPlacedMap;
            set
            {
                if (_selectedWhoPlacedMap is not null)
                    _selectedWhoPlacedMap.PropertyChanged -= OnSelectedWhoPlacedMapPropertyChanged;

                SetProperty(ref _selectedWhoPlacedMap, value);

                if (_selectedWhoPlacedMap is not null)
                    _selectedWhoPlacedMap.PropertyChanged += OnSelectedWhoPlacedMapPropertyChanged;

                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnSelectedWhoPlacedMapPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewWhoPlacedMap] Метод: [OnNewWhoPlacedMapPropertyChanged] 

        private WhoPlacedMapViewModel? _newWhoPlacedMap;
        public WhoPlacedMapViewModel? NewWhoPlacedMap
        {
            get => _newWhoPlacedMap;
            private set
            {
                if (_newWhoPlacedMap is not null)
                    _newWhoPlacedMap.PropertyChanged -= OnNewWhoPlacedMapPropertyChanged;

                SetProperty(ref _newWhoPlacedMap, value);

                if (_newWhoPlacedMap is not null)
                    _newWhoPlacedMap.PropertyChanged += OnNewWhoPlacedMapPropertyChanged;

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnNewWhoPlacedMapPropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<WhoPlacedMapViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommand);
            DeleteCommandAsync = new RelayCommandAsync<WhoPlacedMapViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommand);
            RevertChangeCommand = new RelayCommand<WhoPlacedMapViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);
        }

        #region Команда [CreateCommandAsync]: Создание WhoPlacedMap

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewWhoPlacedMap is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewWhoPlacedMap.ToModel();

                var result = await _whoPlacedMapService.AddAsync(newModel);

                if (!result.IsSuccess)
                {
                    MessageBox.Show("Не удалось добавить запись");
                    return;
                }

                WhoPlacedMaps.Add(new WhoPlacedMapViewModel(result.Value!));

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

        private bool CanExecute_CreateCommandAsync() => NewWhoPlacedMap != null && !string.IsNullOrWhiteSpace(NewWhoPlacedMap.Name) && NewWhoPlacedMap.OldId >= 0;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление WhoPlacedMap

        public RelayCommandAsync<WhoPlacedMapViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(WhoPlacedMapViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _whoPlacedMapService.UpdateAsync(model.Id, new UpdateWhoPlacedMapRequest(model.Name));

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

        private bool CanExecute_UpdateCommand(WhoPlacedMapViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление WhoPlacedMap

        public RelayCommandAsync<WhoPlacedMapViewModel>? DeleteCommandAsync { get; private set; }

        private async Task Execute_DeleteCommandAsync(WhoPlacedMapViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _whoPlacedMapService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    WhoPlacedMaps.Remove(model);
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

        private bool CanExecute_DeleteCommand(WhoPlacedMapViewModel model) => model is not null;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<WhoPlacedMapViewModel>? RevertChangeCommand { get; private set; }

        private void Execute_RevertChangeCommand(WhoPlacedMapViewModel model) => model?.RevertChanges();

        private bool CanExecute_RevertChangeCommand(WhoPlacedMapViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [PrepareNew]: Создание пустого WhoPlacedMapViewModel для будущего добавление

        private void PrepareNew() => NewWhoPlacedMap = new WhoPlacedMapViewModel(WhoPlacedMapResponse.Empty);

        #endregion

        #region Метод [GetAll]: Получение WhoPlacedMaps

        public async Task GetAll()
        {
            var whoPlacedMaps = await _whoPlacedMapService.GetAllAsync();

            foreach (var whoPlacedMap in whoPlacedMaps)
                WhoPlacedMaps.Add(new WhoPlacedMapViewModel(whoPlacedMap));
        }

        #endregion
    }
}