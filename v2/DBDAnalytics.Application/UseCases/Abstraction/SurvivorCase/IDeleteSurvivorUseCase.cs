namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase
{
    public interface IDeleteSurvivorUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idSurvivor);
    }
}