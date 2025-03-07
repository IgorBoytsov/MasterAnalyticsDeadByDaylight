using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.ItemCase
{
    public interface IGetItemUseCase
    {
        List<ItemDTO> GetAll();
        Task<List<ItemDTO>> GetAllAsync();
        Task<ItemDTO?> GetAsync(int idPlatform);
    }
}