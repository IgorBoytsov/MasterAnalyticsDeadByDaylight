using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IItemAddonService
    {
        Task<(ItemAddonDTO? ItemAddonDTO, string? Message)> CreateAsync(int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idItem);
        List<ItemAddonDTO> GetAll();
        Task<List<ItemAddonDTO>> GetAllAsync();
        Task<ItemAddonDTO> GetAsync(int idItemAddon);
        Task<(ItemAddonDTO? ItemAddonDTO, string? Message)> UpdateAsync(int idItemAddon, int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription);
    }
}