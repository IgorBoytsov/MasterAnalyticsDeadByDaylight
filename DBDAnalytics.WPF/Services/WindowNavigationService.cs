using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace DBDAnalytics.WPF.Services
{
    internal class WindowNavigationService : IWindowNavigationService
    {
        private Dictionary<WindowName, Window> _windows = [];
        private readonly Dictionary<string, IWindowFactory> _windowFactories = [];

        public WindowNavigationService(IEnumerable<IWindowFactory> windowFactories)
        {
            _windowFactories = windowFactories.ToDictionary(f => f.GetType().Name.Replace("Factory", ""), f => f);
        }

        public void OpenWindow(WindowName windowName, object parameter = null, TypeParameter typeParameter = TypeParameter.None, bool IsOpenDialog = false)
        {
            if (_windows.TryGetValue(windowName, out var windowExist))
            {
                if (windowExist.DataContext is IUpdatable viewModel)
                {
                    viewModel.Update(parameter, typeParameter);
                }

                windowExist.WindowState = WindowState.Normal;
                windowExist.Activate();
                return;
            }
            Open(windowName, parameter, typeParameter, IsOpenDialog);
        }

        private void Open(WindowName windowName, object parameter = null, TypeParameter typeParameter = TypeParameter.None, bool IsOpenDialog = false)
        {
            if (_windowFactories.TryGetValue(windowName.ToString(), out var factory))
            {
                var window = factory.CreateWindow(parameter);

                _windows[windowName] = window;

                window.Closed += (c, e) => _windows.Remove(windowName);
                window.StateChanged += MainWindowStateChangeRaised;

                if (!IsOpenDialog)
                    window.Show();
                else
                    window.ShowDialog();
            }
            else
            {
                throw new Exception($"Такое окно не зарегистрировано {windowName}");
            }
        }

        public void TransmittingValue(WindowName windowName, object parameter = null, TypeParameter typeParameter = TypeParameter.None)
        {
            if (_windows.TryGetValue(windowName, out var windowExist))
            {
                if (windowExist.DataContext is IUpdatable viewModel)
                {
                    viewModel.Update(parameter, typeParameter);
                }

                //windowExist.Activate();
            }
        }

        #region WindowStat методы

        public void CloseWindow(WindowName windowName)
        {
            if (_windows.TryGetValue(windowName, out var windowExist))
            {
                windowExist.Close();
            }
        }

        public void MinimizeWindow(WindowName windowName)
        {
            if (_windows.TryGetValue(windowName, out var windowExist))
            {
                SystemCommands.MinimizeWindow(windowExist);
            }
        }

        public void MaximizeWindow(WindowName windowName)
        {
            if (_windows.TryGetValue(windowName, out var windowExist))
            {
                SystemCommands.MaximizeWindow(windowExist);
            }
        }

        public void RestoreWindow(WindowName windowName)
        {
            if (_windows.TryGetValue(windowName, out var windowExist))
            {
                SystemCommands.RestoreWindow(windowExist);
            }
        }

        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (sender is Window window)
            {
                var mainWindowBorder = window.FindName("MainWindowBorder") as Border;
                var restoreButton = window.FindName("RestoreButton") as Button;
                var maximizeButton = window.FindName("MaximizeButton") as Button;

                if (window.WindowState == WindowState.Maximized)
                {
                    mainWindowBorder.BorderThickness = new Thickness(8);
                    restoreButton.Visibility = Visibility.Visible;
                    maximizeButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    mainWindowBorder.BorderThickness = new Thickness(0);
                    restoreButton.Visibility = Visibility.Collapsed;
                    maximizeButton.Visibility = Visibility.Visible;
                }
            }
        }

        #endregion

        public void ShutDownApp()
        {
            App.Current.Shutdown();
        }
    }
}