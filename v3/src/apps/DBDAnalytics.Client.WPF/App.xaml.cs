using DBDAnalytics.Client.WPF.Ioc;
using DBDAnalytics.FIleStorageService.Client;
using DBDAnalytics.UserService.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Enums;
using Shared.WPF.Navigations.Pages;
using Shared.WPF.Navigations.Windows;
using Shared.WPF.Services;
using System.Windows;

namespace DBDAnalytics.Client.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var configurationBuilder = new ConfigurationBuilder()
                     .SetBasePath(AppContext.BaseDirectory)
                     .AddJsonFile("apiconfig.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();

            var services = new ServiceCollection();

            services.AddHttpService(configuration);
            services.ConfigureFileStorageApiHttpService(configuration);
            services.ConfigurePage();
            services.ConfigureWindows();

            services.AddSingleton<IFileDialogService, FileDialogService>();

            services.AddSingleton<IWindowNavigation, WindowNavigation>();
            services.AddSingleton<IPageNavigation, PageNavigation>();
            services.AddSingleton<IAuthorizationService, AuthorizationService>();

            ServiceProvider = services.BuildServiceProvider();

            ServiceProvider.GetService<IWindowNavigation>()!.Open(WindowsName.MainWindow);
            //ServiceProvider.GetService<IPageNavigation>()!.Navigate(PagesName.AddMatch, FramesName.MainFrame);

            base.OnStartup(e);
        }
    }

}