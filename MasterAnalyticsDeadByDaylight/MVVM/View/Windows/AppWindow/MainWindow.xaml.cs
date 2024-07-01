using MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.WindowNavigation;
using System.Windows;
using System.Windows.Navigation;

namespace MasterAnalyticsDeadByDaylight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StateChanged += MainWindowStateChangeRaised;
            DataContext = new MainWindowViewModel(new PageNavigationService(MainFrame));
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