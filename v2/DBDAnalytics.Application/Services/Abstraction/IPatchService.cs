using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IPatchService
    {
        Task<(PatchDTO? PatchDTO, string? Message)> CreateAsync(string patchNumber, DateOnly patchDateRelease, string? description);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idPatch);
        List<PatchDTO> GetAll();
        Task<List<PatchDTO>> GetAllAsync();
        Task<PatchDTO> GetAsync(int idPatch);
        Task<(PatchDTO? PatchDTO, string? Message)> UpdateAsync(int idPatch, string patchNumber, DateOnly patchDateRelease, string? description);
        Task<PatchDTO> ForcedUpdateAsync(int idPatch, string patchNumber, DateOnly patchDateRelease, string? description);
    }
}