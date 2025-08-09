using DBDAnalytics.Application.UseCases.Abstraction.TypeDeathCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.TypeDeathCase
{
    public class DeleteTypeDeathUseCase(ITypeDeathRepository typeDeathRepository) : IDeleteTypeDeathUseCase
    {
        private readonly ITypeDeathRepository _typeDeathRepository = typeDeathRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idTypeDeath)
        {
            string message = string.Empty;

            var existBeforeDelete = await _typeDeathRepository.ExistAsync(idTypeDeath);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _typeDeathRepository.DeleteAsync(idTypeDeath);

            var existAfterDelete = await _typeDeathRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}