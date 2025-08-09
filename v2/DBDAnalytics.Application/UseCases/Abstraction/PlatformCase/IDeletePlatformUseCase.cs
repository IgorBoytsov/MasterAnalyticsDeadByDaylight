namespace DBDAnalytics.Application.UseCases.Abstraction.PlatformCase
{
    public interface IDeletePlatformUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idPlatform);
    }
}