using DBDAnalytics.Application.Ico;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Factories.PageFactories;
using DBDAnalytics.WPF.Factories.WindowFactories;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.Services;
using DBDAnalytics.WPF.ViewModels.PageVM;
using DBDAnalytics.WPF.ViewModels.WindowVM;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DBDAnalytics.WPF
{
    public partial class App : System.Windows.Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.ConfigureApplicationServices();

            ConfigureServices(services);
            ConfigureWindow(services);
            ConfigurePage(services);

            ServiceProvider = services.BuildServiceProvider();

            ServiceProvider.GetService<IWindowNavigationService>().OpenWindow(WindowName.MainWindow);

            base.OnStartup(e);
        }

        // Добавление Windows и их VM в коллекцию сервисов 
        private static void ConfigureWindow(ServiceCollection services)
        {
            services.AddTransient<IWindowFactory, MainWindowFactory>();
            services.AddTransient<MainWindowVM>();
            services.AddSingleton<Func<MainWindowVM>>(provider => () => provider.GetRequiredService<MainWindowVM>());            
            
            services.AddTransient<IWindowFactory, PreviewMatchFactory>();
            services.AddTransient<PreviewMatchVM>();
            services.AddSingleton<Func<PreviewMatchVM>>(provider => () => provider.GetRequiredService<PreviewMatchVM>());

            services.AddTransient<IWindowFactory, InteractionAssociationFactory>();
            services.AddTransient<InteractionAssociationVM>();
            services.AddSingleton<Func<InteractionAssociationVM>>(provider => () => provider.GetRequiredService<InteractionAssociationVM>());
            
            services.AddTransient<IWindowFactory, AddMatchFactory>();
            services.AddTransient<AddMatchVM>();
            services.AddSingleton<Func<AddMatchVM>>(provider => () => provider.GetRequiredService<AddMatchVM>());
            
            services.AddTransient<IWindowFactory, InteractionGameEventFactory>();
            services.AddTransient<InteractionGameEventVM>();
            services.AddSingleton<Func<InteractionGameEventVM>>(provider => () => provider.GetRequiredService<InteractionGameEventVM>());
            
            services.AddTransient<IWindowFactory, InteractionGameModeFactory>();
            services.AddTransient<InteractionGameModeVM>();
            services.AddSingleton<Func<InteractionGameModeVM>>(provider => () => provider.GetRequiredService<InteractionGameModeVM>());  
            
            services.AddTransient<IWindowFactory, InteractionItemFactory>();
            services.AddTransient<InteractionItemVM>();
            services.AddSingleton<Func<InteractionItemVM>>(provider => () => provider.GetRequiredService<InteractionItemVM>()); 

            services.AddTransient<IWindowFactory, InteractionKillerFactory>();
            services.AddTransient<InteractionKillerVM>();
            services.AddSingleton<Func<InteractionKillerVM>>(provider => () => provider.GetRequiredService<InteractionKillerVM>()); 
            
            services.AddTransient<IWindowFactory, InteractionKillerPerkCategoryFactory>();
            services.AddTransient<InteractionKillerPerkCategoryVM>();
            services.AddSingleton<Func<InteractionKillerPerkCategoryVM>>(provider => () => provider.GetRequiredService<InteractionKillerPerkCategoryVM>());  
            
            services.AddTransient<IWindowFactory, InteractionMeasurementFactory>();
            services.AddTransient<InteractionMeasurementVM>();
            services.AddSingleton<Func<InteractionMeasurementVM>>(provider => () => provider.GetRequiredService<InteractionMeasurementVM>()); 
            
            services.AddTransient<IWindowFactory, InteractionOfferingCategoryFactory>();
            services.AddTransient<InteractionOfferingCategoryVM>();
            services.AddSingleton<Func<InteractionOfferingCategoryVM>>(provider => () => provider.GetRequiredService<InteractionOfferingCategoryVM>());  

             services.AddTransient<IWindowFactory, InteractionOfferingFactory>();
            services.AddTransient<InteractionOfferingVM>();
            services.AddSingleton<Func<InteractionOfferingVM>>(provider => () => provider.GetRequiredService<InteractionOfferingVM>());  

            services.AddTransient<IWindowFactory, InteractionPatchFactory>();
            services.AddTransient<InteractionPatchVM>();
            services.AddSingleton<Func<InteractionPatchVM>>(provider => () => provider.GetRequiredService<InteractionPatchVM>());
            
            services.AddTransient<IWindowFactory, InteractionPlatformFactory>();
            services.AddTransient<InteractionPlatformVM>();
            services.AddSingleton<Func<InteractionPlatformVM>>(provider => () => provider.GetRequiredService<InteractionPlatformVM>());

            services.AddTransient<IWindowFactory, InteractionRarityFactory>();
            services.AddTransient<InteractionRarityVM>();
            services.AddSingleton<Func<InteractionRarityVM>>(provider => () => provider.GetRequiredService<InteractionRarityVM>());

            services.AddTransient<IWindowFactory, InteractionRoleFactory>();
            services.AddTransient<InteractionRoleVM>();
            services.AddSingleton<Func<InteractionRoleVM>>(provider => () => provider.GetRequiredService<InteractionRoleVM>());      
            
            services.AddTransient<IWindowFactory, InteractionSurvivorPerkCategoryFactory>();
            services.AddTransient<InteractionSurvivorPerkCategoryVM>();
            services.AddSingleton<Func<InteractionSurvivorPerkCategoryVM>>(provider => () => provider.GetRequiredService<InteractionSurvivorPerkCategoryVM>());

            services.AddTransient<IWindowFactory, InteractionSurvivorFactory>();
            services.AddTransient<InteractionSurvivorVM>();
            services.AddSingleton<Func<InteractionSurvivorVM>>(provider => () => provider.GetRequiredService<InteractionSurvivorVM>());

            services.AddTransient<IWindowFactory, InteractionTypeDeathFactory>();
            services.AddTransient<InteractionTypeDeathVM>();
            services.AddSingleton<Func<InteractionTypeDeathVM>>(provider => () => provider.GetRequiredService<InteractionTypeDeathVM>());
            
            services.AddTransient<IWindowFactory, InteractionWhoPlacedMapFactory>();
            services.AddTransient<InteractionWhoPlacedMapVM>();
            services.AddSingleton<Func<InteractionWhoPlacedMapVM>>(provider => () => provider.GetRequiredService<InteractionWhoPlacedMapVM>());
        }

        // Добавление Pages и их VM в коллекцию сервисов 
        private static void ConfigurePage(ServiceCollection services)
        {
            services.AddTransient<IPageFactory, DashBoardFactory>();
            services.AddTransient<DashBoardVM>();
            services.AddSingleton<Func<DashBoardVM>>(provider => () => provider.GetRequiredService<DashBoardVM>());

            services.AddTransient<IPageFactory, KillerDetailsFactory>();
            services.AddTransient<KillerDetailsVM>();
            services.AddSingleton<Func<KillerDetailsVM>>(provider => () => provider.GetRequiredService<KillerDetailsVM>());

            services.AddTransient<IPageFactory, MapDetailsFactory>();
            services.AddTransient<MapDetailsVM>();
            services.AddSingleton<Func<MapDetailsVM>>(provider => () => provider.GetRequiredService<MapDetailsVM>());

            services.AddTransient<IPageFactory, SurvivorDetailsFactory>();
            services.AddTransient<SurvivorDetailsVM>();
            services.AddSingleton<Func<SurvivorDetailsVM>>(provider => () => provider.GetRequiredService<SurvivorDetailsVM>());

            services.AddTransient<IPageFactory, ItemDetailsFactory>();
            services.AddTransient<ItemDetailsVM>();
            services.AddSingleton<Func<ItemDetailsVM>>(provider => () => provider.GetRequiredService<ItemDetailsVM>());

            services.AddTransient<IPageFactory, OfferingDetailsFactory>();
            services.AddTransient<OfferingDetailsVM>();
            services.AddSingleton<Func<OfferingDetailsVM>>(provider => () => provider.GetRequiredService<OfferingDetailsVM>());

            services.AddTransient<IPageFactory, PerkDetailsFactory>();
            services.AddTransient<PerkDetailsVM>();
            services.AddSingleton<Func<PerkDetailsVM>>(provider => () => provider.GetRequiredService<PerkDetailsVM>());
        }

        // Добавляем сервисы WPF коллекцию сервисов 
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient(typeof(Lazy<>), typeof(LazyService<>));

            services.AddSingleton<IWindowNavigationService, WindowNavigationService>();
            services.AddSingleton<IPageNavigationService, PageNavigationService>();
        }
    }
}