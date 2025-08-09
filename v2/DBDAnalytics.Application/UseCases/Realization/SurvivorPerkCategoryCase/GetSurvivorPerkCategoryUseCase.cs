using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCategoryCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorPerkCategoryCase
{
    public class GetSurvivorPerkCategoryUseCase(ISurvivorPerkCategoryRepository survivorPerkCategoryRepository) : IGetSurvivorPerkCategoryUseCase
    {
        private readonly ISurvivorPerkCategoryRepository _survivorPerkCategoryRepository = survivorPerkCategoryRepository;

        public async Task<List<SurvivorPerkCategoryDTO>> GetAllAsync()
        {
            var domainEntities = await _survivorPerkCategoryRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<SurvivorPerkCategoryDTO> Get()
        {
            var domainEntities = _survivorPerkCategoryRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<SurvivorPerkCategoryDTO?> GetAsync(int idSurvivorPerkCategory)
        {
            var domainEntity = await _survivorPerkCategoryRepository.GetAsync(idSurvivorPerkCategory);

            if (domainEntity == null)
            {
                Debug.WriteLine($"SurvivorPerkCategory с ID {idSurvivorPerkCategory} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}
