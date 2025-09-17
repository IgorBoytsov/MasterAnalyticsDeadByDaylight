namespace Shared.HttpClients.Abstractions
{
    public interface IGetByIdApiService<TResponse, in TKey> where TResponse : class
    {
        Task<TResponse?> GetByIdAsync(TKey id);
    }
}