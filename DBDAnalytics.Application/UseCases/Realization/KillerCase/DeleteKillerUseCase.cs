using DBDAnalytics.Application.UseCases.Abstraction.KillerCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerCase
{
    public class DeleteKillerUseCase(IKillerRepository killerRepository) : IDeleteKillerUseCase
    {
        private readonly IKillerRepository _killerRepository = killerRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idKiller)
        {
            string message = string.Empty;

            var existBeforeDelete = await _killerRepository.ExistAsync(idKiller);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _killerRepository.DeleteAsync(idKiller);

            var existAfterDelete = await _killerRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}