using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Associations
{
    public class AssociationApiService(HttpClient httpClient)
        : BaseApiService<PlayerAssociationResponse, int>(httpClient, "api/player-associations"), IPlayerAssociationService
    {
    }
}