namespace DBDAnalytics.Application.UseCases.Abstraction.RarityCase
{
    public interface IDeleteRarityUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idRarity);
    }
}