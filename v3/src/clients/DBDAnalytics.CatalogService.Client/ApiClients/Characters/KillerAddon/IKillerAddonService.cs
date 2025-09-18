using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerAddon
{
    public interface IKillerAddonService :
        IKillerAddonReadOnlyService,
        IDeleteApiService<string>
    {
        Task<Result<string>> UpdateAsync(ClientUpdateKillerAddonRequest request);
    }
}