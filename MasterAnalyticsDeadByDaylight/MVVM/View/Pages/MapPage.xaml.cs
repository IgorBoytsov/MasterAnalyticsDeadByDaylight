using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using System.Windows.Controls;
using System.Drawing;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.CalculationService.MapService;
using MasterAnalyticsDeadByDaylight.Services.CalculationService.KillerService;

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
            IMapCalculationService mapCalculationService = new MapCalculationService(_contextFactory);
            IKillerCalculationService killerCalculationService = new KillerCalculationService(_contextFactory);
            DataContext = new MapPageViewModel(dataService, mapCalculationService, killerCalculationService);
        }
    }
}
