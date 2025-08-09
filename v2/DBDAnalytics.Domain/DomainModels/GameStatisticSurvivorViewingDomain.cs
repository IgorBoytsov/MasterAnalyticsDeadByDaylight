using System.Runtime.InteropServices;

namespace DBDAnalytics.Domain.DomainModels
{
    public class GameStatisticSurvivorViewingDomain
    {
        private GameStatisticSurvivorViewingDomain(
            int idGameStatistic, int idSurvivor, int idTypeDeath, byte[]? survivorImage, 
            DateTime? dateMatch, string? matchTime, string mapName, int countKill, int countHook, int countRecentGenerator)
        {
            IdGameStatistic = idGameStatistic;
            IdSurvivor = idSurvivor;
            IdTypeDeath = idTypeDeath;
            SurvivorImage = survivorImage;
            DateMatch = dateMatch;
            MatchTime = matchTime;
            MapName = mapName;
            CountKill = countKill;
            CountHook = countHook;
            CountRecentGenerator = countRecentGenerator;
        }

        public int IdGameStatistic { get; private set; }

        public int IdSurvivor { get; private set; }

        public int IdTypeDeath { get; private set; }

        public byte[]? SurvivorImage { get; private set; } = null!;

        public DateTime? DateMatch { get; private set; }

        public string? MatchTime { get; private set; } = null!;

        public string MapName { get; private set; } = null!;

        public int CountKill { get; private set; }

        public int CountHook { get; private set; }

        public int CountRecentGenerator { get; set; }

        public static (GameStatisticSurvivorViewingDomain GameStatisticSurvivorViewingDomain, string Message) Create(
            int idGameStatistic, int idSurvivor, int idTypeDeath, byte[]? survivorImage,
            DateTime? dateMatch, string? matchTime, string mapName, int countKill, int countHook, int countRecentGenerator)
        {
            string message = string.Empty;

            var view = new GameStatisticSurvivorViewingDomain(idGameStatistic, idSurvivor, idTypeDeath, survivorImage, dateMatch, matchTime, mapName, countKill, countHook, countRecentGenerator);

            return (view, message);
        }
    }
}