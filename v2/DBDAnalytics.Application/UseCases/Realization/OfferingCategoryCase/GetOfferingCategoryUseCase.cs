using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.OfferingCategoryCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.OfferingCategoryCase
{
    public class GetOfferingCategoryUseCase(IOfferingCategoryRepository offeringCategoryRepository) : IGetOfferingCategoryUseCase
    {
        private readonly IOfferingCategoryRepository _offeringCategoryRepository = offeringCategoryRepository;

        public async Task<List<OfferingCategoryDTO>> GetAllAsync()
        {
            var domainEntities = await _offeringCategoryRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<OfferingCategoryDTO> GetAll()
        {
            var domainEntities = _offeringCategoryRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<OfferingCategoryDTO?> GetAsync(int idOfferingCategory)
        {
            var domainEntity = await _offeringCategoryRepository.GetAsync(idOfferingCategory);

            if (domainEntity == null)
            {
                Debug.WriteLine($"OfferingCategory с ID {idOfferingCategory} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}