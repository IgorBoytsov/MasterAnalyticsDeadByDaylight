using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DBDAnalytics.FIleStorageService.Client
{
    public static class HttpServiceConfiguration
    {
        public static IServiceCollection ConfigureFileStorageApiHttpService(this IServiceCollection services, IConfiguration configuration)
        {
            string? catalogServiceApiUrl = configuration.GetValue<string>("BaseFileStorageApiServiceUrl");

            if (string.IsNullOrEmpty(catalogServiceApiUrl))
                throw new InvalidOperationException("BaseFileStorageApiServiceUrl не сконфигурирована.");

            const string fileStorageApiClientName = "FileStorageApi";
            services.AddHttpClient(fileStorageApiClientName, client => client.BaseAddress = new Uri(catalogServiceApiUrl));

            services.AddHttpClient<IFileStorageService, FileStorageServiceClient>(fileStorageApiClientName);

            return services;
        }
    }
}