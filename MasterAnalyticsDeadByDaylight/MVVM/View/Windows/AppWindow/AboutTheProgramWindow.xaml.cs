using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow
{
    /// <summary>
    /// Логика взаимодействия для AboutTheProgramWindow.xaml
    /// </summary>
    public partial class AboutTheProgramWindow : Window
    {
        public AboutTheProgramWindow()
        {
            InitializeComponent();
            DataContext = new AboutTheProgramWindowViewModel();
        }
    }
}
