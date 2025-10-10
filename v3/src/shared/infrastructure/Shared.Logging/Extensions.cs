using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Shared.Logging
{
    public static class Extensions
    {
        public static IHostBuilder UseSerialogLogger(this IHostBuilder builder)
        {
            builder.UseSerilog((context, services, conf) =>
            {
                conf.MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .MinimumLevel.Override("System", LogEventLevel.Warning);

                conf.Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .Enrich.WithProperty("ApplicationName", context.HostingEnvironment.ApplicationName)
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);

                conf.ReadFrom.Configuration(context.Configuration);
            });

            return builder;
        }
    }
}