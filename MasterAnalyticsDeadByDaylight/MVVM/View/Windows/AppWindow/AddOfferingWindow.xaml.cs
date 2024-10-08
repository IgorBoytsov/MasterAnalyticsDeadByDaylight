using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
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
            Func<MasterAnalyticsDeadByDaylightDbContext> contextFactory = () => new MasterAnalyticsDeadByDaylightDbContext();
            ICustomDialogService service = new CustomDialogService();
            IDataService dataService = new DataService(contextFactory);
            DataContext = new AddOfferingWindowViewModel(service, dataService);
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
