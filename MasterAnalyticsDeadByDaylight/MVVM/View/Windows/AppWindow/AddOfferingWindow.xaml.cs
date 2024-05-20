using MasterAnalyticsDeadByDaylight.MVVM.ViewModel;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow
{
    /// <summary>
    /// Логика взаимодействия для AddOfferingWindow.xaml
    /// </summary>
    public partial class AddOfferingWindow : Window
    {
        public AddOfferingWindow()
        {
            InitializeComponent();
            DataContext = new AddOfferingWindowViewModel();
        }
    }
}
