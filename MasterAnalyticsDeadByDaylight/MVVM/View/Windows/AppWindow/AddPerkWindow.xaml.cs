using MasterAnalyticsDeadByDaylight.MVVM.ViewModel;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow
{
    /// <summary>
    /// Логика взаимодействия для AddPerkWindow.xaml
    /// </summary>
    public partial class AddPerkWindow : Window
    {
        public AddPerkWindow()
        {
            InitializeComponent();
            DataContext = new AddPerkWindowViewModel();
        }
    }
}
