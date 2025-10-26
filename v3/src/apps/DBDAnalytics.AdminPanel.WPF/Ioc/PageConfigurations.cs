using DBDAnalytics.AdminPanel.WPF.Factories.Pages;
using DBDAnalytics.AdminPanel.WPF.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Navigations.Pages;

namespace DBDAnalytics.AdminPanel.WPF.Ioc
{
    internal static class PageConfigurations
    {
        public static void ConfigurePage(this ServiceCollection service)
        {
            service.AddTransient<IPageFactory, KillerPerkCategoryPageFactory>();
            service.AddTransient<KillerPerkCategoryPageViewModel>();
            service.AddSingleton<Func<KillerPerkCategoryPageViewModel>>(provider => () => provider.GetRequiredService<KillerPerkCategoryPageViewModel>());

            service.AddTransient<IPageFactory, SurvivorPerkCategoryPageFactory>();
            service.AddTransient<SurvivorPerkCategoryPageViewModel>();
            service.AddSingleton<Func<SurvivorPerkCategoryPageViewModel>>(provider => () => provider.GetRequiredService<SurvivorPerkCategoryPageViewModel>());

            service.AddTransient<IPageFactory, OfferingCategoryPageFactory>();
            service.AddTransient<OfferingCategoryPageViewModel>();
            service.AddSingleton<Func<OfferingCategoryPageViewModel>>(provider => () => provider.GetRequiredService<OfferingCategoryPageViewModel>());

            service.AddTransient<IPageFactory, GameEventPageFactory>();
            service.AddTransient<GameEventPageViewModel>();
            service.AddSingleton<Func<GameEventPageViewModel>>(provider => () => provider.GetRequiredService<GameEventPageViewModel>());

            service.AddTransient<IPageFactory, GameModePageFactory>();
            service.AddTransient<GameModePageViewModel>();
            service.AddSingleton<Func<GameModePageViewModel>>(provider => () => provider.GetRequiredService<GameModePageViewModel>());

            service.AddTransient<IPageFactory, ItemPageFactory>();
            service.AddTransient<ItemPageViewModel>();
            service.AddSingleton<Func<ItemPageViewModel>>(provider => () => provider.GetRequiredService<ItemPageViewModel>());

            service.AddTransient<IPageFactory, KillerPageFactory>();
            service.AddTransient<KillerPageViewModel>();
            service.AddSingleton<Func<KillerPageViewModel>>(provider => () => provider.GetRequiredService<KillerPageViewModel>());

            service.AddTransient<IPageFactory, MeasurementPageFactory>();
            service.AddTransient<MeasurementPageViewModel>();
            service.AddSingleton<Func<MeasurementPageViewModel>>(provider => () => provider.GetRequiredService<MeasurementPageViewModel>());

            service.AddTransient<IPageFactory, OfferingPageFactory>();
            service.AddTransient<OfferingPageViewModel>();
            service.AddSingleton<Func<OfferingPageViewModel>>(provider => () => provider.GetRequiredService<OfferingPageViewModel>());

            service.AddTransient<IPageFactory, PlatformPageFactory>();
            service.AddTransient<PlatformPageViewModel>();
            service.AddSingleton<Func<PlatformPageViewModel>>(provider => () => provider.GetRequiredService<PlatformPageViewModel>());
           
            service.AddTransient<IPageFactory, PlayerAssociationPageFactory>();
            service.AddTransient<PlayerAssociationPageViewModel>();
            service.AddSingleton<Func<PlayerAssociationPageViewModel>>(provider => () => provider.GetRequiredService<PlayerAssociationPageViewModel>());

            service.AddTransient<IPageFactory, PatchPageFactory>();
            service.AddTransient<PatchPageViewModel>();
            service.AddSingleton<Func<PatchPageViewModel>>(provider => () => provider.GetRequiredService<PatchPageViewModel>());
           
            service.AddTransient<IPageFactory, RarityPageFactory>();
            service.AddTransient<RarityPageViewModel>();
            service.AddSingleton<Func<RarityPageViewModel>>(provider => () => provider.GetRequiredService<RarityPageViewModel>());
           
            service.AddTransient<IPageFactory, RolePageFactory>();
            service.AddTransient<RolePageViewModel>();
            service.AddSingleton<Func<RolePageViewModel>>(provider => () => provider.GetRequiredService<RolePageViewModel>());
           
            service.AddTransient<IPageFactory, SurvivorPageFactory>();
            service.AddTransient<SurvivorPageViewModel>();
            service.AddSingleton<Func<SurvivorPageViewModel>>(provider => () => provider.GetRequiredService<SurvivorPageViewModel>());
           
            service.AddTransient<IPageFactory, TypeDeathPageFactory>();
            service.AddTransient<TypeDeathPageViewModel>();
            service.AddSingleton<Func<TypeDeathPageViewModel>>(provider => () => provider.GetRequiredService<TypeDeathPageViewModel>());
           
            service.AddTransient<IPageFactory, WhoPlacedMapPageFactory>();
            service.AddTransient<WhoPlacedMapPageViewModel>();
            service.AddSingleton<Func<WhoPlacedMapPageViewModel>>(provider => () => provider.GetRequiredService<WhoPlacedMapPageViewModel>());
        }
    }
}