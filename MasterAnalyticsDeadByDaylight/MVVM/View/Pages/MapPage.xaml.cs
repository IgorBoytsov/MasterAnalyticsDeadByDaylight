using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using System.Windows.Controls;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для MapPage.xaml
    /// </summary>
    public partial class MapPage : Page
    {
        MapPageViewModel ViewModel { get; set; }

        public MapPage()
        {
            InitializeComponent();

            Func<MasterAnalyticsDeadByDaylightDbContext> _contextFactory = () => new MasterAnalyticsDeadByDaylightDbContext();
            IDataService dataService = new DataService(_contextFactory);
            DataContext = new MapPageViewModel(dataService);
        }
    }
}
