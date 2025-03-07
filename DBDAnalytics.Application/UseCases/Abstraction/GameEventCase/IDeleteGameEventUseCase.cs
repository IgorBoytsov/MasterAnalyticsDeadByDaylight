namespace DBDAnalytics.Application.UseCases.Abstraction.GameEventCase
{
    public interface IDeleteGameEventUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idGameEvent);
    }
}