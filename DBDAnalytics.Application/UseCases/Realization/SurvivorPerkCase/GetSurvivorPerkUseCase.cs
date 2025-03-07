using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorPerkCase
{
    public class GetSurvivorPerkUseCase(ISurvivorPerkRepository survivorPerkRepository) : IGetSurvivorPerkUseCase
    {
        private readonly ISurvivorPerkRepository _survivorPerkRepository = survivorPerkRepository;

        public async Task<List<SurvivorPerkDTO>> GetAllAsync()
        {
            var domainEntities = await _survivorPerkRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<SurvivorPerkDTO> GetAll()
        {
            var domainEntities = _survivorPerkRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<SurvivorPerkDTO?> GetAsync(int idPerk)
        {
            var domainEntity = await _survivorPerkRepository.GetAsync(idPerk);

            if (domainEntity == null)
            {
                Debug.WriteLine($"SurvivorPerk с ID {idPerk} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}