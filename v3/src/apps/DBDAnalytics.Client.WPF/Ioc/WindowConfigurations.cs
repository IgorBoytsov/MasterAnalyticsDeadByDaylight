using DBDAnalytics.Client.WPF.Factories.Windows;
using DBDAnalytics.Client.WPF.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Navigations.Windows;

namespace DBDAnalytics.Client.WPF.Ioc
{
    internal static class WindowConfigurations
    {
        public static void ConfigureWindows(this ServiceCollection service)
        {
            service.AddTransient<IWindowFactory, MainWindowFactory>();
            service.AddTransient<MainWindowViewModel>();
            service.AddSingleton<Func<MainWindowViewModel>>(provider => () => provider.GetRequiredService<MainWindowViewModel>());
        }
    }
}