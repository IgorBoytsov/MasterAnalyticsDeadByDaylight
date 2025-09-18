using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients.Abstractions;

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