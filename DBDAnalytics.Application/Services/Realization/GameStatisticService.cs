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

        public async Task<GameStatisticKillerViewingDTO> GetKillerViewAsync(int idGameStatistic)
        {
            return await _getGameStatisticUseCase.GetKillerViewAsync(idGameStatistic);
        }        
        
        public GameStatisticKillerViewingDTO GetKillerView(int idGameStatistic)
        {
            return _getGameStatisticUseCase.GetKillerView(idGameStatistic);
        }

        public async Task<List<GameStatisticKillerViewingDTO>> GetKillerViewsAsync()
        {
            return await _getGameStatisticUseCase.GetKillerViewsAsync();
        }

        public async Task<GameStatisticSurvivorViewingDTO> GetSurvivorViewAsync(int idGameStatistic)
        {
            return await _getGameStatisticUseCase.GetSurvivorViewAsync(idGameStatistic);
        }
                
        public GameStatisticSurvivorViewingDTO GetSurvivorView(int idGameStatistic)
        {
            return _getGameStatisticUseCase.GetSurvivorView(idGameStatistic);
        }

        public async Task<List<GameStatisticSurvivorViewingDTO>> GetSurvivorViewsAsync()
        {
            return await _getGameStatisticUseCase.GetSurvivorViewsAsync();
        }
    }
}