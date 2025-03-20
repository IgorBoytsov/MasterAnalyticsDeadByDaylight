using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameStatisticCase
{
    public class GetGameStatisticKillerViewingUseCase(IGameStatisticRepository gameStatisticRepository) : IGetGameStatisticKillerViewingUseCase
    {
        private readonly IGameStatisticRepository _gameStatisticRepository = gameStatisticRepository;

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

        public async Task<List<GameStatisticKillerViewingDTO>> GetKillerViewsFilteredAsync(GameStatisticKillerFilterDTO filter)
        {
            var filterDomain = filter.ToDomain();
            var domainEntities = await _gameStatisticRepository.GetKillerViewsFilteredAsync(filterDomain);

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public DateTime? GetLastDateMatch()
        {
            return _gameStatisticRepository.GetLastDateMatch(true);
        }
    }
}