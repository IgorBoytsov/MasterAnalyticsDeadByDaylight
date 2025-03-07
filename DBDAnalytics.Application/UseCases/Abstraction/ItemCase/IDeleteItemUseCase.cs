namespace DBDAnalytics.Application.UseCases.Abstraction.ItemCase
{
    public interface IDeleteItemUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idItem);
    }
}