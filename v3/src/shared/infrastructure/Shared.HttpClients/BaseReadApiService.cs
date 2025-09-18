using Shared.HttpClients.Abstractions;
using System.Net.Http.Json;
using System.Text.Json;

namespace Shared.HttpClients
{
    //TODO: Добавить логирование ошибок.
    public abstract class BaseReadApiService<TResponse, TKey> : IBaseReadApiService<TResponse, TKey>
        where TResponse : class
    {
        public readonly HttpClient _httpClient;
        public readonly string _endpoint;
        protected readonly JsonSerializerOptions _jsonSerializerOptions;

        protected BaseReadApiService(HttpClient httpClient, string endpoint)
        {
            _httpClient = httpClient;
            _endpoint = endpoint;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public virtual async Task<List<TResponse>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_endpoint);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<List<TResponse>>(_jsonSerializerOptions);
                return result ?? [];
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при извлечении всех элементов из {_endpoint}: {ex.Message}");
                return [];
            }
        }

        public virtual async Task<TResponse?> GetByIdAsync(TKey id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_endpoint}/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<TResponse>(_jsonSerializerOptions);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при выборке элемента с идентификатором {id} из {_endpoint}: {ex.Message}");
                return null;
            }
        }
    }
}