using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.PatchCase
{
    public interface IUpdatePatchUseCase
    {
        Task<(PatchDTO? PatchDTO, string? Message)> UpdateAsync(int idPatch, string patchNumber, DateOnly patchDateRelease);
        Task<PatchDTO?> ForcedUpdateAsync(int idPatch, string patchNumber, DateOnly patchDateRelease);
    }
}