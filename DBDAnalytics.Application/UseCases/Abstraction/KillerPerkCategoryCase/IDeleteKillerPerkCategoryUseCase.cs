namespace DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCategoryCase
{
    public interface IDeleteKillerPerkCategoryUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idKillerPerkCategory);
    }
}