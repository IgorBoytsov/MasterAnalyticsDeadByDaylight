using DBDAnalytics.AdminPanel.WPF.ViewModels.Components;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Role;
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
    internal sealed class RolePageViewModel : BasePageViewModel, IAsyncInitializable
    {
        private readonly IRoleService _roleService;

        public RolePageViewModel(IRoleService roleService)
        {
            _roleService = roleService;

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

        public ObservableCollection<RoleViewModel> Roles { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IPopupController EditPopup { get; } = new PopupController();

        #region Свойство: [SelectedRole] Метод: [OnSelectedRolePropertyChanged]

        private RoleViewModel? _selectedRole;
        public RoleViewModel? SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (_selectedRole is not null)
                    _selectedRole.PropertyChanged -= OnSelectedRolePropertyChanged;

                SetProperty(ref _selectedRole, value);

                if (_selectedRole is not null)
                    _selectedRole.PropertyChanged += OnSelectedRolePropertyChanged;

                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnSelectedRolePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateCommandAsync?.RaiseCanExecuteChanged();
            RevertChangeCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Свойство: [NewRole] Метод: [OnNewRolePropertyChanged] 

        private RoleViewModel? _newRole;
        public RoleViewModel? NewRole
        {
            get => _newRole;
            private set
            {
                if (_newRole is not null)
                    _newRole.PropertyChanged -= OnNewRolePropertyChanged;

                SetProperty(ref _newRole, value);

                if (_newRole is not null)
                    _newRole.PropertyChanged += OnNewRolePropertyChanged;

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void OnNewRolePropertyChanged(object? sender, PropertyChangedEventArgs e) => CreateCommandAsync?.RaiseCanExecuteChanged();

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync<RoleViewModel>(Execute_UpdateCommandAsync, CanExecute_UpdateCommand);
            DeleteCommandAsync = new RelayCommandAsync<RoleViewModel>(Execute_DeleteCommandAsync, CanExecute_DeleteCommand);
            RevertChangeCommand = new RelayCommand<RoleViewModel>(Execute_RevertChangeCommand, CanExecute_RevertChangeCommand);
        }

        #region Команда [CreateCommandAsync]: Создание Role

        public RelayCommandAsync? CreateCommandAsync { get; private set; }

        private async Task Execute_CreateCommandAsync()
        {
            if (NewRole is null)
                return;

            IsBusy = true;

            try
            {
                var newModel = NewRole.ToModel();

                var response = await _roleService.AddAsync(newModel);

                if (response is null)
                {
                    MessageBox.Show("Не удалось добавить запись");
                    return;
                }

                Roles.Add(new RoleViewModel(response.Value!));

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

        private bool CanExecute_CreateCommandAsync() => NewRole != null && !string.IsNullOrWhiteSpace(NewRole.Name) && NewRole.OldId >= 0;

        #endregion

        #region Команда [UpdateCommandAsync]: Обновление Role

        public RelayCommandAsync<RoleViewModel>? UpdateCommandAsync { get; private set; }

        private async Task Execute_UpdateCommandAsync(RoleViewModel model)
        {
            if (model is null)
                return;

            IsBusy = true;

            try
            {
                var result = await _roleService.UpdateAsync(model.Id, new UpdateRoleRequest(model.Name));

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

        private bool CanExecute_UpdateCommand(RoleViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        #region Команда [DeleteCommandAsync]: Удаление Role

        public RelayCommandAsync<RoleViewModel>? DeleteCommandAsync { get; private set; }

        private async Task Execute_DeleteCommandAsync(RoleViewModel model)
        {
            if (model is null)
                return;

            if (MessageBox.Show($"Вы точно хотите удалить {model.Name}?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsBusy = true;

                try
                {
                    var result = await _roleService.DeleteAsync(model.Id);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"Ошибка удаления: {result.StringMessage}");
                        return;
                    }

                    Roles.Remove(model);
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

        private bool CanExecute_DeleteCommand(RoleViewModel model) => model is not null;

        #endregion

        #region Команда [RevertChangeCommand]: Откат изменений

        public RelayCommand<RoleViewModel>? RevertChangeCommand { get; private set; }

        private void Execute_RevertChangeCommand(RoleViewModel model) => model?.RevertChanges();

        private bool CanExecute_RevertChangeCommand(RoleViewModel model)
        {
            if (model is not null)
                return model.HasChanges;

            return false;
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод [PrepareNew]: Создание пустого RoleViewModel для будущего добавление

        private void PrepareNew() => NewRole = new RoleViewModel(RoleResponse.Empty);

        #endregion

        #region Метод [GetAll]: Получение GameEvents

        public async Task GetAll()
        {
            var roles = await _roleService.GetAllAsync();

            foreach (var role in roles)
                Roles.Add(new RoleViewModel(role));
        }

        #endregion
    }
}