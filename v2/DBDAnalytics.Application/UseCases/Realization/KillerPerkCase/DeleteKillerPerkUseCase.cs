using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerPerkCase
{
    public class DeleteKillerPerkUseCase(IKillerPerkRepository killerPerkRepository) : IDeleteKillerPerkUseCase
    {
        private readonly IKillerPerkRepository _killerPerkRepository = killerPerkRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idPerk)
        {
            string message = string.Empty;

            var existBeforeDelete = await _killerPerkRepository.ExistAsync(idPerk);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _killerPerkRepository.DeleteAsync(idPerk);

            var existAfterDelete = await _killerPerkRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}