using MasterAnalyticsDeadByDaylight.MVVM.ViewModel;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.ModalWindow
{
    /// <summary>
    /// Логика взаимодействия для ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow()
        {
            InitializeComponent();
            DataContext = new ErrorWindowViewModel();
        }
    }
}
