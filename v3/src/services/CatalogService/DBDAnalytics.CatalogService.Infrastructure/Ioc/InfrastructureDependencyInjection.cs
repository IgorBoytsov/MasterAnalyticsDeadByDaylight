using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Infrastructure.EF.Contexts;
using DBDAnalytics.CatalogService.Infrastructure.Repositories;
using DBDAnalytics.CatalogService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace DBDAnalytics.CatalogService.Infrastructure.Ioc
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CatalogContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IApplicationDbContext>(provider =>
                    provider.GetRequiredService<CatalogContext>());

            services.AddMinio(cfg =>
            {
                var miniConfig = configuration.GetSection("Minio");

                cfg.WithEndpoint(miniConfig["Endpoint"])
                    .WithCredentials(miniConfig["AccessKey"], miniConfig["SecretKey"])
                    .WithSSL(bool.Parse(miniConfig["UseSsl"] ?? "false"));
            });

            services.AddScoped<IFileStorageService, MinioFileStorageService>();
            services.AddScoped<IFileUploadManager, FileUploadManager>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRarityRepository, RarityRepository>();

            services.AddScoped<IGameModeRepository, GameModeRepository>();
            services.AddScoped<IGameEventRepository, GameEventRepository>();

            services.AddScoped<IMeasurementRepository, MeasurementRepository>();
            services.AddScoped<IWhoPlacedMapRepository, WhoPlacedMapRepository>();

            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddScoped<ITypeDeathRepository, TypeDeathRepository>();
            services.AddScoped<IPlayerAssociationRepository, PlayerAssociationRepository>();

            services.AddScoped<IOfferingRepository, OfferingRepository>();
            services.AddScoped<IOfferingCategoryRepository, OfferingCategoryRepository>();

            services.AddScoped<IKillerRepository, KillerRepository>();
            services.AddScoped<IKillerPerkCategoryRepository, KillerPerkCategoryRepository>();

            services.AddScoped<ISurvivorRepository, SurvivorRepository>();
            services.AddScoped<ISurvivorPerkCategoryRepository, SurvivorPerkCategoryRepository>();

            return services;
        }
    }
}