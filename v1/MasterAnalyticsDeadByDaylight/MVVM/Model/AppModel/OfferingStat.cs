using System.Drawing;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    public class OfferingStat
    {
        public int Index { get; set; }

        public string OfferingName { get; set; }

        public byte[] OfferingImage { get; set; }

        public int OfferingID {  get; set; }

        public int OfferingRole {  get; set; }

        public double PickRateSurvivorOffering { get; set; }

        public int PickSurvivorOfferingCount { get; set; }

        public double PickRateKillerOffering { get; set; }

        public int PickKillerOfferingCount { get; set; }

        public List<OfferingCharacterUse> OfferingSurvivorUses { get; set; }

        public List<OfferingCharacterUse> OfferingKillerUses { get; set; }
    }

    public class  OfferingCharacterUse
    {
        public string NameCharacter { get; set; }

        public int AmountUsedOffering { get; set; }
    }
}
