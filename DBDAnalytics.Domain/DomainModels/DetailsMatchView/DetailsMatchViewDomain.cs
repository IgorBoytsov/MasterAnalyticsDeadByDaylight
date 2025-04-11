namespace DBDAnalytics.Domain.DomainModels.DetailsMatchView
{
    public class DetailsMatchViewDomain
    {
        private DetailsMatchViewDomain()
        {
            
        }

        public byte[]? MapImage { get; private set; }

        public string? MapName { get; private set; }

        public string? GameEvent { get; private set; }

        public string? GameMode { get; private set; }

        public DateTime? DateTimeMatch { get; private set; }

        public string? MatchDuration { get; private set; }

        public byte[]? MatchImage { get; private set; }

        public DetailsMatchKillerViewDomain Killer { get; private set; } = null!;

        public DetailsMatchSurvivorViewDomain FirstSurvivor { get; private set; } = null!;

        public DetailsMatchSurvivorViewDomain SecondSurvivor { get; private set; } = null!;

        public DetailsMatchSurvivorViewDomain ThirdSurvivor { get; private set; } = null!;

        public DetailsMatchSurvivorViewDomain FourthSurvivor { get; private set; } = null!;

        public static DetailsMatchViewDomain Create(
            byte[]? mapImage, string? mapName, string? gameEvent, string? gameMode, DateTime? dateTimeMatch, string? matchDuration, byte[]? matchImage,
            DetailsMatchKillerViewDomain killer,
            DetailsMatchSurvivorViewDomain firstSurvivor,
            DetailsMatchSurvivorViewDomain secondSurvivor,
            DetailsMatchSurvivorViewDomain thirdSurvivor,
            DetailsMatchSurvivorViewDomain fourthSurvivor)
        {
            return new DetailsMatchViewDomain
            {
                MapImage = mapImage,
                MapName = mapName,
                GameEvent = gameEvent,
                GameMode = gameMode,
                DateTimeMatch = dateTimeMatch,
                MatchDuration = matchDuration,
                MatchImage = matchImage,
                Killer = killer,
                FirstSurvivor = firstSurvivor,
                SecondSurvivor = secondSurvivor,
                ThirdSurvivor = thirdSurvivor,
                FourthSurvivor = fourthSurvivor,
            };
        }
    }
}