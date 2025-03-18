using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IGameStatisticRepository
    {
        int Create(int idKillerInfo, int idFirstSurvivor, int idSecondSurvivor, int idThirdSurvivor, int idFourthSurvivor, int idMap, int idWhoPlacedMap, int idWhoPlacedMapWin, int idPatch, int idGameMode, int idGameEvent, DateTime? dateTimeMatch, string? gameTimeMatch, int countKills, int countHooks, int numberRecentGenerators, string descriptionGame, byte[]? resultMatch, int idMatchAttribute);
        Task<List<GameStatisticKillerViewingDomain>> GetKillerViewsAsync();
        Task<GameStatisticKillerViewingDomain> GetKillerViewAsync(int idGameStatistic);
        GameStatisticKillerViewingDomain GetKillerView(int idGameStatistic);
        Task<List<GameStatisticSurvivorViewingDomain>> GetSurvivorViewsAsync();
        Task<GameStatisticSurvivorViewingDomain> GetSurvivorViewAsync(int idGameStatistic);
        GameStatisticSurvivorViewingDomain GetSurvivorView(int idGameStatistic);
    }
}