using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.Killer
{
    public interface IKillerReadOnlyService : IBaseReadApiService<KillerSoloResponse, string>
    {
    }
}