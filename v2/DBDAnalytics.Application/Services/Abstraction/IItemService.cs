using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IItemService
    {
        Task<(ItemDTO? ItemDTO, string Message)> CreateAsync(string itemName, byte[]? itemImage, string? itemDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idItem);
        Task<ItemDTO> ForcedUpdateAsync(int idItem, string itemName, byte[]? itemImage, string? itemDescription);
        List<ItemDTO> GetAll();
        Task<List<ItemDTO>> GetAllAsync();
        Task<ItemDTO> GetAsync(int idPlatform);
        List<ItemWithAddonsDTO> GetItemsWithAddons();
        Task<List<ItemWithAddonsDTO>> GetItemsWithAddonsAsync();
        ItemWithAddonsDTO GetItemWithAddons(int idItem);
        Task<ItemWithAddonsDTO> GetItemWithAddonsAsync(int idItem);
        Task<(ItemDTO? ItemDTO, string? Message)> UpdateAsync(int idItem, string itemName, byte[]? itemImage, string? itemDescription);
    }
}