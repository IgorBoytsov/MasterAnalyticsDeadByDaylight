namespace DBDAnalytics.Application.UseCases.Abstraction.TypeDeathCase
{
    public interface IDeleteTypeDeathUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idTypeDeath);
    }
}