using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients.Abstractions;
using System.Net.Http.Json;
using System.Text.Json;

//TODO: Добавить логирование ошибок
namespace Shared.HttpClients
{
    public abstract class BaseApiService<TResponse, TKey> : IBaseApiService<TResponse, TKey>
        where TResponse : class
    {
        public readonly HttpClient _httpClient;
        public readonly string _endpoint;
        protected readonly JsonSerializerOptions _jsonSerializerOptions;

        protected BaseApiService(HttpClient httpClient, string endpoint)
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

        public virtual async Task<Result<TResponse?>> AddAsync<TRequest>(TRequest newItem)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_endpoint, newItem, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                return Result<TResponse?>.Success(await response.Content.ReadFromJsonAsync<TResponse>(_jsonSerializerOptions));
            }
            catch (HttpRequestException)
            {
                var response = await _httpClient.PostAsJsonAsync(_endpoint, newItem, _jsonSerializerOptions);
                var errorBody = await response.Content.ReadAsStringAsync();

                return Result<TResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка добавление элемента в {_endpoint}: {response.StatusCode} - {errorBody}"));
            }
        }

        public virtual async Task<Result> UpdateAsync<TRequest>(TKey id, TRequest updatedItem)
        {
            try
            {
                var response = await _httpClient.PatchAsJsonAsync($"{_endpoint}/{id}", updatedItem, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                    return Result.Success();

                return Result.Failure(new Error(ErrorCode.Update, await response.Content.ReadAsStringAsync()));
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка обновление элемента с ID {id} в {_endpoint}: {ex.Message}");
                return Result.Failure(new Error(ErrorCode.Update, "Ошибка API"));
            }
        }

        public virtual async Task<Result> DeleteAsync(TKey id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_endpoint}/{id}");

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(ErrorCode.Delete, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка удаления элемента с ID {id} из {_endpoint}: {ex.Message}");
                return Result.Failure(new Error(ErrorCode.Update, "Ошибка API"));
            }
        }
    }
}