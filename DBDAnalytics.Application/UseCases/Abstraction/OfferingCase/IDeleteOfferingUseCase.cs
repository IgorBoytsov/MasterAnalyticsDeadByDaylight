namespace DBDAnalytics.Application.UseCases.Abstraction.OfferingCase
{
    public interface IDeleteOfferingUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idOffering);
    }
}