using DBDAnalytics.Application.UseCases.Abstraction.ItemCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.ItemCase
{
    public class DeleteItemUseCase(IItemRepository itemRepository) : IDeleteItemUseCase
    {
        private readonly IItemRepository _itemRepository = itemRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idItem)
        {
            string message = string.Empty;

            var existBeforeDelete = await _itemRepository.ExistAsync(idItem);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _itemRepository.DeleteAsync(idItem);

            var existAfterDelete = await _itemRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}