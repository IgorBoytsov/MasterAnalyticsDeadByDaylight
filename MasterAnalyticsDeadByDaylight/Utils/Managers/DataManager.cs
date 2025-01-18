using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using Microsoft.Extensions.DependencyInjection;

namespace MasterAnalyticsDeadByDaylight.Utils.Managers
{
    public class DataManager
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDataService _dataService;

        public DataManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dataService = _serviceProvider.GetService<IDataService>();

            GetData();
        }

        public IEnumerable<Role> Roles { get; private set; }

        public IEnumerable<Killer> Killers { get; private set; }
        public IEnumerable<KillerPerk> KillerPerks { get; private set; }
        public IEnumerable<Survivor> Survivors { get; private set; }
        public IEnumerable<SurvivorPerk> SurvivorPerks { get; private set; }
        public IEnumerable<Offering> Offerings { get; private set; }

        private void GetData()
        {
            Roles = _dataService.GetAllData<Role>();

            Killers = _dataService.GetAllData<Killer>();
            KillerPerks = _dataService.GetAllData<KillerPerk>();
            Survivors = _dataService.GetAllData<Survivor>();
            SurvivorPerks = _dataService.GetAllData<SurvivorPerk>();
            Offerings = _dataService.GetAllData<Offering>();
        }
    }
}
