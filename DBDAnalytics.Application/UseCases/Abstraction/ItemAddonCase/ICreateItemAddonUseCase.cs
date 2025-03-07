using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.ItemAddonCase
{
    public interface ICreateItemAddonUseCase
    {
        Task<(ItemAddonDTO? ItemAddonDTO, string? Message)> CreateAsync(int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription);
    }
}