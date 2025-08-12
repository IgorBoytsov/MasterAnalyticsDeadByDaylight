namespace Shared.Api.Infrastructure.EFBase
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Remove(TEntity entity);
    }
}