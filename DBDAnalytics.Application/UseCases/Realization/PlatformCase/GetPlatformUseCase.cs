using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.PlatformCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.PlatformCase
{
    public class GetPlatformUseCase(IPlatformRepository platformRepository) : IGetPlatformUseCase
    {
        private readonly IPlatformRepository _platformRepository = platformRepository;

        public async Task<List<PlatformDTO>> GetAllAsync()
        {
            var domainEntities = await _platformRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<PlatformDTO> GetAll()
        {
            var domainEntities = _platformRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<PlatformDTO?> GetAsync(int idPlatform)
        {
            var domainEntity = await _platformRepository.GetAsync(idPlatform);

            if (domainEntity == null)
            {
                Debug.WriteLine($"Platform с ID {idPlatform} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}