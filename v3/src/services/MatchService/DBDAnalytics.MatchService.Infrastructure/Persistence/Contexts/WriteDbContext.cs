using DBDAnalytics.MatchService.Application.Abstractions.Common;
using DBDAnalytics.MatchService.Application.Abstractions.Contexts;
using DBDAnalytics.MatchService.Domain.Models;
using DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Write;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Contexts
{
    public sealed class WriteDbContext(DbContextOptions<WriteDbContext> options) : 
        DbContext(options), IWriteDbContext, IUnitOfWork
    {
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MatchWriteConfiguration());
            modelBuilder.ApplyConfiguration(new KillerPerformanceWriteConfiguration());
            modelBuilder.ApplyConfiguration(new SurvivorPerformanceWriteConfiguration());
            modelBuilder.ApplyConfiguration(new KillerPerkWriteConfiguration());
            modelBuilder.ApplyConfiguration(new KillerAddonWriteConfiguration());
            modelBuilder.ApplyConfiguration(new SurvivorPerkWriteConfiguration());
            modelBuilder.ApplyConfiguration(new SurvivorItemWriteConfiguration());
            modelBuilder.ApplyConfiguration(new SurvivorItemAddonWriteConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}