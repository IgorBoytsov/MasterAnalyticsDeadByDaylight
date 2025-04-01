namespace DBDAnalytics.Domain.DomainModels.DetailsModels
{
    public class DetailsMatchDomain
    {
        private DetailsMatchDomain()
        {
        }

        public int IdGameStatistic { get; private set; }

        public int CountKill { get; private set; }

        public int CountHook { get; private set; }

        public int RecentGenerator { get; private set; }

        public string? DurationMatch { get; private set; }

        public DateTime? Date { get; private set; }

        public DetailsMatchKillerDomain Killer { get; private set; } = null!;

        public DetailsMatchSurvivorDomain? FirstSurvivorInfo { get; private set; }

        public DetailsMatchSurvivorDomain? SecondSurvivorInfo { get; private set; }

        public DetailsMatchSurvivorDomain? ThirdSurvivorInfo { get; private set; }

        public DetailsMatchSurvivorDomain? FourthSurvivorInfo { get; private set; }

        public static DetailsMatchDomain Create(
            DetailsMatchKillerDomain killer,
            int idGameStatistic,
            int countKill, int countHook, int recentGenerator, int score,
            string? DurationMatch, DateTime? date,
            DetailsMatchSurvivorDomain? firstSurvivorInfo,
            DetailsMatchSurvivorDomain? secondSurvivorInfo,
            DetailsMatchSurvivorDomain? thirdSurvivorInfo,
            DetailsMatchSurvivorDomain? fourthSurvivorInfo)
        {
            var stats = new DetailsMatchDomain
            {
                Killer = killer,

                IdGameStatistic = idGameStatistic,

                CountKill = countKill,
                CountHook = countHook,
                RecentGenerator = recentGenerator,
                DurationMatch = DurationMatch,
                Date = date,

                FirstSurvivorInfo = firstSurvivorInfo,
                SecondSurvivorInfo = secondSurvivorInfo,
                ThirdSurvivorInfo = thirdSurvivorInfo,
                FourthSurvivorInfo = fourthSurvivorInfo
            };

            return stats;
        }
    }
}