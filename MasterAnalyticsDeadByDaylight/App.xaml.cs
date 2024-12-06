using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.DialogService;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.WindowNavigation;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<Func<MasterAnalyticsDeadByDaylightDbContext>>(provider => () => provider.GetRequiredService<MasterAnalyticsDeadByDaylightDbContext>());
            services.AddTransient<MasterAnalyticsDeadByDaylightDbContext>();
            services.AddSingleton<IDataService, DataService>();

            services.AddSingleton<ICustomDialogService, CustomDialogService>();

            services.AddSingleton<IPageNavigationService, PageNavigationService>();
            services.AddSingleton<IWindowNavigationService, WindowNavigationService>();
        }
    }

}
