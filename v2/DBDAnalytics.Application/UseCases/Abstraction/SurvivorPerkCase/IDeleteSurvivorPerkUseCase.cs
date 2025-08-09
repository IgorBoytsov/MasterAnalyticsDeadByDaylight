namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCase
{
    public interface IDeleteSurvivorPerkUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idPerk);
    }
}