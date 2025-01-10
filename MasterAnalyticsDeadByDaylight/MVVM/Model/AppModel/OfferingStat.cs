using System.Drawing;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    public class OfferingStat
    {
        public string OfferingName { get; set; }

        public byte[] OfferingImage { get; set; }

        public int OfferingID {  get; set; }

        public int OfferingRole {  get; set; }

        public int AllAmountCharacterUsed { get; set; }

        public double PickRateCharacterPercent { get; set; }

        public double NumberMatchSurvivor { get; set; }

        public double PickRateKillerPercent { get; set; }

        public double NumberPickKillerOffering { get; set; }

        public double NumberMatchKiller { get; set; }

        public List<OfferingCharacterUse> OfferingSurvivorUses { get; set; }

        public List<OfferingCharacterUse> OfferingKillerUses { get; set; }
    }

    public class  OfferingCharacterUse
    {
        public string NameCharacter { get; set; }

        public int AmountUsedOffering { get; set; }
    }
}
