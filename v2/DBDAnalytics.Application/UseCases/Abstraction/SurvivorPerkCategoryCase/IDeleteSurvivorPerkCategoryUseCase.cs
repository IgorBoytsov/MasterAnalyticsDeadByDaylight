namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCategoryCase
{
    public interface IDeleteSurvivorPerkCategoryUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idSurvivorPerkCategory);
    }
}