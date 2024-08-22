using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using System.Windows;
using System.Windows.Input;

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
            IDialogService service = new DialogService();
            DataContext = new AddOfferingWindowViewModel(service);
        }
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }
    }
}
