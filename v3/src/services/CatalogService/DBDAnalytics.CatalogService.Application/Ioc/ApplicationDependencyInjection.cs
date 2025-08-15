using DBDAnalytics.CatalogService.Application.Features.Killers.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Api.Application.Builder;

namespace DBDAnalytics.CatalogService.Application.Ioc
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(CreateKillerCommandValidator).Assembly);

            services.AddSharedApiApplicationServices()
                .WithValidation()
                .WithValidationBehavior();

            return services;
        }
    }
}