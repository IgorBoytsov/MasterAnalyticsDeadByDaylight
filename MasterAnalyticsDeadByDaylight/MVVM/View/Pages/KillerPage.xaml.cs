using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using MasterAnalyticsDeadByDaylight.Services.CalculationService.KillerService;
using MasterAnalyticsDeadByDaylight.Services.CalculationService.MapService;
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
            IKillerCalculationService killerCalculationService = new KillerCalculationService(contextFactory);
            IMapCalculationService mapCalculationService = new MapCalculationService(contextFactory);
            DataContext = new KillerPageViewModel(this, dataService, killerCalculationService, mapCalculationService);
        }
    }
}
