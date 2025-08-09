namespace DBDAnalytics.Application.DTOs.DetailsViewDTOs
{
    public class DetailsMatchViewDTO
    {
        public byte[]? MapImage { get; set; }

        public string? MapName { get; set; }

        public string? GameEvent { get; set; }

        public string? GameMode { get; set; }

        public DateTime? DateTimeMatch { get; set; }

        public string? MatchDuration { get; set; }

        public byte[]? MatchImage { get; set; }

        public DetailsMatchKillerViewDTO Killer { get; set; } = null!;

        public DetailsMatchSurvivorViewDTO FirstSurvivor { get; set; } = null!;

        public DetailsMatchSurvivorViewDTO SecondSurvivor { get; set; } = null!;

        public DetailsMatchSurvivorViewDTO ThirdSurvivor { get; set; } = null!;

        public DetailsMatchSurvivorViewDTO FourthSurvivor { get; set; } = null!;
    }
}