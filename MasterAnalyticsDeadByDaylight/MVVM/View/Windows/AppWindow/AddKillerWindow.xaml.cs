using MasterAnalyticsDeadByDaylight.MVVM.ViewModel;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow
{
    /// <summary>
    /// Логика взаимодействия для AddKillerWindow.xaml
    /// </summary>
    public partial class AddKillerWindow : Window
    {
        public AddKillerWindow()
        {
            InitializeComponent();
            DataContext = new AddKillerWindowViewModel();
        }
    }
}
