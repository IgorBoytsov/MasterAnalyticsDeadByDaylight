using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.GameModeCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameModeCase
{
    public class CreateGameModeUseCase(IGameModeRepository gameModeRepository) : ICreateGameModeUseCase
    {
        private readonly IGameModeRepository _gameModeRepository = gameModeRepository;

        public async Task<(GameModeDTO? GameModeDTO, string? Message)> CreateAsync(string gameModeName, string gameModeDescription)
        {
            string message = string.Empty;

            var (CreatedGameMode, Message) = GameModeDomain.Create(0, gameModeName, gameModeDescription);

            if (CreatedGameMode is null)
            {
                return (null, Message);
            }

            bool exist = await _gameModeRepository.ExistAsync(gameModeName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _gameModeRepository.CreateAsync(gameModeName, gameModeDescription);

            var domainEntity = await _gameModeRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}