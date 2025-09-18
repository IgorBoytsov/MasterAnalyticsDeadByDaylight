using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Platform
{
    public interface IPlatformReadOnlyService : IBaseReadApiService<PlatformResponse, int>
    {
    }
}