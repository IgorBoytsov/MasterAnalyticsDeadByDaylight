using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class GameStatisticService(ICreateGameStatisticUseCase createGameStatisticUseCase,
                                      IGetGameStatisticUseCase getGameStatisticUseCase) : IGameStatisticService
    {
        private readonly ICreateGameStatisticUseCase _createGameStatisticUseCase = createGameStatisticUseCase;
        private readonly IGetGameStatisticUseCase _getGameStatisticUseCase = getGameStatisticUseCase;

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

        public async Task<GameStatisticViewingDTO> GetAsync(int idGameStatistic)
        {
            return await _getGameStatisticUseCase.GetAsync(idGameStatistic);
        }
        
        public GameStatisticViewingDTO Get(int idGameStatistic)
        {
            return _getGameStatisticUseCase.Get(idGameStatistic);
        }

        public async Task<List<GameStatisticViewingDTO>> GetViewsAsync()
        {
            return await _getGameStatisticUseCase.GetViewsAsync();
        }
    }
}