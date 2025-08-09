using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.OfferingCategoryCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.OfferingCategoryCase
{
    public class UpdateOfferingCategoryUseCase(IOfferingCategoryRepository offeringCategoryRepository) : IUpdateOfferingCategoryUseCase
    {
        private readonly IOfferingCategoryRepository _offeringCategoryRepository = offeringCategoryRepository;

        public async Task<(OfferingCategoryDTO? OfferingCategoryDTO, string? Message)> UpdateAsync(int idOfferingCategory, string offeringCategoryName, string? description)
        {
            string message = string.Empty;

            if (idOfferingCategory == 0 || string.IsNullOrWhiteSpace(offeringCategoryName))
                return (null, "Такой записи не существует");

            var exist = await _offeringCategoryRepository.ExistAsync(offeringCategoryName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _offeringCategoryRepository.UpdateAsync(idOfferingCategory, offeringCategoryName, description);

            var domainEntity = await _offeringCategoryRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}