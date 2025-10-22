using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Rarity;
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
    internal sealed class RarityPageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IRarityService _rarityService;

        public RarityPageViewModel(IRarityService rarityService)
        {
            _rarityService = rarityService;

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

        public ObservableCollection<RarityViewModel> Rarities { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        #region Свойство: [SelectedRarity] Метод: [OnSelectedRarityPropertyChanged]

        private RarityViewModel? _selectedRarity;
        public RarityViewModel? SelectedRarity
        {
            get => _selectedRarity;
            set
            {
                if (_selectedRarity is not null)
                    _selectedRarity.PropertyChanged -= OnSelectedRarityPropertyChanged;

                SetProperty(ref _selectedRarity, value);

                if (_selectedRarity is not null)
                    _selectedRarity.PropertyChanged += OnSelectedRarityPropertyChanged;

                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnSelectedRarityPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewRarity] Метод: [OnNewRarityPropertyChanged] 

        private RarityViewModel? _newRarity;
        public RarityViewModel? NewRarity
        {
            get => _newRarity;
            private set
            {
                if (_newRarity is not null)
                    _newRarity.PropertyChanged -= OnNewRarityPropertyChanged;

                SetProperty(ref _newRarity, value);

                if (_newRarity is not null)
                    _newRarity.PropertyChanged += OnNewRarityPropertyChanged;

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnNewRarityPropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<RarityViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommand);
            DeleteCommandAsync = new RelayCommandAsync<RarityViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommand);
            RevertChangeCommand = new RelayCommand<RarityViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);
        }

        #region Команда [CreateCommandAsync]: Создание Rarity

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewRarity is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewRarity.ToModel();

                var result = await _rarityService.AddAsync(newModel);

                if (!result.IsSuccess)
                {
                    MessageBox.Show("Не удалось добавить запись");
                    return;
                }

                Rarities.Add(new RarityViewModel(result.Value!));

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

        private bool CanExecute_CreateCommandAsync() => NewRarity != null && !string.IsNullOrWhiteSpace(NewRarity.Name) && NewRarity.OldId >= 0;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление Rarity

        public RelayCommandAsync<RarityViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(RarityViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _rarityService.UpdateAsync(model.Id, new UpdateRarityRequest(model.Name));

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

        private bool CanExecute_UpdateCommand(RarityViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление Rarity

        public RelayCommandAsync<RarityViewModel>? DeleteCommandAsync { get; private set; }

        private async Task Execute_DeleteCommandAsync(RarityViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _rarityService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    Rarities.Remove(model);
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

        private bool CanExecute_DeleteCommand(RarityViewModel model) => model is not null;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<RarityViewModel>? RevertChangeCommand { get; private set; }

        private void Execute_RevertChangeCommand(RarityViewModel model) => model?.RevertChanges();

        private bool CanExecute_RevertChangeCommand(RarityViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [PrepareNew]: Создание пустого RarityViewModel для будущего добавление

        private void PrepareNew() => NewRarity = new RarityViewModel(RarityResponse.Empty);

        #endregion

        #region Метод [GetAll]: Получение GameEvents

        public async Task GetAll()
        {
            var gameEvents = await _rarityService.GetAllAsync();

            foreach (var gameEvent in gameEvents)
                Rarities.Add(new RarityViewModel(gameEvent));
        }

        #endregion
    }
}