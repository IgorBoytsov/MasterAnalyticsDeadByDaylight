using DBDAnalytics.Shared.Domain.Results;

namespace Shared.HttpClients.Abstractions
{
    public interface IDeleteApiService<in TKey>
    {
        Task<Result> DeleteAsync(TKey id);
    }
}