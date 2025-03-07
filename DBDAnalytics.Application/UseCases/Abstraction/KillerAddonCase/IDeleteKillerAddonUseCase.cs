namespace DBDAnalytics.Application.UseCases.Abstraction.KillerAddonCase
{
    public interface IDeleteKillerAddonUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idKillerAddon);
    }
}