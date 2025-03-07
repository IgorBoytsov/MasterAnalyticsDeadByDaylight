using Microsoft.Extensions.DependencyInjection;

namespace DBDAnalytics.WPF.Services
{
    internal class LazyService<T>(IServiceProvider serviceProvider) 
        : Lazy<T>(() => serviceProvider.GetRequiredService<T>()) where T : class
    {
    }
}
