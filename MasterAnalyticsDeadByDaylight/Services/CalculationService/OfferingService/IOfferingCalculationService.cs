using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

namespace MasterAnalyticsDeadByDaylight.Services.CalculationService.OfferingService
{
    public interface IOfferingCalculationService
    {
        public Task<List<OfferingStat>> CalculatingOfferingStat(PlayerAssociation typeAssociation, OfferingCategory offeringCategory);
    }
}
