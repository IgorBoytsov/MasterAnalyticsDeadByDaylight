namespace Shared.Api.Infrastructure.EFBase
{
    public interface IBaseReadDbContext
    {
        IQueryable<TEntity> Set<TEntity>() where TEntity : class;
    }
}