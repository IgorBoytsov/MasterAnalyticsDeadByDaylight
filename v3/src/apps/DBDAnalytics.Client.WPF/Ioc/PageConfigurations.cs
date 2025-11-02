using DBDAnalytics.Client.WPF.Factories.Pages;
using DBDAnalytics.Client.WPF.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Navigations.Pages;

namespace DBDAnalytics.Client.WPF.Ioc
{
    internal static class PageConfigurations
    {
        public static void ConfigurePage(this ServiceCollection service)
        {
            service.AddTransient<IPageFactory, AddMatchPageFactory>();
            service.AddTransient<AddMatchPageViewModel>();
            service.AddSingleton<Func<AddMatchPageViewModel>>(provider => () => provider.GetRequiredService<AddMatchPageViewModel>());
        }
    }
}