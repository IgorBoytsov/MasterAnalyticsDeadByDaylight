using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerkCategory;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Update;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Pages
{
    internal sealed class SurvivorPerkCategoryPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly ISurvivorPerkCategoryService _survivorPerkCategoryService;

        public SurvivorPerkCategoryPageViewModel(ISurvivorPerkCategoryService survivorPerkCategoryService)
        {
            _survivorPerkCategoryService = survivorPerkCategoryService;

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

        public ObservableCollection<SurvivorPerkCategoryViewModel> SurvivorPerkCategories { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        #region Свойство: [SelectedSurvivorPerkCategory] Метод: [OnSelectedSurvivorPerkCategoryPropertyChanged]

        private SurvivorPerkCategoryViewModel? _selectedSurvivorPerkCategory;
        public SurvivorPerkCategoryViewModel? SelectedSurvivorPerkCategory
        {
            get => _selectedSurvivorPerkCategory;
            set
            {
                if (_selectedSurvivorPerkCategory is not null)
                    _selectedSurvivorPerkCategory.PropertyChanged -= OnSelectedSurvivorPerkCategoryPropertyChanged;

                SetProperty(ref _selectedSurvivorPerkCategory, value);

                if (_selectedSurvivorPerkCategory is not null)
                    _selectedSurvivorPerkCategory.PropertyChanged += OnSelectedSurvivorPerkCategoryPropertyChanged;

                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnSelectedSurvivorPerkCategoryPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewSurvivorPerkCategory] Метод: [OnNewSurvivorPerkCategoryPropertyChanged] 

        private SurvivorPerkCategoryViewModel? _newSurvivorPerkCategory;
        public SurvivorPerkCategoryViewModel? NewSurvivorPerkCategory
        {
            get => _newSurvivorPerkCategory;
            private set
            {
                if (_newSurvivorPerkCategory is not null)
                    _newSurvivorPerkCategory.PropertyChanged -= OnNewSurvivorPerkCategoryPropertyChanged;

                SetProperty(ref _newSurvivorPerkCategory, value);

                if (_newSurvivorPerkCategory is not null)
                    _newSurvivorPerkCategory.PropertyChanged += OnNewSurvivorPerkCategoryPropertyChanged;

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnNewSurvivorPerkCategoryPropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<SurvivorPerkCategoryViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommand);
            DeleteCommandAsync = new RelayCommandAsync<SurvivorPerkCategoryViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommand);
            RevertChangeCommand = new RelayCommand<SurvivorPerkCategoryViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);
        }

        #region Команда [CreateCommandAsync]: Создание SurvivorPerkCategory

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewSurvivorPerkCategory is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewSurvivorPerkCategory.ToModel();

                var result = await _survivorPerkCategoryService.AddAsync(newModel);

                if (!result.IsSuccess)
                {
                    MessageBox.Show("Не удалось добавить запись");
                    return;
                }

                SurvivorPerkCategories.Add(new SurvivorPerkCategoryViewModel(result.Value!));

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

        private bool CanExecute_CreateCommandAsync() => NewSurvivorPerkCategory != null && !string.IsNullOrWhiteSpace(NewSurvivorPerkCategory.Name) && NewSurvivorPerkCategory.OldId >= 0;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление SurvivorPerkCategory

        public RelayCommandAsync<SurvivorPerkCategoryViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(SurvivorPerkCategoryViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _survivorPerkCategoryService.UpdateAsync(model.Id, new UpdateSurvivorPerkCategoryRequest(model.Name));

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

        private bool CanExecute_UpdateCommand(SurvivorPerkCategoryViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление SurvivorPerkCategory

        public RelayCommandAsync<SurvivorPerkCategoryViewModel>? DeleteCommandAsync { get; private set; }

        private async Task Execute_DeleteCommandAsync(SurvivorPerkCategoryViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _survivorPerkCategoryService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    SurvivorPerkCategories.Remove(model);
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

        private bool CanExecute_DeleteCommand(SurvivorPerkCategoryViewModel model) => model is not null;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<SurvivorPerkCategoryViewModel>? RevertChangeCommand { get; private set; }

        private void Execute_RevertChangeCommand(SurvivorPerkCategoryViewModel model) => model?.RevertChanges();

        private bool CanExecute_RevertChangeCommand(SurvivorPerkCategoryViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [PrepareNew]: Создание пустого SurvivorPerkCategoryViewModel для будущего добавление

        private void PrepareNew() => NewSurvivorPerkCategory = new SurvivorPerkCategoryViewModel(SurvivorPerkCategoryResponse.Empty);

        #endregion

        #region Метод [GetAll]: Получение SurvivorPerkCategories

        public async Task GetAll()
        {
            var survivorPerkCategories = await _survivorPerkCategoryService.GetAllAsync();

            foreach (var survivorPerkCategory in survivorPerkCategories)
                SurvivorPerkCategories.Add(new SurvivorPerkCategoryViewModel(survivorPerkCategory));
        }

        #endregion
    }
}