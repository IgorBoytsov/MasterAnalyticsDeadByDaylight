using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.PatchCase
{
    public interface IUpdatePatchUseCase
    {
        Task<(PatchDTO? PatchDTO, string? Message)> UpdateAsync(int idPatch, string patchNumber, DateOnly patchDateRelease, string? description);
        Task<PatchDTO?> ForcedUpdateAsync(int idPatch, string patchNumber, DateOnly patchDateRelease, string? description);
    }
}