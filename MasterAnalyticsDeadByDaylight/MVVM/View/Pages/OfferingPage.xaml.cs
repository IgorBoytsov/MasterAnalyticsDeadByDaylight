using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using MasterAnalyticsDeadByDaylight.Services.CalculationService.OfferingService;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для OfferingPage.xaml
    /// </summary>
    public partial class OfferingPage : Page
    {
        public OfferingPage()
        {
            InitializeComponent();
            Func<MasterAnalyticsDeadByDaylightDbContext> _contextFactory = () => new MasterAnalyticsDeadByDaylightDbContext();
            Func<MasterAnalyticsDeadByDaylightDbContext> _contextFactory2 = () => new MasterAnalyticsDeadByDaylightDbContext();
            
            IDataService dataService = new DataService(_contextFactory);
            IOfferingCalculationService offeringCalculationService = new OfferingCalculationService(_contextFactory2);

            DataContext = new OfferingPageViewModel(dataService, offeringCalculationService);
        }
    }
}
