namespace DBDAnalytics.Application.DTOs.DetailsDTOs
{
    public class DetailsMatchSurvivorDTO
    {
        public int IdSurvivor { get; set; }

        public int IdTypeDeath { get; set; }

        public int IdPlatform { get; set; }

        public bool Bot { get; set; }

        public bool Anonymous { get; set; }
    }
}