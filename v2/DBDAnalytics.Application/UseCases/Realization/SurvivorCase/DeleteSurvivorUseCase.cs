using DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorCase
{
    public class DeleteSurvivorUseCase(ISurvivorRepository survivorRepository) : IDeleteSurvivorUseCase
    {
        private readonly ISurvivorRepository _survivorRepository = survivorRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idSurvivor)
        {
            string message = string.Empty;

            var existBeforeDelete = await _survivorRepository.ExistAsync(idSurvivor);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _survivorRepository.DeleteAsync(idSurvivor);

            var existAfterDelete = await _survivorRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}