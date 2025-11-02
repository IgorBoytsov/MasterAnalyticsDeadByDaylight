using DBDAnalytics.UserService.Client.ApiClients;
using DBDAnalytics.UserService.Client.Models.Requests;
using DBDAnalytics.UserService.Client.Services;
using Shared.WPF.Commands;
using Shared.WPF.Navigations.Pages;
using Shared.WPF.Navigations.Windows;
using Shared.WPF.ViewModels.Base;
using System.Windows;

namespace DBDAnalytics.Client.WPF.ViewModels.Windows
{
    internal sealed class MainWindowViewModel : BaseWindowViewModel
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public MainWindowViewModel(
        IWindowNavigation windowNavigation,
        IPageNavigation pageNavigation,
        IUserService userService,
        IAuthorizationService authorizationService) : base(windowNavigation, pageNavigation)
        {
            _userService = userService;
            _authorizationService = authorizationService;

            InitializeCommand();
        }

        /*--Коллекции-------------------------------------------------------------------------------------*/

        /*--Свойства--------------------------------------------------------------------------------------*/

        private string? _authPassword;
        public string? AuthPassword
        {
            get => _authPassword;
            set => SetProperty(ref _authPassword, value);
        }

        private string? _autLogin;
        public string? AuthLogin
        {
            get => _autLogin;
            set => SetProperty(ref _autLogin, value);
        }

        private string? _autEmail;
        public string? AuthEmail
        {
            get => _autEmail;
            set => SetProperty(ref _autEmail, value);
        }

        private Visibility _menuVisibility = Visibility.Collapsed;
        public Visibility MenuVisibility
        {
            get => _menuVisibility;
            set => SetProperty(ref _menuVisibility, value);
        }

        private Visibility _menuAuthVisibility = Visibility.Visible;
        public Visibility MenuAuthVisibility
        {
            get => _menuAuthVisibility;
            set => SetProperty(ref _menuAuthVisibility, value);
        }

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            LoginCommandAsync = new RelayCommandAsync(Execute_LoginCommandAsync, CanExecute_LoginCommandAsync);
        }

        #region Команда [LoginCommandAsync]: Аунтетификация пользователя

        public RelayCommandAsync? LoginCommandAsync { get; private set;}

        private async Task Execute_LoginCommandAsync()
        {
            //var loginUserRequest = new LoginUserRequest(AuthPassword!, AuthLogin!, AuthEmail!);

            var result = await _authorizationService.AuthenticationAsync(AuthLogin!, AuthPassword!, AuthEmail!);

            if (!result.IsSuccess)
            {
                MessageBox.Show($"Ошибки при входе {result.StringMessage}");
                return;
            }

            result.Switch(
                onSuccess: user => 
                {
                    MenuVisibility = Visibility.Visible;
                    MenuAuthVisibility = Visibility.Collapsed;
                },
                onFailure: errors => MessageBox.Show($"Ошибки при входе {errors[0].Message}"));
        }

        //private bool CanExecute_LoginCommandAsync() => !string.IsNullOrWhiteSpace(AuthPassword) && !string.IsNullOrWhiteSpace(AuthLogin) && !string.IsNullOrWhiteSpace(AuthEmail);
        private bool CanExecute_LoginCommandAsync() => true;
       
        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/
    }
}