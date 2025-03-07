using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.GameEventCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameEventCase
{
    public class UpdateGameEventUseCase(IGameEventRepository gameEventRepository) : IUpdateGameEventUseCase
    {
        private readonly IGameEventRepository _gameEventRepository = gameEventRepository;

        public async Task<(GameEventDTO? GameEventDTO, string? Message)> UpdateAsync(int idGameEvent, string gameEventName, string gameEventDescription)
        {
            string message = string.Empty;

            if (idGameEvent == 0 || string.IsNullOrWhiteSpace(gameEventName))
                return (null, "Такой записи не существует");

            var exist = await _gameEventRepository.ExistAsync(gameEventName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _gameEventRepository.UpdateAsync(idGameEvent, gameEventName, gameEventDescription);

            var domainEntity = await _gameEventRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<GameEventDTO?> ForcedUpdateAsync(int idGameEvent, string gameEventName, string gameEventDescription)
        {
            int id = await _gameEventRepository.UpdateAsync(idGameEvent, gameEventName, gameEventDescription);

            var domainEntity = await _gameEventRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}