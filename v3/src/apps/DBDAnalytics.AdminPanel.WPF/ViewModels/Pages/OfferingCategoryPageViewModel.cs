using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.OfferingCategory;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Update;
using DBDAnalytics.Shared.Contracts.Responses.Offering;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Pages
{
    internal sealed class OfferingCategoryPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IOfferingCategoryService _offeringCategoryService;

        public OfferingCategoryPageViewModel(IOfferingCategoryService offeringCategoryService)
        {
            _offeringCategoryService = offeringCategoryService;

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

        public ObservableCollection<OfferingCategoryViewModel> OfferingCategories { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        #region Свойство: [SelectedOfferingCategory] Метод: [OnSelectedOfferingCategoryPropertyChanged]

        private OfferingCategoryViewModel? _selectedOfferingCategory;
        public OfferingCategoryViewModel? SelectedOfferingCategory
        {
            get => _selectedOfferingCategory;
            set
            {
                if (_selectedOfferingCategory is not null)
                    _selectedOfferingCategory.PropertyChanged -= OnSelectedOfferingCategoryPropertyChanged;

                SetProperty(ref _selectedOfferingCategory, value);

                if (_selectedOfferingCategory is not null)
                    _selectedOfferingCategory.PropertyChanged += OnSelectedOfferingCategoryPropertyChanged;

                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnSelectedOfferingCategoryPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewOfferingCategory] Метод: [OnNewOfferingCategoryPropertyChanged] 

        private OfferingCategoryViewModel? _newOfferingCategory;
        public OfferingCategoryViewModel? NewOfferingCategory
        {
            get => _newOfferingCategory;
            private set
            {
                if (_newOfferingCategory is not null)
                    _newOfferingCategory.PropertyChanged -= OnNewOfferingCategoryPropertyChanged;

                SetProperty(ref _newOfferingCategory, value);

                if (_newOfferingCategory is not null)
                    _newOfferingCategory.PropertyChanged += OnNewOfferingCategoryPropertyChanged;

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnNewOfferingCategoryPropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<OfferingCategoryViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommand);
            DeleteCommandAsync = new RelayCommandAsync<OfferingCategoryViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommand);
            RevertChangeCommand = new RelayCommand<OfferingCategoryViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);
        }

        #region Команда [CreateCommandAsync]: Создание OfferingCategory

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewOfferingCategory is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewOfferingCategory.ToModel();

                var result = await _offeringCategoryService.AddAsync(newModel);

                if (!result.IsSuccess)
                {
                    MessageBox.Show("Не удалось добавить запись");
                    return;
                }

                OfferingCategories.Add(new OfferingCategoryViewModel(result.Value!));

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

        private bool CanExecute_CreateCommandAsync() => NewOfferingCategory != null && !string.IsNullOrWhiteSpace(NewOfferingCategory.Name) && NewOfferingCategory.OldId >= 0;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление OfferingCategory

        public RelayCommandAsync<OfferingCategoryViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(OfferingCategoryViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _offeringCategoryService.UpdateAsync(model.Id, new UpdateOfferingCategoryRequest(model.Name));

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

        private bool CanExecute_UpdateCommand(OfferingCategoryViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление OfferingCategory

        public RelayCommandAsync<OfferingCategoryViewModel>? DeleteCommandAsync { get; private set; }

        private async Task Execute_DeleteCommandAsync(OfferingCategoryViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _offeringCategoryService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    OfferingCategories.Remove(model);
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

        private bool CanExecute_DeleteCommand(OfferingCategoryViewModel model) => model is not null;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<OfferingCategoryViewModel>? RevertChangeCommand { get; private set; }

        private void Execute_RevertChangeCommand(OfferingCategoryViewModel model) => model?.RevertChanges();

        private bool CanExecute_RevertChangeCommand(OfferingCategoryViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [PrepareNew]: Создание пустого OfferingCategoryViewModel для будущего добавление

        private void PrepareNew() => NewOfferingCategory = new OfferingCategoryViewModel(OfferingCategoryResponse.Empty);

        #endregion

        #region Метод [GetAll]: Получение OfferingCategories

        public async Task GetAll()
        {
            var offeringCategories = await _offeringCategoryService.GetAllAsync();

            foreach (var offeringCategory in offeringCategories)
                OfferingCategories.Add(new OfferingCategoryViewModel(offeringCategory));
        }

        #endregion
    }
}