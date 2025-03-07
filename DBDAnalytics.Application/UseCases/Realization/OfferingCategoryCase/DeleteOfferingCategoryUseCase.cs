using DBDAnalytics.Application.UseCases.Abstraction.OfferingCategoryCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.OfferingCategoryCase
{
    public class DeleteOfferingCategoryUseCase(IOfferingCategoryRepository offeringCategoryRepository) : IDeleteOfferingCategoryUseCase
    {
        private readonly IOfferingCategoryRepository _offeringCategoryRepository = offeringCategoryRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idOfferingCategory)
        {
            string message = string.Empty;

            var existBeforeDelete = await _offeringCategoryRepository.ExistAsync(idOfferingCategory);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _offeringCategoryRepository.DeleteAsync(idOfferingCategory);

            var existAfterDelete = await _offeringCategoryRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}