using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.GameModeCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameModeCase
{
    public class UpdateGameModeUseCase(IGameModeRepository gameModeRepository) : IUpdateGameModeUseCase
    {
        private readonly IGameModeRepository _gameModeRepository = gameModeRepository;

        public async Task<(GameModeDTO? GameEventDTO, string? Message)> UpdateAsync(int idGameMode, string gameModeName, string gameModeDescription)
        {
            string message = string.Empty;

            if (idGameMode == 0 || string.IsNullOrWhiteSpace(gameModeName))
                return (null, "Такой записи не существует");

            var exist = await _gameModeRepository.ExistAsync(gameModeName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _gameModeRepository.UpdateAsync(idGameMode, gameModeName, gameModeDescription);

            var domainEntity = await _gameModeRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<GameModeDTO?> ForcedUpdateAsync(int idGameMode, string gameModeName, string gameModeDescription)
        {
            int id = await _gameModeRepository.UpdateAsync(idGameMode, gameModeName, gameModeDescription);

            var domainEntity = await _gameModeRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}
