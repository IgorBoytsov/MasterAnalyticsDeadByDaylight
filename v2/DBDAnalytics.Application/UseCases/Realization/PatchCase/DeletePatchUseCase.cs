using DBDAnalytics.Application.UseCases.Abstraction.PatchCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.PatchCase
{
    public class DeletePatchUseCase(IPatchRepository patchRepository) : IDeletePatchUseCase
    {
        private readonly IPatchRepository _patchRepository = patchRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idPatch)
        {
            string message = string.Empty;

            var existBeforeDelete = await _patchRepository.ExistAsync(idPatch);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _patchRepository.DeleteAsync(idPatch);

            var existAfterDelete = await _patchRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}
