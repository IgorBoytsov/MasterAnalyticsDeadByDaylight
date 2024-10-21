namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    public class OfferingStat
    {
        public string OfferingName { get; set; }

        public byte[] OfferingImage { get; set; }

        public int OfferingID {  get; set; }

        public string OfferingRole {  get; set; }

        public int AllAmountSurvivorUsed { get; set; }

        public int AllAmountKillerUsed { get; set; }

        public double PickRateSurvivorPercent { get; set; }

        public double NumberPickSurvivorOffering { get; set; }

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
