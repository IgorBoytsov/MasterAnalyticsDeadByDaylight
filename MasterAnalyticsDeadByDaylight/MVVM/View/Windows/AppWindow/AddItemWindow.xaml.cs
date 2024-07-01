using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow
{
    /// <summary>
    /// Логика взаимодействия для AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        public AddItemWindow()
        {
            InitializeComponent();
            DataContext = new AddItemWindowViewModel();
        }
    }
}
