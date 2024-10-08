using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using System.Windows;
using System.Windows.Input;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow
{
    /// <summary>
    /// Логика взаимодействия для AddSurvivorWindow.xaml
    /// </summary>
    public partial class AddSurvivorWindow : Window
    {
        public AddSurvivorWindow()
        {
            InitializeComponent();
            Func<MasterAnalyticsDeadByDaylightDbContext> contextFactory = () => new MasterAnalyticsDeadByDaylightDbContext();
            ICustomDialogService dialogService = new CustomDialogService();
            IDataService dataService = new DataService(contextFactory);
            DataContext = new AddSurvivorWindowViewModel(dialogService, dataService);
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
