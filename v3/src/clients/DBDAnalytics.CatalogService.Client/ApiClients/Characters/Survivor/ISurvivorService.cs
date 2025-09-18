using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients.Abstractions;

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