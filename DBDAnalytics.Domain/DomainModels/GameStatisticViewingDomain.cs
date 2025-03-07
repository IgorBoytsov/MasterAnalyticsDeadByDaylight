namespace DBDAnalytics.Domain.DomainModels
{
    public class GameStatisticViewingDomain
    {
        private GameStatisticViewingDomain(
            int idGameStatistic, int idKiller, byte[]? killerImage, DateTime? dateMatch, string? matchTime,
            string mapName, byte[]? mapImage, int countKill, int countHook, int countRecentGenerator, byte[]? resultMatch)
        {
            IdGameStatistic = idGameStatistic;
            IdKiller = idKiller;
            KillerImage = killerImage;
            DateMatch = dateMatch;
            MatchTime = matchTime;
            MapName = mapName;
            MapImage = mapImage;
            CountKill = countKill;
            CountHook = countHook;
            CountRecentGenerator = countRecentGenerator;
            ResultMatch = resultMatch;
        }

        public int IdGameStatistic { get; private set; }

        public int IdKiller { get; private set; }

        public byte[]? KillerImage { get; private set; } = null!;

        public DateTime? DateMatch { get; private set; }

        public string? MatchTime { get; private set; } = null!;

        public string MapName { get; private set; } = null!;

        public byte[]? MapImage { get; private set; } = null!;

        public int CountKill { get; private set; }

        public int CountHook { get; private set; }

        public int CountRecentGenerator { get; set; }

        public byte[]? ResultMatch { get; set; }

        public static (GameStatisticViewingDomain GameStatisticViewingDTO, string Message) Create(
              int idGameStatistic, int idKiller, byte[]? killerImage, DateTime? dateMatch, string? matchTime,
              string mapName, byte[]? mapImage, int countKill, int countHook, int countRecentGenerator, byte[]? resultMatch)
        {
            string message = string.Empty;

            var matchView = new GameStatisticViewingDomain(idGameStatistic, idKiller, killerImage, dateMatch, matchTime, mapName, mapImage, countKill, countHook, countRecentGenerator, resultMatch);

            return (matchView, message);
        }
    }
}