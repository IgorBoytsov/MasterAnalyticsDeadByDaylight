using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.MapCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.MapCase
{
    public class GetMapUseCase(IMapRepository mapRepository) : IGetMapUseCase
    {
        private readonly IMapRepository _mapRepository = mapRepository;

        public async Task<List<MapDTO>> GetAllAsync()
        {
            var domainEntities = await _mapRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<MapDTO> GetAll()
        {
            var domainEntities = _mapRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<MapDTO?> GetAsync(int idMap)
        {
            var domainEntity = await _mapRepository.GetAsync(idMap);

            if (domainEntity == null)
            {
                Debug.WriteLine($"Map с ID {idMap} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}