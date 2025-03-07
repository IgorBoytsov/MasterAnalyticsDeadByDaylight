using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase
{
    public interface IGetSurvivorUseCase
    {
        List<SurvivorDTO> GetAll();
        Task<List<SurvivorDTO>> GetAllAsync();
        Task<SurvivorDTO?> GetAsync(int idSurvivor);
    }
}