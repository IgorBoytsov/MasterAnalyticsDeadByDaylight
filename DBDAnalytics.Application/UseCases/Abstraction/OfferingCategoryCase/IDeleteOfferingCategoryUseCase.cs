namespace DBDAnalytics.Application.UseCases.Abstraction.OfferingCategoryCase
{
    public interface IDeleteOfferingCategoryUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idOfferingCategory);
    }
}