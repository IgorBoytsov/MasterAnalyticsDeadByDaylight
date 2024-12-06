using MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.Services.NavigationService.WindowNavigation
{
    public class WindowNavigationService : IWindowNavigationService
    {
        private Dictionary<string, Window> _windows = new();

        private readonly IServiceProvider _serviceProvider;

        public WindowNavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void OpenWindow(string windowName, object parameter = null)
        {
            if (_windows.TryGetValue(windowName, out var windowExist))
            {
                if (windowExist.DataContext is IUpdatable viewModel)
                {
                    viewModel.Update(parameter);
                    windowExist.Activate();
                }
                return;
            }
            Open(windowName, parameter);
        }

        private void Open(string windowName, object parameter = null)
        {
            Action action = windowName switch
            {
                "AboutTheProgramWindow" => () =>
                {
                    var viewModel = new AboutTheProgramWindowViewModel(_serviceProvider);
                    var window = new AboutTheProgramWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show ();
                }
                ,
                "AddAdditionalDataWindow" => () =>
                {
                    var viewModel = new AddAdditionalDataWindowViewModel(_serviceProvider);
                    var window = new AddAdditionalDataWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show();
                }
                ,
                "AddItemWindow" => () =>
                {
                    var viewModel = new AddItemWindowViewModel(_serviceProvider);
                    var window = new AddItemWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show();
                }
                ,
                "AddKillerWindow" => () =>
                {
                    var viewModel = new AddKillerWindowViewModel(_serviceProvider);
                    var window = new AddKillerWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show();
                }
                ,
                "AddMapWindow" => () =>
                {
                    var viewModel = new AddMapWindowViewModel(_serviceProvider);
                    var window = new AddMapWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show();
                }
                ,
                "AddMatchWindow" => () =>
                {
                    var viewModel = new AddMatchWindowViewModel(_serviceProvider);
                    var window = new AddMatchWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show();
                }
                ,
                "AddOfferingWindow" => () =>
                {
                    var viewModel = new AddOfferingWindowViewModel(_serviceProvider);
                    var window = new AddOfferingWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show();
                }
                ,
                "AddPerkWindow" => () =>
                {
                    var viewModel = new AddPerkWindowViewModel(_serviceProvider);
                    var window = new AddPerkWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show();
                }
                ,
                "AddSurvivorWindow" => () =>
                {
                    var viewModel = new AddSurvivorWindowViewModel(_serviceProvider);
                    var window = new AddSurvivorWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show();
                }
                ,
                "DataBackupWindow" => () =>
                {
                    var viewModel = new DataBackupWindowViewModel(_serviceProvider);
                    var window = new DataBackupWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show();
                }
                ,
                "HowToUseWindow" => () =>
                {
                    var viewModel = new HowToUseWindowViewModel(_serviceProvider);
                    var window = new HowToUseWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show();
                }
                ,
                "ReportCreationWindow" => () =>
                {
                    var viewModel = new ReportCreationWindowViewModel(_serviceProvider);
                    var window = new ReportCreationWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show();
                }
                ,
                "ShowDetailsMatchWindow" => () =>
                {
                    var viewModel = new ShowDetailsMatchWindowViewModel(_serviceProvider);
                    var window = new ShowDetailsMatchWindow()
                    {
                        DataContext = viewModel,
                    };

                    _windows[windowName] = window;
                    window.Closed += (s, e) => _windows.Remove(windowName);
                    window.Show();
                }
                ,
                _ => () => throw new Exception("Окно отсутствует")
            };
            action?.Invoke();
        }
    }
}
