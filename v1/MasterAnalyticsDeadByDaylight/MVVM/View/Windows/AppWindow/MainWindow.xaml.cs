using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            StateChanged += MainWindowStateChangeRaised;

            var pageNavigation = serviceProvider.GetService<IPageNavigationService>();
            pageNavigation.SetFrame(MainFrame);

            DataContext = viewModel;
        }

        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }
    }
}