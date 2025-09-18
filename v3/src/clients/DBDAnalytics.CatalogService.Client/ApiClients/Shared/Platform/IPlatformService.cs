using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Platform
{
    public interface IPlatformService : 
        IPlatformReadOnlyService, 
        IBaseWriteApiService<PlatformResponse, int>
    {
    }
}