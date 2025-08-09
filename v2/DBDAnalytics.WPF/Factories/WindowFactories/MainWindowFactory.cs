using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.ViewModels.WindowVM;
using DBDAnalytics.WPF.Views.Windows;
using System.Windows;
using System.Windows.Controls;

namespace DBDAnalytics.WPF.Factories.WindowFactories
{
    internal class MainWindowFactory(Func<MainWindowVM> viewModelFactory, IPageNavigationService pageNavigationService) : IWindowFactory
    {
        private readonly Func<MainWindowVM> _viewModelFactory = viewModelFactory;
        private readonly IPageNavigationService _pageNavigationService = pageNavigationService;

        public Window CreateWindow(object parameter = null, TypeParameter typeParameter = TypeParameter.None)
        {
            var viewModel = _viewModelFactory();
            viewModel.Update(parameter, typeParameter);
            var window = new MainWindow { DataContext = viewModel };

            window.Loaded += (sender, args) =>
            {
                var mainFrame = window.FindName("MainFrame") as Frame;
                _pageNavigationService.RegisterFrame(FrameName.MainFrame, mainFrame);
                _pageNavigationService.Navigate(PageName.DashBoard, FrameName.MainFrame);
            };

            return window;
        }
    }
}