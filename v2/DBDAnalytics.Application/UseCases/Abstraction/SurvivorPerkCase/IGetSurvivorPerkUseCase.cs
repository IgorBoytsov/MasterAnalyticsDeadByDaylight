using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCase
{
    public interface IGetSurvivorPerkUseCase
    {
        List<SurvivorPerkDTO> GetAll();
        Task<List<SurvivorPerkDTO>> GetAllAsync();
        Task<SurvivorPerkDTO?> GetAsync(int idRole);
    }
}