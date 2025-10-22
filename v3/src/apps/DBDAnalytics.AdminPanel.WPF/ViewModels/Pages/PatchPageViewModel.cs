using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Patch;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Create;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Update;
using DBDAnalytics.Shared.Contracts.Responses;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Pages
{
    internal sealed class PatchPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IPatchService _patchService;

        public PatchPageViewModel(IPatchService patchService)
        {
            _patchService = patchService;

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

        public ObservableCollection<PatchViewModel> Patches { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        #region Свойство: [SelectedPatch] Метод: [OnSelectedPatchPropertyChanged]

        private PatchViewModel? _selectedPatch;
        public PatchViewModel? SelectedPatch
        {
            get => _selectedPatch;
            set
            {
                if (_selectedPatch is not null)
                    _selectedPatch.PropertyChanged -= OnSelectedPatchPropertyChanged;

                SetProperty(ref _selectedPatch, value);

                if (_selectedPatch is not null)
                    _selectedPatch.PropertyChanged += OnSelectedPatchPropertyChanged;

                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnSelectedPatchPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewPatch] Метод: [OnNewPatchPropertyChanged] 

        private PatchViewModel? _newPatch;
        public PatchViewModel? NewPatch
        {
            get => _newPatch;
            private set
            {
                if (_newPatch is not null)
                    _newPatch.PropertyChanged -= OnNewPatchPropertyChanged;

                SetProperty(ref _newPatch, value);

                if (_newPatch is not null)
                    _newPatch.PropertyChanged += OnNewPatchPropertyChanged;

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnNewPatchPropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<PatchViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommand);
            DeleteCommandAsync = new RelayCommandAsync<PatchViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommand);
            RevertChangeCommand = new RelayCommand<PatchViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);
        }

        #region Команда [CreateCommandAsync]: Создание Patch

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewPatch is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewPatch.ToModel();

                var response = await _patchService.AddAsync(new CreatePatchRequest(newModel.OldId, newModel.Name, DateTime.Parse(newModel.Date)));

                if (!response.IsSuccess)
                {
                    MessageBox.Show($"Не удалось добавить запись: {response.StringMessage}");
                    return;
                }

                Patches.Add(new PatchViewModel(response.Value!));

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

        private bool CanExecute_CreateCommandAsync() => NewPatch != null && !string.IsNullOrWhiteSpace(NewPatch.Name) && NewPatch.OldId >= 0;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление Patch

        public RelayCommandAsync<PatchViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(PatchViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _patchService.UpdateAsync(model.Id, new UpdatePatchRequest(model.OldId,model.Name, model.Date));

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

        private bool CanExecute_UpdateCommand(PatchViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление Patch

        public RelayCommandAsync<PatchViewModel>? DeleteCommandAsync { get; private set; }

        private async Task Execute_DeleteCommandAsync(PatchViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _patchService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    Patches.Remove(model);
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

        private bool CanExecute_DeleteCommand(PatchViewModel model) => model is not null;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<PatchViewModel>? RevertChangeCommand { get; private set; }

        private void Execute_RevertChangeCommand(PatchViewModel model) => model?.RevertChanges();

        private bool CanExecute_RevertChangeCommand(PatchViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [PrepareNew]: Создание пустого PatchViewModel для будущего добавление

        private void PrepareNew() => NewPatch = new PatchViewModel(PatchResponse.Empty);

        #endregion

        #region Метод [GetAll]: Получение Patches

        public async Task GetAll()
        {
            var patches = await _patchService.GetAllAsync();

            foreach (var patch in patches)
                Patches.Add(new PatchViewModel(patch));
        }

        #endregion
    }
}