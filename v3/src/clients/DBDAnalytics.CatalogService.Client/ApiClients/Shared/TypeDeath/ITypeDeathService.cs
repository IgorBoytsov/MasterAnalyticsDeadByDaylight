using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.TypeDeath
{
    public interface ITypeDeathService :
        ITypeDeathDeadOnlyService,
        IBaseWriteApiService<TypeDeathResponse, int>
    {
    }
}