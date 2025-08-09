using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerCase
{
    public interface IGetKillerLoadoutUseCase
    {
        Task<List<KillerLoadoutDTO>> GetKillersWithAddonsAndPerksAsync();
        Task<KillerLoadoutDTO> GetKillerWithAddonsAndPerksAsync(int idKiller);
    }
}