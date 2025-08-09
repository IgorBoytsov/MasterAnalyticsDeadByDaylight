using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCategoryCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.KillerPerkCategoryCase
{
    public class GetKillerPerkCategoryUseCase(IKillerPerkCategoryRepository KillerPerkCategoryRepository) : IGetKillerPerkCategoryUseCase
    {
        private readonly IKillerPerkCategoryRepository _KillerPerkCategoryRepository = KillerPerkCategoryRepository;

        public async Task<List<KillerPerkCategoryDTO>> GetAllAsync()
        {
            var domainEntities = await _KillerPerkCategoryRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<KillerPerkCategoryDTO> GetAll()
        {
            var domainEntities = _KillerPerkCategoryRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<KillerPerkCategoryDTO?> GetAsync(int idKillerPerkCategory)
        {
            var domainEntity = await _KillerPerkCategoryRepository.GetAsync(idKillerPerkCategory);

            if (domainEntity == null)
            {
                Debug.WriteLine($"KillerPerkCategory с ID {idKillerPerkCategory} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}