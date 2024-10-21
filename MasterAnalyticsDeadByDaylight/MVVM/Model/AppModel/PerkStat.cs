namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    public class PerkStat
    {
        public string PerkName { get; set; }

        public byte[] PerkImage { get; set; }

        public int PerkID { get; set; }

        public int AllAmountPerkUsed { get; set; }

        public double PickRate { get; set; }

        public double PickRatePercent { get; set; }

        public double WinRateAVG { get; set; }

        public double WinRatePercent { get; set; }

        public List<PerkCharacterUse> PerkCharacterUses { get; set; }
    }

    public class PerkCharacterUse
    {
        public string NameCharacter { get; set; }

        public int AmountUsedPerk { get; set; }
    }
}
