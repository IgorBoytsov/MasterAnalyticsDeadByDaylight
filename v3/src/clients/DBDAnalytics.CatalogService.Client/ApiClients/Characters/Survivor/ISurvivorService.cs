using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.HttpClients.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.Survivor
{
    public interface ISurvivorService :
        ISurvivorReadOnlyService,
        IDeleteApiService<string>
    {
        Task<Result<SurvivorResponse>> AddAsync(ClientAddSurvivorRequest request);
        Task<Result<string>> UpdateAsync(ClientUpdateSurvivorRequest request);
    }
}