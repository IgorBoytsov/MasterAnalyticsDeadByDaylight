namespace DBDAnalytics.Application.UseCases.Abstraction.WhoPlacedMapCase
{
    public interface IDeleteWhoPlacedMapUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idWhoPlacedMap);
    }
}