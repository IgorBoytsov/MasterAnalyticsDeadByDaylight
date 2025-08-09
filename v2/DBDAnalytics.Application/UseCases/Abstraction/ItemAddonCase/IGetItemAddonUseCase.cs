using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.ItemAddonCase
{
    public interface IGetItemAddonUseCase
    {
        List<ItemAddonDTO> GetAll();
        Task<List<ItemAddonDTO>> GetAllAsync();
        Task<ItemAddonDTO?> GetAsync(int idItemAddon);
    }
}