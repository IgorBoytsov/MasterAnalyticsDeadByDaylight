using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class GameStatisticService(ICreateGameStatisticUseCase createGameStatisticUseCase,
                                      IGetGameStatisticKillerViewingUseCase getGameStatisticUseCase,
                                      IGetGameStatisticSurvivorViewingUseCase getGameStatisticSurvivorViewingUseCase) : IGameStatisticService
    {
        private readonly ICreateGameStatisticUseCase _createGameStatisticUseCase = createGameStatisticUseCase;
        private readonly IGetGameStatisticKillerViewingUseCase _getGameStatisticUseCase = getGameStatisticUseCase;
        private readonly IGetGameStatisticSurvivorViewingUseCase _getGameStatisticSurvivorViewingUseCase = getGameStatisticSurvivorViewingUseCase;

        public (int IdGameStatistic, string Message) Create(
                int idKillerInfo, int idFirstSurvivor,
                int idSecondSurvivor, int idThirdSurvivor, int idFourthSurvivor,
                int idMap, int idWhoPlacedMap, int idWhoPlacedMapWin,
                int idPatch, int idGameMode, int idGameEvent, DateTime? dateTimeMatch, string? gameTimeMatch,
                int countKills, int countHooks, int numberRecentGenerators, string descriptionGame, byte[]? resultMatch, int idMatchAttribute)
        {
            return _createGameStatisticUseCase.Create(
                         idKillerInfo: idKillerInfo,
                         idFirstSurvivor: idFirstSurvivor, idSecondSurvivor: idSecondSurvivor, idThirdSurvivor: idThirdSurvivor, idFourthSurvivor: idFourthSurvivor,
                         idMap: idMap, idWhoPlacedMap: idWhoPlacedMap, idWhoPlacedMapWin: idWhoPlacedMapWin,
                         idPatch: idPatch, idGameMode: idGameMode, idGameEvent: idGameEvent,
                         dateTimeMatch: dateTimeMatch, gameTimeMatch: gameTimeMatch,
                         countKills: countKills, countHooks: countHooks, numberRecentGenerators: numberRecentGenerators, descriptionGame: descriptionGame, resultMatch: resultMatch,
                         idMatchAttribute: idMatchAttribute);
        }      
        
        public GameStatisticKillerViewingDTO GetKillerView(int idGameStatistic)
        {
            return _getGameStatisticUseCase.GetKillerView(idGameStatistic);
        }
                
        public GameStatisticSurvivorViewingDTO GetSurvivorView(int idGameStatistic)
        {
            return _getGameStatisticSurvivorViewingUseCase.GetSurvivorView(idGameStatistic);
        }
    }
}