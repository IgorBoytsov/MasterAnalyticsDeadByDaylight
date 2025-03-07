using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.OfferingCategoryCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.OfferingCategoryCase
{
    public class CreateOfferingCategoryUseCase(IOfferingCategoryRepository offeringCategoryRepository) : ICreateOfferingCategoryUseCase
    {
        private readonly IOfferingCategoryRepository _offeringCategoryRepository = offeringCategoryRepository;

        public async Task<(OfferingCategoryDTO? OfferingCategoryDTO, string? Message)> CreateAsync(string offeringCategoryName)
        {
            string message = string.Empty;

            var (CreatedOfferingCategory, Message) = OfferingCategoryDomain.Create(0, offeringCategoryName);

            if (CreatedOfferingCategory is null)
            {
                return (null, Message);
            }

            bool exist = await _offeringCategoryRepository.ExistAsync(offeringCategoryName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _offeringCategoryRepository.CreateAsync(offeringCategoryName);

            var domainEntity = await _offeringCategoryRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}