namespace Shared.HttpClients.Abstractions
{
    public interface IBaseApiService<TResponse, TKey> :
        IBaseReadApiService<TResponse, TKey>, 
        IBaseWriteApiService<TResponse, TKey>
        where TResponse : class
    {
    }
}