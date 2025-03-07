using DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameStatisticCase
{
    public class CreateGameStatisticUseCase(IGameStatisticRepository gameStatisticRepository) : ICreateGameStatisticUseCase
    {
        private readonly IGameStatisticRepository _gameStatisticRepository = gameStatisticRepository;

        public (int IdGameStatistic, string? Message) Create(int idKillerInfo,
                          int idFirstSurvivor, int idSecondSurvivor, int idThirdSurvivor, int idFourthSurvivor,
                          int idMap, int idWhoPlacedMap, int idWhoPlacedMapWin,
                          int idPatch, int idGameMode, int idGameEvent,
                          DateTime? dateTimeMatch, string? gameTimeMatch,
                          int countKills, int countHooks, int numberRecentGenerators,
                          string descriptionGame, byte[]? resultMatch, int idMatchAttribute)
        {
            string message = string.Empty;

            var (CreatedGameStatistic, Message) = GameStatisticDomain.Create(
                0, idKillerInfo, idMap, idWhoPlacedMap, idWhoPlacedMapWin, idPatch, 
                idGameMode, idGameEvent, 
                idFirstSurvivor, idSecondSurvivor, idThirdSurvivor, idFourthSurvivor, 
                dateTimeMatch, gameTimeMatch, countKills, countHooks, numberRecentGenerators, 
                descriptionGame, resultMatch, idMatchAttribute);

            if (CreatedGameStatistic is null)
            {
                return (0, Message);
            }

            var id = _gameStatisticRepository.Create(
                     idKillerInfo: idKillerInfo,
                     idFirstSurvivor: idFirstSurvivor, idSecondSurvivor: idSecondSurvivor, idThirdSurvivor: idThirdSurvivor, idFourthSurvivor: idFourthSurvivor,
                     idMap: idMap, idWhoPlacedMap: idWhoPlacedMap, idWhoPlacedMapWin: idWhoPlacedMapWin,
                     idPatch: idPatch, idGameMode: idGameMode, idGameEvent: idGameEvent,
                     dateTimeMatch: dateTimeMatch, gameTimeMatch: gameTimeMatch,
                     countKills: countKills, countHooks: countHooks, numberRecentGenerators: numberRecentGenerators, descriptionGame: descriptionGame, resultMatch: resultMatch,
                     idMatchAttribute: idMatchAttribute);

            return (id, message);
        }
    }
}