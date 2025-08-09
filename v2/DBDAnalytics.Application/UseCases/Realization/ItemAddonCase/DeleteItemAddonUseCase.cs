using DBDAnalytics.Application.UseCases.Abstraction.ItemAddonCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.ItemAddonCase
{
    public class DeleteItemAddonUseCase(IItemAddonRepository itemAddonRepository) : IDeleteItemAddonUseCase
    {
        private readonly IItemAddonRepository _itemAddonRepository = itemAddonRepository;

        //TODO : Реализовать проверку при удаление. Существуют ли связанные данные.
        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idItemAddon)
        {
            string message = string.Empty;

            var existBeforeDelete = await _itemAddonRepository.ExistAsync(idItemAddon);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _itemAddonRepository.DeleteAsync(idItemAddon);

            var existAfterDelete = await _itemAddonRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}