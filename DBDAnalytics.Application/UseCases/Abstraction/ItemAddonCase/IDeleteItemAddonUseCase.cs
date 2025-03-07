namespace DBDAnalytics.Application.UseCases.Abstraction.ItemAddonCase
{
    public interface IDeleteItemAddonUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idItemAddon);
    }
}