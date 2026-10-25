using DBDAnalytics.UserService.Client.ApiClients;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Navigations.Pages;
using Shared.WPF.Navigations.Windows;
using Shared.WPF.Services.Initializers;
using Shared.WPF.ViewModels.Base;
using System.Windows;

namespace DBDAnalytics.Client.WPF.ViewModels.Windows
{
    internal sealed class MainWindowViewModel : BaseWindowViewModel, IAsyncInitializable
    {
        private readonly IAuthorizationService _authorizationService;

        public MainWindowViewModel(
        IWindowNavigation windowNavigation,
        IPageNavigation pageNavigation,
        IAuthorizationService authorizationService) : base(windowNavigation, pageNavigation)
        {
            //_authService = authService;
            _authorizationService = authorizationService;

            InitializeCommand();
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            var result = await _authorizationService.AuthenticationByTokenAsync(AppType.Client);

            result.Switch(onSuccess: () =>
            {
                MenuVisibility = Visibility.Visible;
                MenuAuthVisibility = Visibility.Collapsed;
            }, onFailure: errors => MessageBox.Show($"Действие сессии окончено. Вам нужно снова зайти в учетную запись."));

            IsInitialize = true;
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
             MenuAuthVisibility = Visibility.Collapsed;
            // var result = await _authorizationService.AuthenticationAsync(AuthLogin!, AuthPassword!, AppType.Client);

            // result.Switch(
            //     onSuccess: user => 
            //     {
            //         MenuVisibility = Visibility.Visible;
            //         MenuAuthVisibility = Visibility.Collapsed;
            //     },
            //     onFailure: errors => MessageBox.Show($"Ошибки при входе {errors[0].Message}"));
        }

        private bool CanExecute_LoginCommandAsync() => true;

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/
    }
}