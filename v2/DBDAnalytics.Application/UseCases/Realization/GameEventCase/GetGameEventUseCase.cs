using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.GameEventCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.GameEventCase
{
    public class GetGameEventUseCase(IGameEventRepository gameEventRepository) : IGetGameEventUseCase
    {
        private readonly IGameEventRepository _gameEventRepository = gameEventRepository;

        public async Task<List<GameEventDTO>> GetAllAsync()
        {
            var domainEntities = await _gameEventRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<GameEventDTO> GetAll()
        {
            var domainEntities = _gameEventRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<GameEventDTO?> GetAsync(int idGameEvent)
        {
            var domainEntity = await _gameEventRepository.GetAsync(idGameEvent);

            if (domainEntity == null)
            {
                Debug.WriteLine($"GameEvent с ID {idGameEvent} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}