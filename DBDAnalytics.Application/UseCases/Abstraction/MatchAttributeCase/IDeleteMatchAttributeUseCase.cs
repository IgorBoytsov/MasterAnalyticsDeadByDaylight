namespace DBDAnalytics.Application.UseCases.Abstraction.MatchAttributeCase
{
    public interface IDeleteMatchAttributeUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idMap);
    }
}