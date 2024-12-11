using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.WindowNavigation;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для MatchPage.xaml
    /// </summary>
    public partial class MatchPage : Page
    {
        public MatchPage()
        {
            InitializeComponent();
            //Func<MasterAnalyticsDeadByDaylightDbContext> _contextFactory = () => new MasterAnalyticsDeadByDaylightDbContext(); 
            //IDataService dataService = new DataService(_contextFactory);
            //IWindowNavigationService windowNavigationService = new WindowNavigationService(null);
            //DataContext = new MatchPageViewModel(dataService, windowNavigationService);
        }
    }
}
