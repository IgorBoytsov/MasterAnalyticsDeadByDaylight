namespace DBDAnalytics.Application.UseCases.Abstraction.PatchCase
{
    public interface IDeletePatchUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idPatch);
    }
}