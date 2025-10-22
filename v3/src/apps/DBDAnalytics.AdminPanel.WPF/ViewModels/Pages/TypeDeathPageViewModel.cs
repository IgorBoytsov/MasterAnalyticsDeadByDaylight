using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.TypeDeath;
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
    internal sealed class TypeDeathPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly ITypeDeathService _typeDeathService;

        public TypeDeathPageViewModel(ITypeDeathService typeDeathService)
        {
            _typeDeathService = typeDeathService;

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

        public ObservableCollection<TypeDeathViewModel> TypeDeaths { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        #region Свойство: [SelectedTypeDeath] Метод: [OnSelectedTypeDeathPropertyChanged]

        private TypeDeathViewModel? _selectedTypeDeath;
        public TypeDeathViewModel? SelectedTypeDeath
        {
            get => _selectedTypeDeath;
            set
            {
                if (_selectedTypeDeath is not null)
                    _selectedTypeDeath.PropertyChanged -= OnSelectedTypeDeathPropertyChanged;

                SetProperty(ref _selectedTypeDeath, value);

                if (_selectedTypeDeath is not null)
                    _selectedTypeDeath.PropertyChanged += OnSelectedTypeDeathPropertyChanged;

                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnSelectedTypeDeathPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewTypeDeath] Метод: [OnNewTypeDeathPropertyChanged] 

        private TypeDeathViewModel? _newTypeDeath;
        public TypeDeathViewModel? NewTypeDeath
        {
            get => _newTypeDeath;
            private set
            {
                if (_newTypeDeath is not null)
                    _newTypeDeath.PropertyChanged -= OnNewTypeDeathPropertyChanged;

                SetProperty(ref _newTypeDeath, value);

                if (_newTypeDeath is not null)
                    _newTypeDeath.PropertyChanged += OnNewTypeDeathPropertyChanged;

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnNewTypeDeathPropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<TypeDeathViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommand);
            DeleteCommandAsync = new RelayCommandAsync<TypeDeathViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommand);
            RevertChangeCommand = new RelayCommand<TypeDeathViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);
        }

        #region Команда [CreateCommandAsync]: Создание TypeDeath

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewTypeDeath is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewTypeDeath.ToModel();

                var result = await _typeDeathService.AddAsync(newModel);

                if (!result.IsSuccess)
                {
                    MessageBox.Show("Не удалось добавить запись");
                    return;
                }

                TypeDeaths.Add(new TypeDeathViewModel(result.Value!));

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

        private bool CanExecute_CreateCommandAsync() => NewTypeDeath != null && !string.IsNullOrWhiteSpace(NewTypeDeath.Name) && NewTypeDeath.OldId >= 0;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление TypeDeath

        public RelayCommandAsync<TypeDeathViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(TypeDeathViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _typeDeathService.UpdateAsync(model.Id, new UpdateTypeDeathRequest(model.Name));

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

        private bool CanExecute_UpdateCommand(TypeDeathViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление TypeDeath

        public RelayCommandAsync<TypeDeathViewModel>? DeleteCommandAsync { get; private set; }

        private async Task Execute_DeleteCommandAsync(TypeDeathViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _typeDeathService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    TypeDeaths.Remove(model);
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

        private bool CanExecute_DeleteCommand(TypeDeathViewModel model) => model is not null;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<TypeDeathViewModel>? RevertChangeCommand { get; private set; }

        private void Execute_RevertChangeCommand(TypeDeathViewModel model) => model?.RevertChanges();

        private bool CanExecute_RevertChangeCommand(TypeDeathViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [PrepareNew]: Создание пустого TypeDeathViewModel для будущего добавление

        private void PrepareNew() => NewTypeDeath = new TypeDeathViewModel(TypeDeathResponse.Empty);

        #endregion

        #region Метод [GetAll]: Получение TypeDeaths

        public async Task GetAll()
        {
            var typeDeaths = await _typeDeathService.GetAllAsync();

            foreach (var typeDeath in typeDeaths)
                TypeDeaths.Add(new TypeDeathViewModel(typeDeath));
        }

        #endregion
    }
}