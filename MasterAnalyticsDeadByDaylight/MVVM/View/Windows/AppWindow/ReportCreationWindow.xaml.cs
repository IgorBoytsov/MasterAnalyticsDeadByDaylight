using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow
{
    /// <summary>
    /// Логика взаимодействия для ReportCreationWindow.xaml
    /// </summary>
    public partial class ReportCreationWindow : Window
    {
        public ReportCreationWindow()
        {
            InitializeComponent();
            DataContext = new ReportCreationWindowViewModel();
        }
    }
}
