using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow
{
    /// <summary>
    /// Логика взаимодействия для AddMapWindow.xaml
    /// </summary>
    public partial class AddMapWindow : Window
    {
        public AddMapWindow()
        {
            InitializeComponent();
            DataContext = new AddMapWindowViewModel();
        }
    }
}
