using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.GameEventCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameEventCase
{
    public class CreateGameEventUseCase(IGameEventRepository gameEventRepository) : ICreateGameEventUseCase
    {
        private readonly IGameEventRepository _gameEventRepository = gameEventRepository;

        public async Task<(GameEventDTO? GameEventDTO, string? Message)> CreateAsync(string gameEventName, string gameEventDescription)
        {
            string message = string.Empty;

            var (CreatedGameEvent, Message) = GameEventDomain.Create(0, gameEventName, gameEventDescription);

            if (CreatedGameEvent is null)
            {
                return (null, Message);
            }

            bool exist = await _gameEventRepository.ExistAsync(gameEventName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _gameEventRepository.CreateAsync(gameEventName, gameEventDescription);

            var domainEntity = await _gameEventRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}