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
using DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Perk;
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
using DBDAnalytics.MatchService.Client.ApiClients;
using DBDAnalytics.UserService.Client.ApiClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using UserServiceImplementation = DBDAnalytics.UserService.Client.ApiClients.UserService;

namespace DBDAnalytics.Client.WPF.Ioc
{
    internal static class HttpServiceConfigurations
    {
        public static void AddHttpService(this IServiceCollection service, IConfiguration configuration)
        {
            string? catalogServiceApiUrl = configuration.GetValue<string>("BaseCatalogApiServiceUrl");
            string? matchServiceApiUrl = configuration.GetValue<string>("BaseMatchApiServiceUrl");
            string? userServiceApiUrl = configuration.GetValue<string>("BaseUserApiServiceUrl");

            if (string.IsNullOrEmpty(catalogServiceApiUrl))
                throw new InvalidOperationException("BaseCatalogApiServiceUrl не сконфигурирована.");

            if (string.IsNullOrEmpty(matchServiceApiUrl))
                throw new InvalidOperationException("BaseMatchApiServiceUrl не сконфигурирована.");

            if (string.IsNullOrEmpty(userServiceApiUrl))
                throw new InvalidOperationException("BaseUserApiServiceUrl не сконфигурирована.");

            const string catalogApiClientName = "CatalogApiClient";
            service.AddHttpClient(catalogApiClientName, client => client.BaseAddress = new Uri(catalogServiceApiUrl));

            const string matchApiClientName = "MatchApiClient";
            service.AddHttpClient(matchApiClientName, client => client.BaseAddress = new Uri(matchServiceApiUrl));

            const string userApiClientName = "UserApiClient";
            service.AddHttpClient(userApiClientName, client => client.BaseAddress = new Uri(userServiceApiUrl));

            service.AddHttpClient<IPlayerAssociationReadOnlyService, AssociationApiService>(catalogApiClientName);
            service.AddHttpClient<IPlatformReadOnlyService, PlatformApiService>(catalogApiClientName);
            service.AddHttpClient<IRoleReadOnlyService, RoleApiService>(catalogApiClientName);
            service.AddHttpClient<IRarityReadOnlyService, RarityApiService>(catalogApiClientName);
            service.AddHttpClient<ITypeDeathDeadOnlyService, TypeDeathApiService>(catalogApiClientName);
            service.AddHttpClient<IWhoPlacedMapReadOnlyService, WhoPlacedMapApiService>(catalogApiClientName);
            service.AddHttpClient<IPatchReadOnlyService, PatchApiService>(catalogApiClientName);
            service.AddHttpClient<IOfferingCategoryReadOnlyService, OfferingCategoryApiService>(catalogApiClientName);
            service.AddHttpClient<ISurvivorPerkCategoryReadOnlyService, SurvivorPerkCategoryApiService>(catalogApiClientName);
            service.AddHttpClient<IKillerPerkCategoryReadOnlyService, KillerPerkCategoryApiService>(catalogApiClientName);
            service.AddHttpClient<IGameEventReadOnlyService, GameEventApiService>(catalogApiClientName);
            service.AddHttpClient<IGameModeReadOnlyService, GameModeApiService>(catalogApiClientName);
            service.AddHttpClient<IMeasurementReadOnlyService, MeasurementApiService>(catalogApiClientName);

            service.AddHttpClient<IOfferingReadOnlyService, OfferingApiService>(catalogApiClientName);
            service.AddHttpClient<IKillerReadOnlyService, KillerApiService>(catalogApiClientName);
            service.AddHttpClient<IItemReadOnlyApiService, ItemApiService>(catalogApiClientName);
            service.AddHttpClient<ISurvivorReadOnlyService, SurvivorApiService>(catalogApiClientName);

            service.AddHttpClient<CatalogService.Client.ApiClients.Loadout.Perk.IKillerPerkReadOnlyService, KillerPerkReadOnlyService>(catalogApiClientName);
            service.AddHttpClient<CatalogService.Client.ApiClients.Loadout.Perk.ISurvivorPerkReadOnlyService, SurvivorPerkReadOnlyService>(catalogApiClientName);
            service.AddHttpClient<IMapReadOnlyService, MapApiService>(catalogApiClientName);

            service.AddScoped<IMeasurementMapReadOnlyApiServiceFactory, MeasurementMapReadOnlyApiServiceFactory>();
            service.AddScoped<IKillerPerkReadOnlyApiServiceFactory, KillerPerkReadOnlyApiServiceFactory>();
            service.AddScoped<IKillerAddonReadOnlyApiServiceFactory, KillerAddonReadOnlyApiServiceFactory>();
            service.AddScoped<IItemAddonReadOnlyApiServiceFactory, ItemAddonReadOnlyApiServiceFactory>();
            service.AddScoped<ISurvivorPerkReadOnlyApiServiceFactory, SurvivorPerkReadOnlyApiServiceFactory>();

            service.AddHttpClient<IMatchService, MatchApiService>(matchApiClientName);

            service.AddHttpClient<IUserService, UserApiService>(userApiClientName);
        }
    }
}