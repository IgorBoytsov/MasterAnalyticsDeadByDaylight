using DBDAnalytics.Application.UseCases.Abstraction.PlatformCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.PlatformCase
{
    public class DeletePlatformUseCase(IPlatformRepository platformRepository) : IDeletePlatformUseCase
    {
        private readonly IPlatformRepository _platformRepository = platformRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idPlatform)
        {
            string message = string.Empty;

            var existBeforeDelete = await _platformRepository.ExistAsync(idPlatform);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _platformRepository.DeleteAsync(idPlatform);

            var existAfterDelete = await _platformRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}