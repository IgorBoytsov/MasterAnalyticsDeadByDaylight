using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.OfferingCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.OfferingCase
{
    public class GetOfferingUseCase(IOfferingRepository offeringRepository) : IGetOfferingUseCase
    {
        private readonly IOfferingRepository _offeringRepository = offeringRepository;

        public async Task<List<OfferingDTO>> GetAllAsync()
        {
            var domainEntities = await _offeringRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<OfferingDTO> GetAll()
        {
            var domainEntities = _offeringRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<OfferingDTO?> GetAsync(int idOffering)
        {
            var domainEntity = await _offeringRepository.GetAsync(idOffering);

            if (domainEntity == null)
            {
                Debug.WriteLine($"Offering с ID {idOffering} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}