using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.HttpClients.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.Killer
{
    public interface IKillerService : 
        IKillerReadOnlyService,
        IDeleteApiService<string>
    {
        Task<Result<KillerResponse>> AddAsync(ClientAddKillerRequest request);
        Task<Result<KillersImageKeysResponse>> UpdateAsync(ClientUpdateKillerRequest request);
    }
}