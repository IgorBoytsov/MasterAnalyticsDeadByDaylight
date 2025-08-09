namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    public class Perk
    {
        public int IdPerk { get; set; }

        public int IdCharacter { get; set; }

        public string PerkName { get; set; }

        public byte[] PerkImage { get; set; }

        public int? IdCategory { get; set; }

        public string PerkDescription { get; set; }
    }
}
