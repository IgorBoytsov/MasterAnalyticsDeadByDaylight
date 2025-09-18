using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Associations
{
    public interface IPlayerAssociationReadOnlyService : IBaseReadApiService<PlayerAssociationResponse, int>
    {
    }
}