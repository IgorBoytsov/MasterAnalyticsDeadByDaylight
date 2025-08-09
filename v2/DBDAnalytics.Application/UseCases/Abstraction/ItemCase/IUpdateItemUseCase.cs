using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.ItemCase
{
    public interface IUpdateItemUseCase
    {
        Task<ItemDTO> ForcedUpdateAsync(int idItem, string itemName, byte[]? itemImage, string? itemDescription);
        Task<(ItemDTO? ItemDTO, string? Message)> UpdateAsync(int idItem, string itemName, byte[]? itemImage, string? itemDescription);
    }
}