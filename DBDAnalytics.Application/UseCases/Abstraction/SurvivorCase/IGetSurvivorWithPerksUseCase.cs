using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase
{
    public interface IGetSurvivorWithPerksUseCase
    {
        Task<List<SurvivorWithPerksDTO>> GetSurvivorsWithPerksAsync();
        Task<SurvivorWithPerksDTO> GetSurvivorWithPerksAsync(int idSurvivor);
    }
}