using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace DBDAnalytics.WPF.ViewModels.WindowVM
{
    internal class InteractionRoleVM : BaseVM, IUpdatable
    {
        private readonly IRoleService _roleService;
        private readonly IWindowNavigationService _windowNavigationService;

        public InteractionRoleVM(IWindowNavigationService windowNavigationService, IRoleService roleService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _roleService = roleService;

            GetRoles();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {

        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекции

        public ObservableCollection<RoleDTO> Roles { get; private set; } = [];

        #endregion

        #region Свойство : Title

        public string Title { get; set; } = "Игровые роли";

        #endregion

        #region Свойства : Selected | RoleName | RoleDescription

        private RoleDTO _selectedRole;
        public RoleDTO SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (_selectedRole != value)
                {
                    _selectedRole = value;

                    RoleName = value?.RoleName;
                    RoleDescription = value?.RoleDescription;

                    OnPropertyChanged();
                }
            }
        }

        private string _roleName;
        public string RoleName
        {
            get => _roleName;
            set
            {
                _roleName = value;
                OnPropertyChanged();
            }
        }

        private string _roleDescription;
        public string RoleDescription
        {
            get => _roleDescription;
            set
            {
                _roleDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion 

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Добавление | Удаление | Обновление записей

        private RelayCommand _addRoleCommand;
        public RelayCommand AddRoleCommand { get => _addRoleCommand ??= new(obj => { AddRole(); }); }

        private RelayCommand _deleteRoleCommand;
        public RelayCommand DeleteRoleCommand { get => _deleteRoleCommand ??= new(obj => { DeleteRole(); }); }

        private RelayCommand _updateRoleCommand;
        public RelayCommand UpdateRoleCommand { get => _updateRoleCommand ??= new(obj => { UpdateRole(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        private async void GetRoles()
        {
            var roles = await _roleService.GetAllAsync();

            foreach (var role in roles)
                Roles.Add(role);
        }

        #region CRUD

        // TODO : Изменить MessageBox на кастомное окно
        private async void AddRole()
        {
            var newRoleDTO = await _roleService.CreateAsync(RoleName, RoleDescription);

            if (newRoleDTO.Message != string.Empty)
            {
                MessageBox.Show(newRoleDTO.Message);
                return;
            }
            else
            {
                NotificationTransmittingValue(WindowName.InteractionOffering, newRoleDTO.RoleDTO, TypeParameter.AddAndNotification);
                Roles.Add(newRoleDTO.RoleDTO);
            }
                
        }

        private async void UpdateRole()
        {
            if (SelectedRole == null)
                return;

            var (RoleDTO, Message) = await _roleService.UpdateAsync(SelectedRole.IdRole, RoleName, RoleDescription);

            if (Message == string.Empty)
            {
                NotificationTransmittingValue(WindowName.InteractionOffering, RoleDTO, TypeParameter.UpdateAndNotification);
                Roles.ReplaceItem(SelectedRole, RoleDTO);
            }
            else
            {
                if (MessageBox.Show(Message + "Вы точно хотите произвести обновление?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var forcedRoleDTO = await _roleService.ForcedUpdateAsync(SelectedRole.IdRole, RoleName, RoleDescription);
                    NotificationTransmittingValue(WindowName.InteractionOffering, forcedRoleDTO, TypeParameter.UpdateAndNotification);
                    Roles.ReplaceItem(SelectedRole, forcedRoleDTO);
                }
            }
        }

        private async void DeleteRole()
        {
            if (SelectedRole == null)
                return;

            if (MessageBox.Show("Вы точно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var (IsDeleted, Message) = await _roleService.DeleteAsync(SelectedRole.IdRole);

                if (IsDeleted == true)
                {
                    NotificationTransmittingValue(WindowName.InteractionOffering, SelectedRole.IdRole, TypeParameter.DeleteAndNotification);
                    Roles.Remove(SelectedRole);
                }
                else
                    MessageBox.Show(Message);
            }
        }

        #endregion
    }
}