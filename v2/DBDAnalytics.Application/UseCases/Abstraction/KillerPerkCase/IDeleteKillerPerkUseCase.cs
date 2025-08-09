namespace DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCase
{
    public interface IDeleteKillerPerkUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idPerk);
    }
}