namespace DBDAnalytics.Application.UseCases.Abstraction.KillerCase
{
    public interface IDeleteKillerUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idKiller);
    }
}