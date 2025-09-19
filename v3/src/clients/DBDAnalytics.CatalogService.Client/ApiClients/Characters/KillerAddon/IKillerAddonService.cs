using DBDAnalytics.CatalogService.Client.Models;
using Shared.HttpClients.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerAddon
{
    public interface IKillerAddonService :
        IKillerAddonReadOnlyService,
        IDeleteApiService<string>
    {
        Task<Result<string>> UpdateAsync(ClientUpdateKillerAddonRequest request);
    }
}