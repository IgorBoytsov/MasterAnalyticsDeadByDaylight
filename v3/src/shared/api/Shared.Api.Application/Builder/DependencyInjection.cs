using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Api.Application.Behaviors;
using Shared.Api.Application.Services;

namespace Shared.Api.Application.Builder
{
    public static class DependencyInjection
    {
        public static ISharedApiApplicationServicesBuilder AddSharedApiApplicationServices(this IServiceCollection services)
            => new SharedApiApplicationServicesBuilder(services);

        public static ISharedApiApplicationServicesBuilder WithValidation(this ISharedApiApplicationServicesBuilder builder)
        {
            builder.Services.AddScoped<IValidationService, ValidationService>();

            return builder;
        }

        public static ISharedApiApplicationServicesBuilder WithValidationBehavior(this ISharedApiApplicationServicesBuilder builder)
        {
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return builder;
        }
    }
}