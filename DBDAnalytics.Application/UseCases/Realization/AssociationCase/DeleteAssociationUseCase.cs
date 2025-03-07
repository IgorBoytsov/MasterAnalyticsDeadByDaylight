using DBDAnalytics.Application.UseCases.Abstraction.AssociationCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.AssociationCase
{
    public class DeleteAssociationUseCase(IAssociationRepository associationRepository) : IDeleteAssociationUseCase
    {
        private readonly IAssociationRepository _associationRepository = associationRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idPlayerAssociation)
        {
            string message = string.Empty;

            var existBeforeDelete = await _associationRepository.ExistAsync(idPlayerAssociation);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _associationRepository.DeleteAsync(idPlayerAssociation);

            var existAfterDelete = await _associationRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}