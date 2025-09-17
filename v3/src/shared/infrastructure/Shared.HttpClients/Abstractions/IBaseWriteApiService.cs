namespace Shared.HttpClients.Abstractions
{
    public interface IBaseWriteApiService<TResponse, in TKey> :
        IAddApiService<TResponse>,
        IUpdateApiService<TKey>,
        IDeleteApiService<TKey>
        where TResponse : class
    {
    }
}