using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DBDAnalytics.Infrastructure.Ioc
{
    public static class InfrastructureServiceCollectionRegExtensions
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<DBDContext>();
            services.AddSingleton<Func<DBDContext>>(provider => () => provider.GetRequiredService<DBDContext>());

            services.AddScoped<IAssociationRepository, AssociationRepository>();
            services.AddScoped<IGameEventRepository, GameEventRepository>();
            services.AddScoped<IGameModeRepository, GameModeRepository>();

            services.AddScoped<IGameStatisticRepository, GameStatisticRepository>();
            services.AddScoped<IGameStatisticKillerViewingRepository, GameStatisticKillerViewingRepository>();
            services.AddScoped<IGameStatisticSurvivorViewingRepository, GameStatisticSurvivorViewingRepository>();
            services.AddScoped<IMatchAttributeRepository, MatchAttributeRepository>();

            services.AddScoped<IItemAddonRepository, ItemAddonRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();

            services.AddScoped<IKillerAddonRepository, KillerAddonRepository>();
            services.AddScoped<IKillerInfoRepository, KillerInfoRepository>();
            services.AddScoped<IKillerPerkCategoryRepository, KillerPerkCategoryRepository>();
            services.AddScoped<IKillerPerkRepository, KillerPerkRepository>();
            services.AddScoped<IKillerRepository, KillerRepository>();

            services.AddScoped<IMapRepository, MapRepository>();
            services.AddScoped<IMeasurementRepository, MeasurementRepository>();

            services.AddScoped<IOfferingCategoryRepository, OfferingCategoryRepository>();
            services.AddScoped<IOfferingRepository, OfferingRepository>();

            services.AddScoped<IPatchRepository, PatchRepository>();
            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddScoped<IRarityRepository, RarityRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<ISurvivorInfoRepository, SurvivorInfoRepository>();
            services.AddScoped<ISurvivorPerkCategoryRepository, SurvivorPerkCategoryRepository>();
            services.AddScoped<ISurvivorPerkRepository, SurvivorPerkRepository>();
            services.AddScoped<ISurvivorRepository, SurvivorRepository>();

            services.AddScoped<ITypeDeathRepository, TypeDeathRepository>();
            services.AddScoped<IWhoPlacedMapRepository, WhoPlacedMapRepository>();

            services.AddScoped<IDetailsMatchRepository, DetailsMatchRepository>();

            return services;
        }
    }
}