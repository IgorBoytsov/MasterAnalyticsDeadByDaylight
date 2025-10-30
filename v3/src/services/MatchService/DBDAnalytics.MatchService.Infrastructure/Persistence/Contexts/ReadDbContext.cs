using DBDAnalytics.MatchService.Application.Abstractions.Contexts;
using DBDAnalytics.MatchService.Application.Abstractions.Models;
using DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Read;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Contexts
{
    public sealed class ReadDbContext(DbContextOptions<ReadDbContext> options) : DbContext(options), IReadDbContext
    {
        private DbSet<MatchListItemView> _matchListItems { get; set; } = null!;
        private DbSet<MatchDetailsView> _matchDetails { get; set; } = null!;

        public IQueryable<MatchListItemView> MatchListItems => _matchListItems;
        public IQueryable<MatchDetailsView> MatchDetails => _matchDetails;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MatchListItemReadConfiguration());
            modelBuilder.ApplyConfiguration(new MatchDetailsReadConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
            => throw new InvalidOperationException("Данные DbContext только для чтение. Используйте WriteDbContext для сохранения изменений.");

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
            => throw new InvalidOperationException("Данные DbContext только для чтение. Используйте WriteDbContext для сохранения изменений.");

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => throw new InvalidOperationException("Данные DbContext только для чтение. Используйте WriteDbContext для сохранения изменений.");

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
            => throw new InvalidOperationException("Данные DbContext только для чтение. Используйте WriteDbContext для сохранения изменений.");
    }
}