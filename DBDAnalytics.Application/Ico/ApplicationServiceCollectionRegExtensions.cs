using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.Services.Realization;
using DBDAnalytics.Application.UseCases.Abstraction.AssociationCase;
using DBDAnalytics.Application.UseCases.Abstraction.GameEventCase;
using DBDAnalytics.Application.UseCases.Abstraction.GameModeCase;
using DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase;
using DBDAnalytics.Application.UseCases.Abstraction.ItemAddonCase;
using DBDAnalytics.Application.UseCases.Abstraction.ItemCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerAddonCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerInfoCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCategoryCase;
using DBDAnalytics.Application.UseCases.Abstraction.MapCase;
using DBDAnalytics.Application.UseCases.Abstraction.MatchAttributeCase;
using DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase;
using DBDAnalytics.Application.UseCases.Abstraction.OfferingCase;
using DBDAnalytics.Application.UseCases.Abstraction.OfferingCategoryCase;
using DBDAnalytics.Application.UseCases.Abstraction.PatchCase;
using DBDAnalytics.Application.UseCases.Abstraction.PlatformCase;
using DBDAnalytics.Application.UseCases.Abstraction.RarityCase;
using DBDAnalytics.Application.UseCases.Abstraction.RoleCase;
using DBDAnalytics.Application.UseCases.Abstraction.StatisticCase;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorInfoCase;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCase;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCategoryCase;
using DBDAnalytics.Application.UseCases.Abstraction.TypeDeathCase;
using DBDAnalytics.Application.UseCases.Abstraction.WhoPlacedMapCase;
using DBDAnalytics.Application.UseCases.Realization.AssociationCase;
using DBDAnalytics.Application.UseCases.Realization.GameEventCase;
using DBDAnalytics.Application.UseCases.Realization.GameModeCase;
using DBDAnalytics.Application.UseCases.Realization.GameStatisticCase;
using DBDAnalytics.Application.UseCases.Realization.ItemAddonCase;
using DBDAnalytics.Application.UseCases.Realization.ItemCase;
using DBDAnalytics.Application.UseCases.Realization.KillerAddonCase;
using DBDAnalytics.Application.UseCases.Realization.KillerCase;
using DBDAnalytics.Application.UseCases.Realization.KillerInfoCase;
using DBDAnalytics.Application.UseCases.Realization.KillerPerkCase;
using DBDAnalytics.Application.UseCases.Realization.KillerPerkCategoryCase;
using DBDAnalytics.Application.UseCases.Realization.MapCase;
using DBDAnalytics.Application.UseCases.Realization.MatchAttributeCase;
using DBDAnalytics.Application.UseCases.Realization.MeasurementCase;
using DBDAnalytics.Application.UseCases.Realization.OfferingCase;
using DBDAnalytics.Application.UseCases.Realization.OfferingCategoryCase;
using DBDAnalytics.Application.UseCases.Realization.PatchCase;
using DBDAnalytics.Application.UseCases.Realization.PlatformCase;
using DBDAnalytics.Application.UseCases.Realization.RarityCase;
using DBDAnalytics.Application.UseCases.Realization.RoleCase;
using DBDAnalytics.Application.UseCases.Realization.StatisticCase;
using DBDAnalytics.Application.UseCases.Realization.SurvivorCase;
using DBDAnalytics.Application.UseCases.Realization.SurvivorInfoCase;
using DBDAnalytics.Application.UseCases.Realization.SurvivorPerkCase;
using DBDAnalytics.Application.UseCases.Realization.SurvivorPerkCategoryCase;
using DBDAnalytics.Application.UseCases.Realization.TypeDeathCase;
using DBDAnalytics.Application.UseCases.Realization.WhoPlacedMapCase;
using DBDAnalytics.Infrastructure.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace DBDAnalytics.Application.Ico
{
    public static class ApplicationServiceCollectionRegExtensions
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            #region Сервисы

            services.AddSingleton<IAssociationService, AssociationService>();
            services.AddSingleton<IGameEventService, GameEventService>();
            services.AddSingleton<IGameModeService, GameModeService>();

            services.AddSingleton<IGameStatisticService, GameStatisticService>();
            services.AddSingleton<IMatchAttributeService, MatchAttributeService>();

            services.AddSingleton<IItemAddonService, ItemAddonService>();
            services.AddSingleton<IItemService, ItemService>();

            services.AddSingleton<IKillerInfoService, KillerInfoService>();
            services.AddSingleton<IKillerAddonService, KillerAddonService>();
            services.AddSingleton<IKillerPerkCategoryService, KillerPerkCategoryService>();
            services.AddSingleton<IKillerPerkService, KillerPerkService>();
            services.AddSingleton<IKillerService, KillerService>();

            services.AddSingleton<IMapService, MapService>();
            services.AddSingleton<IMeasurementService, MeasurementService>();

            services.AddSingleton<IOfferingCategoryService, OfferingCategoryService>();
            services.AddSingleton<IOfferingService, OfferingService>();

            services.AddSingleton<IPatchService, PatchService>();
            services.AddSingleton<IPlatformService, PlatformService>();
            services.AddSingleton<IRarityService, RarityService>();
            services.AddSingleton<IRoleService, RoleService>();

            services.AddSingleton<ISurvivorInfoService, SurvivorInfoService>();
            services.AddSingleton<ISurvivorPerkCategoryService, SurvivorPerkCategoryService>();
            services.AddSingleton<ISurvivorPerkService, SurvivorPerkService>();
            services.AddSingleton<ISurvivorService, SurvivorService>();

            services.AddSingleton<ITypeDeathService, TypeDeathService>();
            services.AddSingleton<IWhoPlacedMapService, WhoPlacedMapService>();

            #endregion

            #region UseCases Table

            services.AddScoped<ICreateAssociationUseCase, CreateAssociationUseCase>();
            services.AddScoped<IDeleteAssociationUseCase, DeleteAssociationUseCase>();
            services.AddScoped<IGetAssociationUseCase, GetAssociationUseCase>();
            services.AddScoped<IUpdateAssociationUseCase, UpdateAssociationUseCase>();

            services.AddScoped<ICreateGameEventUseCase, CreateGameEventUseCase>();
            services.AddScoped<IDeleteGameEventUseCase, DeleteGameEventUseCase>();
            services.AddScoped<IGetGameEventUseCase, GetGameEventUseCase>();
            services.AddScoped<IUpdateGameEventUseCase, UpdateGameEventUseCase>();

            services.AddScoped<ICreateGameModeUseCase, CreateGameModeUseCase>();
            services.AddScoped<IDeleteGameModeUseCase, DeleteGameModeUseCase>();
            services.AddScoped<IGetGameModeUseCase, GetGameModeUseCase>();
            services.AddScoped<IUpdateGameModeUseCase, UpdateGameModeUseCase>();

            services.AddScoped<ICreateGameStatisticUseCase, CreateGameStatisticUseCase>();
            services.AddScoped<IGetGameStatisticKillerViewingUseCase, GetGameStatisticKillerViewingUseCase>();
            services.AddScoped<IGetGameStatisticSurvivorViewingUseCase, GetGameStatisticSurvivorViewingUseCase>();

            services.AddScoped<ICreateItemAddonUseCase, CreateItemAddonUseCase>();
            services.AddScoped<IDeleteItemAddonUseCase, DeleteItemAddonUseCase>();
            services.AddScoped<IGetItemAddonUseCase, GetItemAddonUseCase>();
            services.AddScoped<IUpdateItemAddonUseCase, UpdateItemAddonUseCase>();

            services.AddScoped<ICreateItemUseCase, CreateItemUseCase>();
            services.AddScoped<IDeleteItemUseCase, DeleteItemUseCase>();
            services.AddScoped<IGetItemUseCase, GetItemUseCase>();
            services.AddScoped<IGetItemWithAddonsUseCase, GetItemWithAddonsUseCase>();
            services.AddScoped<IUpdateItemUseCase, UpdateItemUseCase>();

            services.AddScoped<ICreateKillerAddonUseCase, CreateKillerAddonUseCase>();
            services.AddScoped<IDeleteKillerAddonUseCase, DeleteKillerAddonUseCase>();
            services.AddScoped<IGetKillerAddonUseCase, GetKillerAddonUseCase>();
            services.AddScoped<IUpdateKillerAddonUseCase, UpdateKillerAddonUseCase>();

            services.AddScoped<ICreateKillerUseCase, CreateKillerUseCase>();
            services.AddScoped<IDeleteKillerUseCase, DeleteKillerUseCase>();
            services.AddScoped<IGetKillerLoadoutUseCase, GetKillerLoadoutUseCase>();
            services.AddScoped<IGetKillerUseCase, GetKillerUseCase>();
            services.AddScoped<IUpdateKillerUseCase, UpdateKillerUseCase>();

            services.AddScoped<ICreateKillerInfoUseCase, CreateKillerInfoUseCase>();

            services.AddScoped<ICreateKillerPerkUseCase, CreateKillerPerkUseCase>();
            services.AddScoped<IDeleteKillerPerkUseCase, DeleteKillerPerkUseCase>();
            services.AddScoped<IGetKillerPerkUseCase, GetKillerPerkUseCase>();
            services.AddScoped<IUpdateKillerPerkUseCase, UpdateKillerPerkUseCase>();

            services.AddScoped<ICreateKillerPerkCategoryUseCase, CreateKillerPerkCategoryUseCase>();
            services.AddScoped<IDeleteKillerPerkCategoryUseCase, DeleteKillerPerkCategoryUseCase>();
            services.AddScoped<IGetKillerPerkCategoryUseCase, GetKillerPerkCategoryUseCase>();
            services.AddScoped<IUpdateKillerPerkCategoryUseCase, UpdateKillerPerkCategoryUseCase>();

            services.AddScoped<ICreateMapUseCase, CreateMapUseCase>();
            services.AddScoped<IDeleteMapUseCase, DeleteMapUseCase>();
            services.AddScoped<IGetMapUseCase, GetMapUseCase>();
            services.AddScoped<IUpdateMapUseCase, UpdateMapUseCase>();

            services.AddScoped<ICreateMeasurementUseCase, CreateMeasurementUseCase>();
            services.AddScoped<IDeleteMeasurementUseCase, DeleteMeasurementUseCase>();
            services.AddScoped<IGetMeasurementUseCase, GetMeasurementUseCase>();
            services.AddScoped<IGetMeasurementWithMapsUseCase, GetMeasurementWithMapsUseCase>();
            services.AddScoped<IUpdateMeasurementUseCase, UpdateMeasurementUseCase>();

            services.AddScoped<ICreateMatchAttributeUseCase, CreateMatchAttributeUseCase>();
            services.AddScoped<IDeleteMatchAttributeUseCase, DeleteMatchAttributeUseCase>();
            services.AddScoped<IGetMatchAttributeUseCase, GetMatchAttributeUseCase>();
            services.AddScoped<IUpdateMatchAttributeUseCase, UpdateMatchAttributeUseCase>();

            services.AddScoped<ICreateOfferingUseCase, CreateOfferingUseCase>();
            services.AddScoped<IDeleteOfferingUseCase, DeleteOfferingUseCase>();
            services.AddScoped<IGetOfferingUseCase, GetOfferingUseCase>();
            services.AddScoped<IUpdateOfferingUseCase, UpdateOfferingUseCase>();

            services.AddScoped<ICreateOfferingCategoryUseCase, CreateOfferingCategoryUseCase>();
            services.AddScoped<IDeleteOfferingCategoryUseCase, DeleteOfferingCategoryUseCase>();
            services.AddScoped<IGetOfferingCategoryUseCase, GetOfferingCategoryUseCase>();
            services.AddScoped<IUpdateOfferingCategoryUseCase, UpdateOfferingCategoryUseCase>();

            services.AddScoped<ICreatePatchUseCase, CreatePatchUseCase>();
            services.AddScoped<IDeletePatchUseCase, DeletePatchUseCase>();
            services.AddScoped<IGetPatchUseCase, GetPatchUseCase>();
            services.AddScoped<IUpdatePatchUseCase, UpdatePatchUseCase>();

            services.AddScoped<ICreatePlatformUseCase, CreatePlatformUseCase>();
            services.AddScoped<IDeletePlatformUseCase, DeletePlatformUseCase>();
            services.AddScoped<IGetPlatformUseCase, GetPlatformUseCase>();
            services.AddScoped<IUpdatePlatformUseCase, UpdatePlatformUseCase>();

            services.AddScoped<ICreateRarityUseCase, CreateRarityUseCase>();
            services.AddScoped<IDeleteRarityUseCase, DeleteRarityUseCase>();
            services.AddScoped<IGetRarityUseCase, GetRarityUseCase>();
            services.AddScoped<IUpdateRarityUseCase, UpdateRarityUseCase>();

            services.AddScoped<ICreateRoleUseCase, CreateRoleUseCase>();
            services.AddScoped<IDeleteRoleUseCase, DeleteRoleUseCase>();
            services.AddScoped<IGetRoleUseCase, GetRoleUseCase>();
            services.AddScoped<IUpdateRoleUseCase, UpdateRoleUseCase>();

            services.AddScoped<ICreateSurvivorUseCase, CreateSurvivorUseCase>();
            services.AddScoped<IDeleteSurvivorUseCase, DeleteSurvivorUseCase>();
            services.AddScoped<IGetSurvivorUseCase, GetSurvivorUseCase>();
            services.AddScoped<IGetSurvivorWithPerksUseCase, GetSurvivorWithPerksUseCase>();
            services.AddScoped<IUpdateSurvivorUseCase, UpdateSurvivorUseCase>();

            services.AddScoped<ICreateSurvivorInfoUseCase, CreateSurvivorInfoUseCase>();
            services.AddScoped<IGetSurvivorInfoUseCase, GetSurvivorInfoUseCase>();

            services.AddScoped<ICreateSurvivorPerkUseCase, CreateSurvivorPerkUseCase>();
            services.AddScoped<IDeleteSurvivorPerkUseCase, DeleteSurvivorPerkUseCase>();
            services.AddScoped<IGetSurvivorPerkUseCase, GetSurvivorPerkUseCase>();
            services.AddScoped<IUpdateSurvivorPerkUseCase, UpdateSurvivorPerkUseCase>();

            services.AddScoped<ICreateSurvivorPerkCategoryUseCase, CreateSurvivorPerkCategoryUseCase>();
            services.AddScoped<IDeleteSurvivorPerkCategoryUseCase, DeleteSurvivorPerkCategoryUseCase>();
            services.AddScoped<IGetSurvivorPerkCategoryUseCase, GetSurvivorPerkCategoryUseCase>();
            services.AddScoped<IUpdateSurvivorPerkCategoryUseCase, UpdateSurvivorPerkCategoryUseCase>();

            services.AddScoped<ICreateTypeDeathUseCase, CreateTypeDeathUseCase>();
            services.AddScoped<IDeleteTypeDeathUseCase, DeleteTypeDeathUseCase>();
            services.AddScoped<IGetTypeDeathUseCase, GetTypeDeathUseCase>();
            services.AddScoped<IUpdateTypeDeathUseCase, UpdateTypeDeathUseCase>();

            services.AddScoped<ICreateWhoPlacedMapUseCase, CreateWhoPlacedMapUseCase>();
            services.AddScoped<IDeleteWhoPlacedMapUseCase, DeleteWhoPlacedMapUseCase>();
            services.AddScoped<IGetWhoPlacedMapUseCase, GetWhoPlacedMapUseCase>();
            services.AddScoped<IUpdateWhoPlacedMapUseCase, UpdateWhoPlacedMapUseCase>();

            #endregion

            #region UseCases Stats

            services.AddScoped<IGetDetailsMatchUseCase, GetDetailsMatchUseCase>();

            #endregion

            #region UseCases DetailsView

            services.AddScoped<IGetDetailsMatchViewUseCase, GetDetailsMatchViewUseCase>();

            #endregion

            #region CalculationService

            services.AddSingleton<ICreatingApplicationModelsService, CreatingApplicationModelsService>();

            services.AddSingleton<ICalculationGeneralService, CalculationGeneralService>();
            services.AddSingleton<ICalculationTimeService, CalculationTimeService>();
            services.AddSingleton<ICalculationKillerService, CalculationKillerService>();
            services.AddSingleton<ICalculationSurvivorService, CalculationSurvivorService>();

            #endregion

            services.ConfigureInfrastructureServices();

            return services;
        }
    }
}