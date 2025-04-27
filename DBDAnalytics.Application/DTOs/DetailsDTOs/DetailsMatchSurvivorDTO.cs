namespace DBDAnalytics.Application.DTOs.DetailsDTOs
{
    public class DetailsMatchSurvivorDTO
    {
        public int IdSurvivor { get; set; }

        public int? IdFirstPerk { get; set; }

        public int? IdSecondPerk { get; set; }

        public int? IdThirdPerk { get; set; }

        public int? IdFourthPerk { get; set; }

        public int? IdItem { get; set; }

        public int? IdFirstAddon { get; set; }

        public int? IdSecondAddon { get; set; }

        public int IdTypeDeath { get; set; }

        public int IdPlatform { get; set; }

        public int IdAssociation { get; set; }

        public bool Bot { get; set; }

        public bool Anonymous { get; set; }
    }
}