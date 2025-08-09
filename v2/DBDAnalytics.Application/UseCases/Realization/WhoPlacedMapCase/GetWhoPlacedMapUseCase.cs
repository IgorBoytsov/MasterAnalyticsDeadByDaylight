using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.WhoPlacedMapCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.WhoPlacedMapCase
{
    public class GetWhoPlacedMapUseCase(IWhoPlacedMapRepository whoPlacedMapRepository) : IGetWhoPlacedMapUseCase
    {
        private readonly IWhoPlacedMapRepository _whoPlacedMapRepository = whoPlacedMapRepository;

        public async Task<List<WhoPlacedMapDTO>> GetAllAsync()
        {
            var domainEntities = await _whoPlacedMapRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<WhoPlacedMapDTO> GetAll()
        {
            var domainEntities = _whoPlacedMapRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<WhoPlacedMapDTO?> GetAsync(int idWhoPlacedMap)
        {
            var domainEntity = await _whoPlacedMapRepository.GetAsync(idWhoPlacedMap);

            if (domainEntity == null)
            {
                Debug.WriteLine($"WhoPlacedMap с ID {idWhoPlacedMap} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}
