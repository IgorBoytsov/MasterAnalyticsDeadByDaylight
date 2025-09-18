using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerk
{
    public interface ISurvivorPerkService :
        ISurvivorPerkReadOnlyService,
        IDeleteApiService<string>
    {
        Task<Result<string>> UpdateAsync(ClientUpdateSurvivorPerkRequest request);
    }
}