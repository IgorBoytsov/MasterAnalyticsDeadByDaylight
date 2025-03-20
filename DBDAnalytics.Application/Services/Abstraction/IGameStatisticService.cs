using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IGameStatisticService
    {
        (int IdGameStatistic, string Message) Create(int idKillerInfo, int idFirstSurvivor, int idSecondSurvivor, int idThirdSurvivor, int idFourthSurvivor, int idMap, int idWhoPlacedMap, int idWhoPlacedMapWin, int idPatch, int idGameMode, int idGameEvent, DateTime? dateTimeMatch, string? gameTimeMatch, int countKills, int countHooks, int numberRecentGenerators, string descriptionGame, byte[]? resultMatch, int idMatchAttribute);
        GameStatisticKillerViewingDTO GetKillerView(int idGameStatistic);
        GameStatisticSurvivorViewingDTO GetSurvivorView(int idGameStatistic);
    }
}