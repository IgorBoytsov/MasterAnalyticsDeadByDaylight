using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.RarityCase
{
    public interface IGetRarityUseCase
    {
        List<RarityDTO> GetAll();
        Task<List<RarityDTO>> GetAllAsync();
        Task<RarityDTO?> GetAsync(int idRarity);
    }
}