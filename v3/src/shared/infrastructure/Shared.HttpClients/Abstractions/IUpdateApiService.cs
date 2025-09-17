using DBDAnalytics.Shared.Domain.Results;

namespace Shared.HttpClients.Abstractions
{
    public interface IUpdateApiService<in TKey>
    {
        Task<Result> UpdateAsync<TRequest>(TKey id, TRequest updatedItem);
    }
}