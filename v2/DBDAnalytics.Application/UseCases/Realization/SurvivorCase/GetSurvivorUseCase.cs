using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorCase
{
    public class GetSurvivorUseCase(ISurvivorRepository survivorRepository) : IGetSurvivorUseCase
    {
        private readonly ISurvivorRepository _survivorRepository = survivorRepository;

        public async Task<List<SurvivorDTO>> GetAllAsync()
        {
            var domainEntities = await _survivorRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<SurvivorDTO> GetAll()
        {
            var domainEntities = _survivorRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<SurvivorDTO?> GetAsync(int idSurvivor)
        {
            var domainEntity = await _survivorRepository.GetAsync(idSurvivor);

            if (domainEntity == null)
            {
                Debug.WriteLine($"Survivor с ID {idSurvivor} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}