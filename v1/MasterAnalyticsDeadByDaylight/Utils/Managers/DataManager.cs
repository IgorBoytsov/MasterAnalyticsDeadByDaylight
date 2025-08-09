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

        #region Константы : ID игровой ассоциации

        public readonly int _ME_ASSOCIATION = 1;
        public readonly int _PARTNER_ASSOCIATION = 2;
        public readonly int _OPPONENT_ASSOCIATION = 3;
        public readonly int _RANDOM_PLAYER_ASSOCIATION = 4;

        #endregion

        #region Константы : ID роли

        public readonly int _KILLER_ROLE = 2;
        public readonly int _SURVIVOR_ROLE = 3;
        public readonly int _GENERAL_ROLE = 5;

        #endregion

        #region Коллекции : Общие

        public IEnumerable<Role> Roles { get; private set; }
        public IEnumerable<Offering> Offerings { get; private set; }

        #endregion

        #region Коллекции : Киллер

        public IEnumerable<Killer> Killers { get; private set; }
        public IEnumerable<KillerPerk> KillerPerks { get; private set; }
        public IEnumerable<KillerAddon> KillerAddons { get; private set; }

        #endregion

        #region Коллекции : Выживший

        public IEnumerable<Survivor> Survivors { get; private set; }
        public IEnumerable<SurvivorPerk> SurvivorPerks { get; private set; }
        public IEnumerable<Item> Items { get; private set; }
        public IEnumerable<ItemAddon> ItemAddons { get; private set; }

        #endregion

        private void GetData()
        {
            Roles = _dataService.GetAllData<Role>();

            Killers = _dataService.GetAllData<Killer>();
            KillerPerks = _dataService.GetAllData<KillerPerk>();
            KillerAddons = _dataService.GetAllData<KillerAddon>();

            Survivors = _dataService.GetAllData<Survivor>();
            SurvivorPerks = _dataService.GetAllData<SurvivorPerk>();
            Items = _dataService.GetAllData<Item>();
            ItemAddons = _dataService.GetAllData<ItemAddon>();

            Offerings = _dataService.GetAllData<Offering>();
        }
    }
}