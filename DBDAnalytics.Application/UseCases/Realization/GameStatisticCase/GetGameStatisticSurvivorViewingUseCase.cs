using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameStatisticCase
{
    public class GetGameStatisticSurvivorViewingUseCase(IGameStatisticRepository gameStatisticRepository) : IGetGameStatisticSurvivorViewingUseCase
    {
        private readonly IGameStatisticRepository _gameStatisticRepository = gameStatisticRepository;

        public async Task<List<GameStatisticSurvivorViewingDTO>> GetSurvivorViewsAsync(GameStatisticSurvivorFilterDTO filter)
        {
            var domainFilter = filter.ToDomain();

            var domainEntities = await _gameStatisticRepository.GetSurvivorViewsAsync(domainFilter);

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

        public DateTime? GetLastDateMatch()
        {
            return _gameStatisticRepository.GetLastDateMatch(false);
        }
    }
}
