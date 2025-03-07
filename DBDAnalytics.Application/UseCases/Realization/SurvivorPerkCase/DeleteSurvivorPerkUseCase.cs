using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorPerkCase
{
    public class DeleteSurvivorPerkUseCase(ISurvivorPerkRepository survivorPerkRepository) : IDeleteSurvivorPerkUseCase
    {
        private readonly ISurvivorPerkRepository _survivorPerkRepository = survivorPerkRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idPerk)
        {
            string message = string.Empty;

            var existBeforeDelete = await _survivorPerkRepository.ExistAsync(idPerk);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _survivorPerkRepository.DeleteAsync(idPerk);

            var existAfterDelete = await _survivorPerkRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}