using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Platform;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Update;
using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Pages
{
    internal sealed class PlatformPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IPlatformService _platformService;

        public PlatformPageViewModel(IPlatformService platformService)
        {
            _platformService = platformService;

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

        public ObservableCollection<PlatformViewModel> Platforms { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        #region Свойство: [SelectedPlatform] Метод: [OnSelectedPlatformPropertyChanged]

        private PlatformViewModel? _selectedPlatform;
        public PlatformViewModel? SelectedPlatform
        {
            get => _selectedPlatform;
            set
            {
                if (_selectedPlatform is not null)
                    _selectedPlatform.PropertyChanged -= OnSelectedPlatformPropertyChanged;

                SetProperty(ref _selectedPlatform, value);

                if (_selectedPlatform is not null)
                    _selectedPlatform.PropertyChanged += OnSelectedPlatformPropertyChanged;

                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnSelectedPlatformPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewPlatform] Метод: [OnNewPlatformPropertyChanged] 

        private PlatformViewModel? _newPlatform;
        public PlatformViewModel? NewPlatform
        {
            get => _newPlatform;
            private set
            {
                if (_newPlatform is not null)
                    _newPlatform.PropertyChanged -= OnNewPlatformPropertyChanged;

                SetProperty(ref _newPlatform, value);

                if (_newPlatform is not null)
                    _newPlatform.PropertyChanged += OnNewPlatformPropertyChanged;

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnNewPlatformPropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<PlatformViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommand);
            DeleteCommandAsync = new RelayCommandAsync<PlatformViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommand);
            RevertChangeCommand = new RelayCommand<PlatformViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);
        }

        #region Команда [CreateCommandAsync]: Создание Platform

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewPlatform is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewPlatform.ToModel();

                var response = await _platformService.AddAsync(newModel);

                if (!response.IsSuccess)
                {
                    MessageBox.Show("Не удалось добавить запись");
                    return;
                }

                Platforms.Add(new PlatformViewModel(response.Value!));

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

        private bool CanExecute_CreateCommandAsync() => NewPlatform != null && !string.IsNullOrWhiteSpace(NewPlatform.Name) && NewPlatform.OldId >= 0;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление Platform

        public RelayCommandAsync<PlatformViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(PlatformViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _platformService.UpdateAsync(model.Id, new UpdatePlatformRequest(model.Name));

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

        private bool CanExecute_UpdateCommand(PlatformViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление Platform

        public RelayCommandAsync<PlatformViewModel>? DeleteCommandAsync { get; private set; }

        private async Task Execute_DeleteCommandAsync(PlatformViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _platformService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    Platforms.Remove(model);
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

        private bool CanExecute_DeleteCommand(PlatformViewModel model) => model is not null;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<PlatformViewModel>? RevertChangeCommand { get; private set; }

        private void Execute_RevertChangeCommand(PlatformViewModel model) => model?.RevertChanges();

        private bool CanExecute_RevertChangeCommand(PlatformViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [PrepareNew]: Создание пустого PlatformViewModel для будущего добавление

        private void PrepareNew() => NewPlatform = new PlatformViewModel(PlatformResponse.Empty);

        #endregion

        #region Метод [GetAll]: Получение Platforms

        public async Task GetAll()
        {
            var latforms = await _platformService.GetAllAsync();

            foreach (var platform in latforms)
                Platforms.Add(new PlatformViewModel(platform));
        }

        #endregion
    }
}