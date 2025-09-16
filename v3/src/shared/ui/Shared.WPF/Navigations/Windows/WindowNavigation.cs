using Shared.WPF.Enums;
using Shared.WPF.ViewModels.Base;
using System.Windows;
using System.Windows.Controls;

namespace Shared.WPF.Navigations.Windows
{
    public sealed class WindowNavigation : IWindowNavigation
    {
        private Dictionary<WindowsName, Window> _windows = [];
        private readonly Dictionary<string, IWindowFactory> _windowFactories = [];

        public WindowNavigation(IEnumerable<IWindowFactory> windowFactories)
        {
            _windowFactories = windowFactories.ToDictionary(f => f.GetType().Name.Replace("Factory", ""), f => f);
        }

        public void Open(WindowsName name, bool isOpenDialog = false)
        {
            if (_windows.TryGetValue(name, out Window? wnd) && wnd is not null)
                (isOpenDialog ? () => { wnd.ShowDialog(); } : (Action)wnd.Show)();
            else
                OpenWindow(name, isOpenDialog);
        }

        private void OpenWindow(WindowsName windowName, bool isOpenDialog = false)
        {
            if (_windowFactories.TryGetValue(windowName.ToString(), out var factory))
            {
                var window = factory.CreateWindow();

                _windows[windowName] = window;

                window.Closed += (c, e) => _windows.Remove(windowName);
                window.StateChanged += MainWindowStateChangeRaised!;

                (isOpenDialog ? () => { window.ShowDialog(); } : (Action)window.Show)();
            }
            else
                throw new Exception($"Такое окно не зарегистрировано {windowName}");
        }

        public void TransmittingValue<TData>(WindowsName windowName, TData value, TransmittingParameter parameter = TransmittingParameter.None, bool isActive = false)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
            {
                if (window.DataContext is IUpdatable viewModel)
                {
                    viewModel.Update<TData>(value, parameter);

                    if (isActive)
                        window.Activate();
                }
            }
        }

        public void Close(WindowsName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
                window.Close();
        }

        public void MinimizeWindow(WindowsName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
                SystemCommands.MinimizeWindow(window);
        }

        public void MaximizeWindow(WindowsName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
                SystemCommands.MaximizeWindow(window);
        }

        public void RestoreWindow(WindowsName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
                SystemCommands.RestoreWindow(window);
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
                    mainWindowBorder!.BorderThickness = new Thickness(8);
                    restoreButton!.Visibility = Visibility.Visible;
                    maximizeButton!.Visibility = Visibility.Collapsed;
                }
                else
                {
                    mainWindowBorder!.BorderThickness = new Thickness(0);
                    restoreButton!.Visibility = Visibility.Collapsed;
                    maximizeButton!.Visibility = Visibility.Visible;
                }
            }
        }
    }
}