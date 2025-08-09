using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.PatchCase
{
    public interface IGetPatchUseCase
    {
        List<PatchDTO> GetAll();
        Task<List<PatchDTO>> GetAllAsync();
        Task<PatchDTO?> GetAsync(int idPatch);
    }
}