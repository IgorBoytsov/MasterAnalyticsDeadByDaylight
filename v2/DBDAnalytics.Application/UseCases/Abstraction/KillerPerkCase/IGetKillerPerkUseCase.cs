using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCase
{
    public interface IGetKillerPerkUseCase
    {
        List<KillerPerkDTO> GetAll();
        Task<List<KillerPerkDTO>> GetAllAsync();
        Task<KillerPerkDTO?> GetAsync(int idPerk);
    }
}