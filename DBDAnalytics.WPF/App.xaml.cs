﻿using DBDAnalytics.Application.Ico;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Factories.WindowFactories;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.Services;
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
        
        }

        // Добавляем сервисы WPF коллекцию сервисов 
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient(typeof(Lazy<>), typeof(LazyService<>));

            services.AddSingleton<IWindowNavigationService, WindowNavigationService>();
        }
    }
}