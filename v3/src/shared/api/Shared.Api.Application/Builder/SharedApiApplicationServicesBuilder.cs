using Microsoft.Extensions.DependencyInjection;

namespace Shared.Api.Application.Builder
{
    internal class SharedApiApplicationServicesBuilder(IServiceCollection services) : ISharedApiApplicationServicesBuilder
    {
        public IServiceCollection Services { get; } = services;
    }
}