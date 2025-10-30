using DBDAnalytics.MatchService.Application.Abstractions.Contexts;
using DBDAnalytics.MatchService.Application.Abstractions.Repositories;
using DBDAnalytics.MatchService.Infrastructure.Persistence.Contexts;
using DBDAnalytics.MatchService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DBDAnalytics.MatchService.Infrastructure.Ioc
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<WriteDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IWriteDbContext>(provider => provider.GetRequiredService<WriteDbContext>());

            services.AddDbContext<ReadDbContext>(option =>
            {
                option.UseNpgsql(connectionString);
                option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IReadDbContext>(provider => provider.GetRequiredService<ReadDbContext>());

            services.AddScoped<IMatchRepository, MatchRepository>();

            return services;
        }
    }
}