using MasterAnalyticsDeadByDaylight.MVVM.ViewModel;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.ModalWindow
{
    /// <summary>
    /// Логика взаимодействия для NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        public NotificationWindow()
        {
            InitializeComponent();
            DataContext = new NotificationWindowViewModel();
        }
    }
}
