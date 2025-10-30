namespace DBDAnalytics.MatchService.Application.Abstractions.Common
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}