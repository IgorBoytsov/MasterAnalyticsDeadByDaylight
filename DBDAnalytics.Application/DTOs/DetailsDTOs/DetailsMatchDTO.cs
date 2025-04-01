namespace DBDAnalytics.Application.DTOs.DetailsDTOs
{
    public class DetailsMatchDTO
    {
        public DetailsMatchKillerDTO KillerDTO { get; set; } = null!;

        public int IdGameStatistic { get; set; }

        public int CountKill { get; set; }

        public int CountHook { get; set; }

        public int RecentGenerator { get; set; }

        public string? DurationMatch { get; set; }

        public DateTime? Date { get; set; }

        public DetailsMatchSurvivorDTO? FirstSurvivorInfo { get; set; }

        public DetailsMatchSurvivorDTO? SecondSurvivorInfo { get; set; }

        public DetailsMatchSurvivorDTO? ThirdSurvivorInfo { get; set; }

        public DetailsMatchSurvivorDTO? FourthSurvivorInfo { get; set; }
    }
}