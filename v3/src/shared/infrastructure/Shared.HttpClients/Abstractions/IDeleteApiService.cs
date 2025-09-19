using Shared.Kernel.Results;

namespace Shared.HttpClients.Abstractions
{
    public interface IDeleteApiService<in TKey>
    {
        Task<Result> DeleteAsync(TKey id);
    }
}