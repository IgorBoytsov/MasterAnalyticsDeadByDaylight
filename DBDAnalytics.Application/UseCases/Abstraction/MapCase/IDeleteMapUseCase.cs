namespace DBDAnalytics.Application.UseCases.Abstraction.MapCase
{
    public interface IDeleteMapUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idMap);
    }
}