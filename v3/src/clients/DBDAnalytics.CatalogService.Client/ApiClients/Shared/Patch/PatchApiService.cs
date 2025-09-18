using DBDAnalytics.Shared.Contracts.Responses;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Patch
{
    public sealed class PatchApiService(HttpClient client)
        : BaseApiService<PatchResponse, int>(client, "api/patches"), IPatchService
    {
    }
}