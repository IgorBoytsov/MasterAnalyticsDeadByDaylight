using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IGameStatisticRepository
    {
        int Create(int idKillerInfo, int idFirstSurvivor, int idSecondSurvivor, int idThirdSurvivor, int idFourthSurvivor, int idMap, int idWhoPlacedMap, int idWhoPlacedMapWin, int idPatch, int idGameMode, int idGameEvent, DateTime? dateTimeMatch, string? gameTimeMatch, int countKills, int countHooks, int numberRecentGenerators, string descriptionGame, byte[]? resultMatch, int idMatchAttribute);
        Task<GameStatisticViewingDomain?> GetAsync(int idGameStatistic);
        GameStatisticViewingDomain? Get(int idGameStatistic);
        Task<List<GameStatisticViewingDomain>> GetViewsAsync();
    }
}