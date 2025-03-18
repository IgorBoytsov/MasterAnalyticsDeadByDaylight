using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameStatisticCase
{
    public class GetGameStatisticUseCase(IGameStatisticRepository gameStatisticRepository) : IGetGameStatisticUseCase
    {
        private readonly IGameStatisticRepository _gameStatisticRepository = gameStatisticRepository;

        public async Task<List<GameStatisticKillerViewingDTO>> GetKillerViewsAsync()
        {
            var domainEntities = await _gameStatisticRepository.GetKillerViewsAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<GameStatisticKillerViewingDTO> GetKillerViewAsync(int idGameStatistic)
        {
            var domainEntity = await _gameStatisticRepository.GetKillerViewAsync(idGameStatistic);

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }

        public GameStatisticKillerViewingDTO GetKillerView(int idGameStatistic)
        {
            var domainEntity = _gameStatisticRepository.GetKillerView(idGameStatistic);

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }

        public async Task<List<GameStatisticSurvivorViewingDTO>> GetSurvivorViewsAsync()
        {
            var domainEntities = await _gameStatisticRepository.GetSurvivorViewsAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<GameStatisticSurvivorViewingDTO> GetSurvivorViewAsync(int idGameStatistic)
        {
            var domainEntity = await _gameStatisticRepository.GetSurvivorViewAsync(idGameStatistic);

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }

        public GameStatisticSurvivorViewingDTO GetSurvivorView(int idGameStatistic)
        {
            var domainEntity = _gameStatisticRepository.GetSurvivorView(idGameStatistic);

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}