namespace DBDAnalytics.Application.UseCases.Abstraction.GameModeCase
{
    public interface IDeleteGameModeUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idGameMode);
    }
}