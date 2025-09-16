using Microsoft.Extensions.DependencyInjection;

namespace Shared.Api.Application.Builder
{
    public interface ISharedApiApplicationServicesBuilder
    {
        IServiceCollection Services { get; }
    }
}