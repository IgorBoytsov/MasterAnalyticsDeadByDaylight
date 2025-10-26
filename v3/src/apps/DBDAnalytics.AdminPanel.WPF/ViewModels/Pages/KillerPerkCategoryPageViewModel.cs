using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerkCategory;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Update;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Pages
{
    internal sealed class KillerPerkCategoryPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IKillerPerkCategoryService _killerPerkCategoryService;

        public KillerPerkCategoryPageViewModel(IKillerPerkCategoryService killerPerkCategoryService)
        {
            _killerPerkCategoryService = killerPerkCategoryService;

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

        public ObservableCollection<KillerPerkCategoryViewModel> KillerPerkCategories { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        #region Свойство: [SelectedKillerPerkCategory] Метод: [OnSelectedKillerPerkCategoryPropertyChanged]

        private KillerPerkCategoryViewModel? _selectedKillerPerkCategory;
        public KillerPerkCategoryViewModel? SelectedKillerPerkCategory
        {
            get => _selectedKillerPerkCategory;
            set
            {
                if (_selectedKillerPerkCategory is not null)
                    _selectedKillerPerkCategory.PropertyChanged -= OnSelectedKillerPerkCategoryPropertyChanged;

                SetProperty(ref _selectedKillerPerkCategory, value);

                if (_selectedKillerPerkCategory is not null)
                    _selectedKillerPerkCategory.PropertyChanged += OnSelectedKillerPerkCategoryPropertyChanged;

                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnSelectedKillerPerkCategoryPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewKillerPerkCategory] Метод: [OnNewKillerPerkCategoryPropertyChanged] 

        private KillerPerkCategoryViewModel? _newKillerPerkCategory;
        public KillerPerkCategoryViewModel? NewKillerPerkCategory
        {
            get => _newKillerPerkCategory;
            private set
            {
                if (_newKillerPerkCategory is not null)
                    _newKillerPerkCategory.PropertyChanged -= OnNewKillerPerkCategoryPropertyChanged;

                SetProperty(ref _newKillerPerkCategory, value);

                if (_newKillerPerkCategory is not null)
                    _newKillerPerkCategory.PropertyChanged += OnNewKillerPerkCategoryPropertyChanged;

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnNewKillerPerkCategoryPropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        #region Управление меню с добавлением\редактированием данных

        private GridLength _editMenuColumnHeight = new(0);
        public GridLength EditMenuColumnHeight
        {
            get => _editMenuColumnHeight;
            set => SetProperty(ref _editMenuColumnHeight, value);
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<KillerPerkCategoryViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommand);
            DeleteCommandAsync = new RelayCommandAsync<KillerPerkCategoryViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommand);
            RevertChangeCommand = new RelayCommand<KillerPerkCategoryViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);
        }

        #region Команда [CreateCommandAsync]: Создание KillerPerkCategory

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewKillerPerkCategory is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewKillerPerkCategory.ToModel();

                var response = await _killerPerkCategoryService.AddAsync(newModel);

                if (!response.IsSuccess)
                {
                    MessageBox.Show("Не удалось добавить запись");
                    return;
                }

                KillerPerkCategories.Add(new KillerPerkCategoryViewModel(response.Value!));

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

        private bool CanExecute_CreateCommandAsync() => NewKillerPerkCategory != null && !string.IsNullOrWhiteSpace(NewKillerPerkCategory.Name) && NewKillerPerkCategory.OldId >= 0;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление KillerPerkCategory

        public RelayCommandAsync<KillerPerkCategoryViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(KillerPerkCategoryViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _killerPerkCategoryService.UpdateAsync(model.Id, new UpdateKillerPerkCategoryRequest(model.Name));

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

        private bool CanExecute_UpdateCommand(KillerPerkCategoryViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление KillerPerkCategory

        public RelayCommandAsync<KillerPerkCategoryViewModel>? DeleteCommandAsync { get; private set; }

        private async Task Execute_DeleteCommandAsync(KillerPerkCategoryViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _killerPerkCategoryService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    KillerPerkCategories.Remove(model);
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

        private bool CanExecute_DeleteCommand(KillerPerkCategoryViewModel model) => model is not null;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<KillerPerkCategoryViewModel>? RevertChangeCommand { get; private set; }

        private void Execute_RevertChangeCommand(KillerPerkCategoryViewModel model) => model?.RevertChanges();

        private bool CanExecute_RevertChangeCommand(KillerPerkCategoryViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [PrepareNew]: Создание пустого KillerPerkCategoryViewModel для будущего добавление

        private void PrepareNew() => NewKillerPerkCategory = new KillerPerkCategoryViewModel(KillerPerkCategoryResponse.Empty);

        #endregion

        #region Метод [GetAll]: Получение KillerPerkCategoties

        public async Task GetAll()
        {
            var killerPerkCategories = await _killerPerkCategoryService.GetAllAsync();

            foreach (var killerPerkCategory in killerPerkCategories)
                KillerPerkCategories.Add(new KillerPerkCategoryViewModel(killerPerkCategory));
        }

        #endregion
    }
}