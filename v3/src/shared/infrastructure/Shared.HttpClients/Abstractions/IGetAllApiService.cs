namespace Shared.HttpClients.Abstractions
{
    public interface IGetAllApiService<TResponse> where TResponse : class
    {
        Task<List<TResponse>> GetAllAsync();
    }
}