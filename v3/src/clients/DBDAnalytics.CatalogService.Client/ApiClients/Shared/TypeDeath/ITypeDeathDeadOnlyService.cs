using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.TypeDeath
{
    public interface ITypeDeathDeadOnlyService : IBaseReadApiService<TypeDeathResponse, int>
    {
    }
}