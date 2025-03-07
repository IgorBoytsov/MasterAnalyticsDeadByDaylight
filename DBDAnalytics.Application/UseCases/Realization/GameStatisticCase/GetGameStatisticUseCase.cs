using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameStatisticCase
{
    public class GetGameStatisticUseCase(IGameStatisticRepository gameStatisticRepository) : IGetGameStatisticUseCase
    {
        private readonly IGameStatisticRepository _gameStatisticRepository = gameStatisticRepository;

        public async Task<GameStatisticViewingDTO> GetAsync(int idGameStatistic)
        {
            var domainEntity = await _gameStatisticRepository.GetAsync(idGameStatistic);

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }        
        
        public GameStatisticViewingDTO? Get(int idGameStatistic)
        {
            var domainEntity = _gameStatisticRepository.Get(idGameStatistic);

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }

        public async Task<List<GameStatisticViewingDTO>> GetViewsAsync()
        {
            var domainEntities = await _gameStatisticRepository.GetViewsAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }       
    }
}