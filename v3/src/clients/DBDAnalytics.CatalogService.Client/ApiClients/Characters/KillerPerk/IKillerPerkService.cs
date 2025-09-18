using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerk
{
    public interface IKillerPerkService :
        IKillerPerkReadOnlyService,
        IDeleteApiService<string>
    {
        Task<Result<string>> UpdateAsync(ClientUpdateKillerPerkRequest request);
    }
}