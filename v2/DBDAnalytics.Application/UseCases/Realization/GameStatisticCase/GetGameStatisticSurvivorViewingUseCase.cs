using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameStatisticCase
{
    public class GetGameStatisticSurvivorViewingUseCase(IGameStatisticSurvivorViewingRepository gameStatisticSurvivorViewingRepository,
                                                        IGameStatisticRepository gameStatisticRepository) : IGetGameStatisticSurvivorViewingUseCase
    {
        private readonly IGameStatisticSurvivorViewingRepository _gameStatisticSurvivorViewingRepository = gameStatisticSurvivorViewingRepository;
        private readonly IGameStatisticRepository _gameStatisticRepository = gameStatisticRepository;

        public async Task<List<GameStatisticSurvivorViewingDTO>> GetSurvivorViewsAsync(GameStatisticSurvivorFilterDTO filter)
        {
            var domainFilter = filter.ToDomain();

            var domainEntities = await _gameStatisticSurvivorViewingRepository.GetSurvivorViewsAsync(domainFilter);

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<GameStatisticSurvivorViewingDTO> GetSurvivorViewAsync(int idGameStatistic)
        {
            var domainEntity = await _gameStatisticSurvivorViewingRepository.GetSurvivorViewAsync(idGameStatistic);

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }

        public GameStatisticSurvivorViewingDTO GetSurvivorView(int idGameStatistic)
        {
            var domainEntity = _gameStatisticSurvivorViewingRepository.GetSurvivorView(idGameStatistic);

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }

        public DateTime? GetLastDateMatch()
        {
            return _gameStatisticRepository.GetLastDateMatch(false);
        }
    }
}