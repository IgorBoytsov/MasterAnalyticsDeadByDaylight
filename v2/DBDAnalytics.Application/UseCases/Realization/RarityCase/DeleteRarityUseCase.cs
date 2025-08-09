using DBDAnalytics.Application.UseCases.Abstraction.RarityCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.RarityCase
{
    public class DeleteRarityUseCase(IRarityRepository rarityRepository) : IDeleteRarityUseCase
    {
        private readonly IRarityRepository _rarityRepository = rarityRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idRarity)
        {
            string message = string.Empty;

            var existBeforeDelete = await _rarityRepository.ExistAsync(idRarity);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _rarityRepository.DeleteAsync(idRarity);

            var existAfterDelete = await _rarityRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}
