using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.PatchCase
{
    public interface ICreatePatchUseCase
    {
        Task<(PatchDTO? PatchDTO, string? Message)> CreateAsync(string patchNumber, DateOnly patchDateRelease, string? description);
    }
}