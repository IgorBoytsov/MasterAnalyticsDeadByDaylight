using DBDAnalytics.AdminPanel.WPF.ViewModels.Windows;
using DBDAnalytics.AdminPanel.WPF.Views.Windows;
using Shared.WPF.Enums;
using Shared.WPF.Navigations.Pages;
using Shared.WPF.Navigations.Windows;
using System.Windows;
using System.Windows.Controls;

namespace DBDAnalytics.AdminPanel.WPF.Factories.Windows
{
    internal sealed class MainWindowFactory(
        Func<MainWindowViewModel> vmFactory, 
        IPageNavigation pageNavigation) : IWindowFactory
    {
        private readonly Func<MainWindowViewModel> _vmFactory = vmFactory;
        private readonly IPageNavigation _pageNavigation = pageNavigation;

        public Window CreateWindow()
        {
            var vm = _vmFactory();

            var window = new MainWindow { DataContext = vm };

            window.Loaded += (sender, args) =>
            {
                var mainFrame = window.FindName("MainFrame") as Frame; 
                _pageNavigation.RegisterFrame(FramesName.MainFrame, mainFrame);
                //_pageNavigation.Navigate(PagesName.GameMode, FramesName.MainFrame);
            };

            return window;
        }
    }
}