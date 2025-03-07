using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.PatchCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class PatchService(ICreatePatchUseCase createPatchUseCase,
                              IDeletePatchUseCase deletePatchUseCase,
                              IGetPatchUseCase getPatchUseCase,
                              IUpdatePatchUseCase updatePatchUseCase) : IPatchService
    {
        private readonly ICreatePatchUseCase _createPatchUseCase = createPatchUseCase;
        private readonly IDeletePatchUseCase _deletePatchUseCase = deletePatchUseCase;
        private readonly IGetPatchUseCase _getPatchUseCase = getPatchUseCase;
        private readonly IUpdatePatchUseCase _updatePatchUseCase = updatePatchUseCase;

        public async Task<(PatchDTO? PatchDTO, string Message)> CreateAsync(string patchNumber, DateOnly patchDateRelease)
        {
            return await _createPatchUseCase.CreateAsync(patchNumber, patchDateRelease);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idPatch)
        {
            return await _deletePatchUseCase.DeleteAsync(idPatch);
        }

        public List<PatchDTO> GetAll()
        {
            return _getPatchUseCase.GetAll();
        }

        public async Task<List<PatchDTO>> GetAllAsync()
        {
            return await _getPatchUseCase.GetAllAsync();
        }

        public async Task<PatchDTO> GetAsync(int idPatch)
        {
            return await _getPatchUseCase.GetAsync(idPatch);
        }

        public async Task<(PatchDTO? PatchDTO, string? Message)> UpdateAsync(int idPatch, string patchNumber, DateOnly patchDateRelease)
        {
            return await _updatePatchUseCase.UpdateAsync(idPatch, patchNumber, patchDateRelease);
        }

        public async Task<PatchDTO> ForcedUpdateAsync(int idPatch, string patchNumber, DateOnly patchDateRelease)
        {
            return await _updatePatchUseCase.ForcedUpdateAsync(idPatch, patchNumber, patchDateRelease);
        }
    }
}
