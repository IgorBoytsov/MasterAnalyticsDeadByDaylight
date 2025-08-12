using Microsoft.EntityFrameworkCore;

namespace Shared.Api.Infrastructure.EFBase
{
    public interface IBaseDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}