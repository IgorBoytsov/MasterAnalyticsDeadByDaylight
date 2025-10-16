using DBDAnalytics.Shared.Contracts.Responses.Maps;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map
{
    public interface IMapReadOnlyService : IGetAllApiService<MapResponse>
    {
    }
}