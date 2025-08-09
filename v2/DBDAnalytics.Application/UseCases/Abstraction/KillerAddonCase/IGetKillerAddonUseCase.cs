using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerAddonCase
{
    public interface IGetKillerAddonUseCase
    {
        List<KillerAddonDTO> GetAll();
        Task<List<KillerAddonDTO>> GetAllAsync();
        Task<KillerAddonDTO?> GetAsync(int idKillerAddon);
        Task<List<KillerAddonDTO>> GetAllByIdKiller(int idKiller);
    }
}