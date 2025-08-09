using DBDAnalytics.Application.UseCases.Abstraction.OfferingCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.OfferingCase
{
    public class DeleteOfferingUseCase(IOfferingRepository offeringRepository) : IDeleteOfferingUseCase
    {
        private readonly IOfferingRepository _offeringRepository = offeringRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idOffering)
        {
            string message = string.Empty;

            var existBeforeDelete = await _offeringRepository.ExistAsync(idOffering);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _offeringRepository.DeleteAsync(idOffering);

            var existAfterDelete = await _offeringRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}
