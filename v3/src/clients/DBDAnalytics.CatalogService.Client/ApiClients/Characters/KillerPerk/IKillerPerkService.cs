using DBDAnalytics.CatalogService.Client.Models;
using Shared.HttpClients.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerk
{
    public interface IKillerPerkService :
        IKillerPerkReadOnlyService,
        IDeleteApiService<string>
    {
        Task<Result<string>> UpdateAsync(ClientUpdateKillerPerkRequest request);
    }
}