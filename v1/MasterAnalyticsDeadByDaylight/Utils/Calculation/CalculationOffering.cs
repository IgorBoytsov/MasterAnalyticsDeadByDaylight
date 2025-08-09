using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

namespace MasterAnalyticsDeadByDaylight.Utils.Calculation
{
    public static class CalculationOffering
    {
        public static double PickRate(int selectedRecords, int countAllRecords)
        {
            return Math.Round((double)selectedRecords / countAllRecords * 100, 2);
        }

        public static OfferingCharacterUse OfferingSurvivorUses(IEnumerable<SurvivorInfo> survivorInfos, Survivor selectedSurvivor, Offering selectedOffering)
        {
            return new OfferingCharacterUse()
            {
                NameCharacter = selectedSurvivor.SurvivorName,
                AmountUsedOffering = survivorInfos.Count(x => x.IdSurvivor == selectedSurvivor.IdSurvivor && x.IdSurvivorOffering == selectedOffering.IdOffering)
            };
        }

        public static OfferingCharacterUse OfferingKillerUses(IEnumerable<KillerInfo> killerInfos, Killer selectedKiller, Offering selectedOffering)
        {
            return new OfferingCharacterUse()
            {
                NameCharacter = selectedKiller.KillerName,
                AmountUsedOffering = killerInfos.Count(x => x.IdKiller == selectedKiller.IdKiller && x.IdKillerOffering == selectedOffering.IdOffering)
            };
        }
    }
}