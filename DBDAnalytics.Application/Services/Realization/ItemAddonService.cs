using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.ItemAddonCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class ItemAddonService(ICreateItemAddonUseCase createItemAddonUseCase,
                                  IDeleteItemAddonUseCase deleteItemAddonUseCase,
                                  IGetItemAddonUseCase getItemAddonUseCase,
                                  IUpdateItemAddonUseCase updateItemAddonUseCase) : IItemAddonService
    {
        private readonly ICreateItemAddonUseCase _createItemAddonUseCase = createItemAddonUseCase;
        private readonly IDeleteItemAddonUseCase _deleteItemAddonUseCase = deleteItemAddonUseCase;
        private readonly IGetItemAddonUseCase _getItemAddonUseCase = getItemAddonUseCase;
        private readonly IUpdateItemAddonUseCase _updateItemAddonUseCase = updateItemAddonUseCase;

        public async Task<(ItemAddonDTO? ItemAddonDTO, string? Message)> CreateAsync(int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription)
        {
            return await _createItemAddonUseCase.CreateAsync(idItem, idRarity, itemAddonName, itemAddonImage, itemAddonDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idItem)
        {
            return await _deleteItemAddonUseCase.DeleteAsync(idItem);
        }

        public List<ItemAddonDTO> GetAll()
        {
            return _getItemAddonUseCase.GetAll();
        }

        public async Task<List<ItemAddonDTO>> GetAllAsync()
        {
            return await _getItemAddonUseCase.GetAllAsync();
        }

        public async Task<ItemAddonDTO> GetAsync(int idItemAddon)
        {
            return await _getItemAddonUseCase.GetAsync(idItemAddon);
        }

        public async Task<(ItemAddonDTO? ItemAddonDTO, string? Message)> UpdateAsync(int idItemAddon, int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription)
        {
            return await _updateItemAddonUseCase.UpdateAsync(idItemAddon, idItem, idRarity, itemAddonName, itemAddonImage, itemAddonDescription);
        }
    }
}