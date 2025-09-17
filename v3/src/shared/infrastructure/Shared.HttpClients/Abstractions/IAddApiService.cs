using DBDAnalytics.Shared.Domain.Results;

namespace Shared.HttpClients.Abstractions
{
    public interface IAddApiService<TResponse> where TResponse : class
    {
        Task<Result<TResponse?>> AddAsync<TRequest>(TRequest newItem);
    }
}