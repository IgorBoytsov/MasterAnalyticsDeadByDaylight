using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Navigations.Pages;
using Shared.WPF.Navigations.Windows;

namespace Shared.WPF.ViewModels.Base
{
    public abstract class BaseWindowViewModel : BaseViewModel
    {
        private readonly IWindowNavigation? _windowNavigation;
        private readonly IPageNavigation? _pageNavigation;

        public BaseWindowViewModel() { }

        public BaseWindowViewModel(IWindowNavigation windowNavigation, IPageNavigation pageNavigation)
        {
            _windowNavigation = windowNavigation;
            _pageNavigation = pageNavigation;

            SetValueCommands();
        }

        private string _windowTitle = null!;
        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        private PagesName _currentOpenPage;
        public PagesName CurrentOpenPage
        {
            get => _currentOpenPage;
            set
            {
                SetProperty(ref _currentOpenPage, value);
                OpenPageCommand?.RaiseCanExecuteChanged();
            }
        }

        /*--Команды---------------------------------------------------------------------------------------*/

        private void SetValueCommands()
        {
            OpenPageCommand = new RelayCommand<PagesName>(Execute_OpenPageCommand, CanExecute_OpenPageCommand);

            ShutDownAppCommand = new RelayCommand(Execute_ShutDownAppCommand);

            MinimizeWindowCommand = new RelayCommand<WindowsName>(Execute_MinimizeWindowCommand);
            MaximizeWindowCommand = new RelayCommand<WindowsName>(Execute_MaximizeWindowCommand);
            RestoreWindowCommand = new RelayCommand<WindowsName>(Execute_RestoreWindowCommand);
            CloseWindowCommand = new RelayCommand<WindowsName>(Execute_CloseWindowCommand);
        }

        #region Открытия страницы

        public RelayCommand<PagesName>? OpenPageCommand { get; private set; }

        private void Execute_OpenPageCommand(PagesName page)
        {
            _pageNavigation?.Navigate(page, FramesName.MainFrame);
            CurrentOpenPage = _pageNavigation!.GetCurrentDisplayedPage(FramesName.MainFrame);
        }

        private bool CanExecute_OpenPageCommand(PagesName page) => CurrentOpenPage != page;

        #endregion

        #region ShutDownApp

        public RelayCommand? ShutDownAppCommand { get; private set; }

        private void Execute_ShutDownAppCommand() => System.Windows.Application.Current.Shutdown();

        #endregion

        #region Minimize

        public RelayCommand<WindowsName>? MinimizeWindowCommand { get; private set; }

        private void Execute_MinimizeWindowCommand(WindowsName windowName) => _windowNavigation!.MinimizeWindow(windowName);

        #endregion

        #region Maximize

        public RelayCommand<WindowsName>? MaximizeWindowCommand { get; private set; }

        private void Execute_MaximizeWindowCommand(WindowsName windowName) => _windowNavigation!.MaximizeWindow(windowName);

        #endregion

        #region Restore

        public RelayCommand<WindowsName>? RestoreWindowCommand { get; private set; }

        private void Execute_RestoreWindowCommand(WindowsName windowName) => _windowNavigation!.RestoreWindow(windowName);

        #endregion

        #region Close

        public RelayCommand<WindowsName>? CloseWindowCommand { get; private set; }

        private void Execute_CloseWindowCommand(WindowsName windowName) => _windowNavigation!.Close(windowName);

        #endregion

    }
}