using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для KillerPage.xaml
    /// </summary>
    public partial class KillerPage : Page
    {
        public KillerPage()
        {
            InitializeComponent();
            Func<MasterAnalyticsDeadByDaylightDbContext> contextFactory = () => new MasterAnalyticsDeadByDaylightDbContext();
            IDataService dataService = new DataService(contextFactory);
            DataContext = new KillerPageViewModel(this, dataService);
        }
    }
}
