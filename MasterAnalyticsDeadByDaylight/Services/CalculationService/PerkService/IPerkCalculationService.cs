using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

namespace MasterAnalyticsDeadByDaylight.Services.CalculationService.PerkService
{
    public interface IPerkCalculationService
    {
        Task<List<PerkStat>> CalculatingPerkStatAsync(Role role, PlayerAssociation typeAssociation, string sortingValue);
    }
}
