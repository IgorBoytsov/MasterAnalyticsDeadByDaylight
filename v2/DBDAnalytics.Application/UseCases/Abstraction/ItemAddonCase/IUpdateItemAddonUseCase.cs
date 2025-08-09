using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.ItemAddonCase
{
    public interface IUpdateItemAddonUseCase
    {
        Task<(ItemAddonDTO? ItemAddonDTO, string? Message)> UpdateAsync(int idItemAddon, int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription);
    }
}