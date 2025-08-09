using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.ItemCase
{
    public interface IGetItemWithAddonsUseCase
    {
        List<ItemWithAddonsDTO> GetItemsWithAddons();
        Task<List<ItemWithAddonsDTO>> GetItemsWithAddonsAsync();
        ItemWithAddonsDTO GetItemWithAddons(int idItem);
        Task<ItemWithAddonsDTO> GetItemWithAddonsAsync(int idItem);
    }
}