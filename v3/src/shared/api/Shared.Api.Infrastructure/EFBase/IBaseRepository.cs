using DBDAnalytics.Shared.Domain.Primitives;

namespace Shared.Api.Infrastructure.EFBase
{
    public interface IBaseRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Remove(TEntity entity);
    }
}