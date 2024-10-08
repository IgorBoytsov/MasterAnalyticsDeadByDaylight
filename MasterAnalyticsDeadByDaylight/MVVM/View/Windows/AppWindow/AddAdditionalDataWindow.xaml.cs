using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using System.Windows;
using System.Windows.Input;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow
{
    /// <summary>
    /// Логика взаимодействия для AddAdditionalDataWindow.xaml
    /// </summary>
    public partial class AddAdditionalDataWindow : Window
    {
        public AddAdditionalDataWindow()
        {
            InitializeComponent();
            Func<MasterAnalyticsDeadByDaylightDbContext> contextFactory = () => new MasterAnalyticsDeadByDaylightDbContext();
            IDataService dataService = new DataService(contextFactory);
            DataContext = new AddAdditionalDataWindowViewModel(dataService);
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
