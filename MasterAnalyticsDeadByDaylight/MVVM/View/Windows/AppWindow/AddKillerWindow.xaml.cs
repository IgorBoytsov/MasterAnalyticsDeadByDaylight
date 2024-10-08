using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using System.Windows;
using System.Windows.Input;

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
            Func<MasterAnalyticsDeadByDaylightDbContext> contextFactory = () => new MasterAnalyticsDeadByDaylightDbContext();
            ICustomDialogService service = new CustomDialogService();
            IDataService dataService = new DataService(contextFactory);
            DataContext = new AddKillerWindowViewModel(service, dataService);
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
