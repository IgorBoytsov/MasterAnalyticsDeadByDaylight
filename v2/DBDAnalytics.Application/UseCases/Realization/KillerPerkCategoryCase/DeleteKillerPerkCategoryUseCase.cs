using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCategoryCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerPerkCategoryCase
{
    public class DeleteKillerPerkCategoryUseCase(IKillerPerkCategoryRepository KillerPerkCategoryRepository) : IDeleteKillerPerkCategoryUseCase
    {
        private readonly IKillerPerkCategoryRepository _KillerPerkCategoryRepository = KillerPerkCategoryRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idKillerPerkCategory)
        {
            string message = string.Empty;

            var existBeforeDelete = await _KillerPerkCategoryRepository.ExistAsync(idKillerPerkCategory);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _KillerPerkCategoryRepository.DeleteAsync(idKillerPerkCategory);

            var existAfterDelete = await _KillerPerkCategoryRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}