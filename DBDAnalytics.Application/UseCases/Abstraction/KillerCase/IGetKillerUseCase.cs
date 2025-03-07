using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerCase
{
    public interface IGetKillerUseCase
    {
        List<KillerDTO> GetAll();
        Task<List<KillerDTO>> GetAllAsync();
        Task<KillerDTO?> GetAsync(int idKiller);
    }
}