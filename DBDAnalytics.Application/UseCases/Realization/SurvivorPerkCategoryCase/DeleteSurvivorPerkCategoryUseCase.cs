using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCategoryCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorPerkCategoryCase
{
    public class DeleteSurvivorPerkCategoryUseCase(ISurvivorPerkCategoryRepository survivorPerkCategoryRepository) : IDeleteSurvivorPerkCategoryUseCase
    {
        private readonly ISurvivorPerkCategoryRepository _survivorPerkCategoryRepository = survivorPerkCategoryRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idSurvivorPerkCategory)
        {
            string message = string.Empty;

            var existBeforeDelete = await _survivorPerkCategoryRepository.ExistAsync(idSurvivorPerkCategory);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _survivorPerkCategoryRepository.DeleteAsync(idSurvivorPerkCategory);

            var existAfterDelete = await _survivorPerkCategoryRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}
