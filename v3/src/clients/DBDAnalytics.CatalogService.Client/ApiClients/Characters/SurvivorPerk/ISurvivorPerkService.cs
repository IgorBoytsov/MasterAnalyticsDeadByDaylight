using DBDAnalytics.CatalogService.Client.Models;
using Shared.HttpClients.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerk
{
    public interface ISurvivorPerkService :
        ISurvivorPerkReadOnlyService,
        IDeleteApiService<string>
    {
        Task<Result<string>> UpdateAsync(ClientUpdateSurvivorPerkRequest request);
    }
}