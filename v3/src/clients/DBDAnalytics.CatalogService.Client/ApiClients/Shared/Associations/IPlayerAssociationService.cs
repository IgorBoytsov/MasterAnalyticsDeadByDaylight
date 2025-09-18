using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Associations
{
    public interface IPlayerAssociationService :
        IPlayerAssociationReadOnlyService, IBaseWriteApiService<PlayerAssociationResponse, int>
    {
    }
}