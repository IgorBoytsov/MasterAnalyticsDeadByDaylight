using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.ItemCase
{
    public interface ICreateItemUseCase
    {
        Task<(ItemDTO? ItemDTO, string? Message)> CreateAsync(string itemName, byte[]? itemImage, string? itemDescription);
    }
}