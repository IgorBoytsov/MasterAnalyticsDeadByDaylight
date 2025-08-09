using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.ItemCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class ItemService(ICreateItemUseCase createItemUseCase,
                             IDeleteItemUseCase deleteItemUseCase,
                             IGetItemUseCase getItemUseCase,
                             IGetItemWithAddonsUseCase getItemWithAddonsUseCase,
                             IUpdateItemUseCase updateItemUseCase) : IItemService
    {
        private readonly ICreateItemUseCase _createItemUseCase = createItemUseCase;
        private readonly IDeleteItemUseCase _deleteItemUseCase = deleteItemUseCase;
        private readonly IGetItemUseCase _getItemUseCase = getItemUseCase;
        private readonly IGetItemWithAddonsUseCase _getItemWithAddonsUseCase = getItemWithAddonsUseCase;
        private readonly IUpdateItemUseCase _updateItemUseCase = updateItemUseCase;

        public async Task<(ItemDTO? ItemDTO, string Message)> CreateAsync(string itemName, byte[]? itemImage, string? itemDescription)
        {
            return await _createItemUseCase.CreateAsync(itemName, itemImage, itemDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idItem)
        {
            return await _deleteItemUseCase.DeleteAsync(idItem);
        }

        public List<ItemDTO> GetAll()
        {
            return _getItemUseCase.GetAll();
        }

        public async Task<List<ItemDTO>> GetAllAsync()
        {
            return await _getItemUseCase.GetAllAsync();
        }

        public async Task<ItemDTO> GetAsync(int idItem)
        {
            return await _getItemUseCase.GetAsync(idItem);
        }

        public List<ItemWithAddonsDTO> GetItemsWithAddons()
        {
            return _getItemWithAddonsUseCase.GetItemsWithAddons();
        }

        public async Task<List<ItemWithAddonsDTO>> GetItemsWithAddonsAsync()
        {
            return await _getItemWithAddonsUseCase.GetItemsWithAddonsAsync();
        }

        public ItemWithAddonsDTO GetItemWithAddons(int idItem)
        {
            return _getItemWithAddonsUseCase.GetItemWithAddons(idItem);
        }

        public async Task<ItemWithAddonsDTO> GetItemWithAddonsAsync(int idItem)
        {
            return await _getItemWithAddonsUseCase.GetItemWithAddonsAsync(idItem);
        }

        public async Task<(ItemDTO? ItemDTO, string? Message)> UpdateAsync(int idItem, string itemName, byte[]? itemImage, string? itemDescription)
        {
            return await _updateItemUseCase.UpdateAsync(idItem, itemName, itemImage, itemDescription);
        }

        public async Task<ItemDTO> ForcedUpdateAsync(int idItem, string itemName, byte[]? itemImage, string? itemDescription)
        {
            return await _updateItemUseCase.ForcedUpdateAsync(idItem, itemName, itemImage, itemDescription);
        }
    }
}