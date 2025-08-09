using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DBDAnalytics.WPF.ViewModels
{
    internal class BaseVM : INotifyPropertyChanged
    {
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IPageNavigationService _pageNavigationService;

        public BaseVM(IWindowNavigationService windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
        }        
        
        public BaseVM(IWindowNavigationService windowNavigationService, IPageNavigationService pageNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _pageNavigationService = pageNavigationService;
        }

        public BaseVM()
        {
        }

        #region Команды : Управление WindowState окон

        private RelayCommand _minimizeWindowCommand;
        public RelayCommand MinimizeWindowCommand => _minimizeWindowCommand ??= new RelayCommand(MinimizeWindow);

        private RelayCommand _maximizeWindowCommand;
        public RelayCommand MaximizeWindowCommand => _maximizeWindowCommand ??= new RelayCommand(MaximizeWindow);

        private RelayCommand _restoreWindowCommand;
        public RelayCommand RestoreWindowCommand => _restoreWindowCommand ??= new RelayCommand(RestoreWindow);

        private RelayCommand _closeWindowCommand;
        public RelayCommand CloseWindowCommand => _closeWindowCommand ??= new RelayCommand(CloseWindow);

        private RelayCommand _shouDownAppCommand;
        public RelayCommand ShouDownAppCommand => _shouDownAppCommand ??= new RelayCommand(ShutDownApp);

        #endregion

        #region Методы : Управление WindowState окон
 
        private void MinimizeWindow(object parameter)
        {
            if (parameter is WindowName windowName)
            {
                _windowNavigationService.MinimizeWindow(windowName);
            }
        }

        private void MaximizeWindow(object parameter)
        {
            if (parameter is WindowName windowName)
            {
                _windowNavigationService.MaximizeWindow(windowName);
            }
        }

        private void RestoreWindow(object parameter)
        {
            if (parameter is WindowName windowName)
            {
                _windowNavigationService.RestoreWindow(windowName);
            }
        }

        private void CloseWindow(object parameter)
        {
            if (parameter is WindowName windowName)
            {
                _windowNavigationService.CloseWindow(windowName);
            }
        }

        private void ShutDownApp(object parameter)
        {
            _windowNavigationService.ShutDownApp();
        }

        #endregion

        #region Метод : Уведомления о изменение значений

        public void NotificationTransmittingValue<T>(WindowName windowName, T value, TypeParameter typeParameter)
        {
            _windowNavigationService.TransmittingValue(windowName, value, typeParameter);
        }

        public void NotificationTransmittingValue<T>(PageName pageName, FrameName frameName, T value, TypeParameter typeParameter)
        {
            _pageNavigationService.TransmittingValue(pageName, frameName, value, typeParameter);
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}