using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IGameStatisticService
    {
        (int IdGameStatistic, string Message) Create(int idKillerInfo, int idFirstSurvivor, int idSecondSurvivor, int idThirdSurvivor, int idFourthSurvivor, int idMap, int idWhoPlacedMap, int idWhoPlacedMapWin, int idPatch, int idGameMode, int idGameEvent, DateTime? dateTimeMatch, string? gameTimeMatch, int countKills, int countHooks, int numberRecentGenerators, string descriptionGame, byte[]? resultMatch, int idMatchAttribute);
        Task<GameStatisticKillerViewingDTO> GetKillerViewAsync(int idGameStatistic);
        GameStatisticKillerViewingDTO GetKillerView(int idGameStatistic);
        Task<List<GameStatisticKillerViewingDTO>> GetKillerViewsAsync();
        Task<GameStatisticSurvivorViewingDTO> GetSurvivorViewAsync(int idGameStatistic);
        GameStatisticSurvivorViewingDTO GetSurvivorView(int idGameStatistic);
        Task<List<GameStatisticSurvivorViewingDTO>> GetSurvivorViewsAsync();
    }
}