namespace DBDAnalytics.Application.DTOs.DetailsDTOs
{
    public class DetailsMatchKillerDTO
    {
        public int IdKiller { get; set; }

        public int? FirstAddonID { get; set; }

        public int? SecondAddonID { get; set; }

        public int? FirstPerkID { get; set; }

        public int? SecondPerkID { get; set; }

        public int? ThirdPerkID { get; set; }

        public int? FourthPerkID { get; set; }

        public int? OfferingID { get; set; }

        public int Score { get; set; }
    }
}