using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow
{
    /// <summary>
    /// Логика взаимодействия для AddSurvivorWindow.xaml
    /// </summary>
    public partial class AddSurvivorWindow : Window
    {
        public AddSurvivorWindow()
        {
            InitializeComponent();
            DataContext = new AddSurvivorWindowViewModel();
        }
    }
}
