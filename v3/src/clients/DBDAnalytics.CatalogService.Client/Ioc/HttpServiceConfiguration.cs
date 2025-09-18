using DBDAnalytics.CatalogService.Client.ApiClients.Characters.Killer;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerAddon;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerk;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerkCategory;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.Survivor;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerk;
using DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerkCategory;
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Item;
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.ItemAddon;
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Offering;
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.OfferingCategory;
using DBDAnalytics.CatalogService.Client.ApiClients.Matches.GameEvent;
using DBDAnalytics.CatalogService.Client.ApiClients.Matches.GameMode;
using DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map;
using DBDAnalytics.CatalogService.Client.ApiClients.Matches.Measurement;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Associations;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Patch;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Platform;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Rarity;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.Role;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.TypeDeath;
using DBDAnalytics.CatalogService.Client.ApiClients.Shared.WhoPlacedMap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DBDAnalytics.CatalogService.Client.Ioc
{
    public static class HttpServiceConfiguration
    {
        public static IServiceCollection ConfigureCatalogApiHttpService(this IServiceCollection services, IConfiguration configuration)
        {
            string? catalogServiceApiUrl = configuration.GetValue<string>("BaseCatalogApiServiceUrl");

            if (string.IsNullOrEmpty(catalogServiceApiUrl))
                throw new InvalidOperationException("BaseCatalogApiServiceUrl не сконфигурирована.");

            const string catalogApiClientName = "CatalogApiClient";
            services.AddHttpClient(catalogApiClientName, client => client.BaseAddress = new Uri(catalogServiceApiUrl));

            services.AddHttpClient<IPlayerAssociationService, AssociationApiService>(catalogApiClientName);
            services.AddHttpClient<IPlatformService, PlatformApiService>(catalogApiClientName);
            services.AddHttpClient<IRoleService, RoleApiService>(catalogApiClientName);
            services.AddHttpClient<IRarityService, RarityApiService>(catalogApiClientName);
            services.AddHttpClient<ITypeDeathService, TypeDeathApiService>(catalogApiClientName);
            services.AddHttpClient<IWhoPlacedMapService, WhoPlacedMapApiService>(catalogApiClientName);
            services.AddHttpClient<IPatchService, PatchApiService>(catalogApiClientName);
            services.AddHttpClient<IOfferingCategoryService, OfferingCategoryApiService>(catalogApiClientName);
            services.AddHttpClient<ISurvivorPerkCategoryService, SurvivorPerkCategoryApiService>(catalogApiClientName);
            services.AddHttpClient<IKillerPerkCategoryService, KillerPerkCategoryApiService>(catalogApiClientName);
            services.AddHttpClient<IGameEventService, GameEventApiService>(catalogApiClientName);
            services.AddHttpClient<IGameModeService, GameModeApiService>(catalogApiClientName);
            services.AddHttpClient<IMeasurementService, MeasurementApiService>(catalogApiClientName);

            services.AddHttpClient<IOfferingService, OfferingApiService>(catalogApiClientName);
            services.AddHttpClient<IKillerService, KillerApiService>(catalogApiClientName);
            services.AddHttpClient<IItemService, ItemApiService>(catalogApiClientName);
            services.AddHttpClient<ISurvivorService, SurvivorApiService>(catalogApiClientName);

            services.AddScoped<IMapApiServiceFactory, MapApiServiceFactory>();
            services.AddScoped<IKillerPerkApiServiceFactory, KillerPerkApiServiceFactory>();
            services.AddScoped<IKillerAddonApiServiceFactory, KillerAddonApiServiceFactory>();
            services.AddScoped<IItemAddonApiServiceFactory, ItemAddonApiServiceFactory>();
            services.AddScoped<ISurvivorPerkApiServiceFactory, SurvivorPerkApiServiceFactory>();

            return services;
        }
    }
}