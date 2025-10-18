using Shared.HttpClients.Abstractions;
using Shared.Kernel.Results;
using System.Net.Http.Json;

//TODO: Добавить логирование ошибок.
namespace Shared.HttpClients
{
    public abstract class BaseApiService<TResponse, TKey>(HttpClient httpClient, string endpoint) :
        BaseReadApiService<TResponse, TKey>(httpClient, endpoint),
        IBaseApiService<TResponse, TKey>
        where TResponse : class
    {

        public virtual async Task<Result<TResponse?>> AddAsync<TRequest>(TRequest newItem)
        {
            HttpResponseMessage response = null!;

            try
            {
                response = await _httpClient.PostAsJsonAsync(_endpoint, newItem, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                if (response.Content.Headers.ContentLength == 0)
                    return Result<TResponse?>.Success(null);

                return Result<TResponse?>.Success(await response.Content.ReadFromJsonAsync<TResponse>(_jsonSerializerOptions));
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                    return Result<TResponse?>.Failure(new Error(ErrorCode.Create, $"Сетевая ошибка при добавлении элемента в {_endpoint}: {ex.Message}"));

                var errorBody = await response.Content.ReadAsStringAsync();
                return Result<TResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка добавления элемента в {_endpoint}: {response.StatusCode} - {errorBody}"));
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                return Result<TResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка десериализации ответа от {_endpoint}: {jsonEx.Message}"));
            }
        }

        public virtual async Task<Result> CreateAsync<TRequest>(TRequest newItem)
        {
            HttpResponseMessage response = null!;

            try
            {
                response = await _httpClient.PostAsJsonAsync(_endpoint, newItem, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                    return Result.Success();

                var errorBody = await response.Content.ReadAsStringAsync();
                return Result.Failure(new Error(ErrorCode.Create, $"Ошибка добавление элемента в {_endpoint}: {response.StatusCode} - {errorBody}"));
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                    return Result.Failure(new Error(ErrorCode.Create, $"Сетевая ошибка при добавление элемента в {_endpoint}: {ex.Message}"));

                var errorBody = await response.Content.ReadAsStringAsync();
                return Result.Failure(new Error(ErrorCode.Create, $"Ошибка добавления элемента в {_endpoint}: {response.StatusCode} - {errorBody}"));
            }
        }

        public virtual async Task<Result> UpdateAsync<TRequest>(TKey id, TRequest updatedItem)
        {
            HttpResponseMessage response = null!;

            try
            {
                var endpointUrl = $"{_endpoint}/{id}";
                response = await _httpClient.PatchAsJsonAsync(endpointUrl, updatedItem, _jsonSerializerOptions);

                if (response.IsSuccessStatusCode)
                    return Result.Success();

                var errorBody = await response.Content.ReadAsStringAsync();
                return Result.Failure(new Error(ErrorCode.Update, $"Ошибка обновления элемента с ID {id} в {endpointUrl}: {response.StatusCode} - {errorBody}"));
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                    return Result.Failure(new Error(ErrorCode.Update, $"Сетевая ошибка при обновлении элемента с ID {id} в {_endpoint}: {ex.Message}"));

                var errorBody = await response.Content.ReadAsStringAsync();
                return Result.Failure(new Error(ErrorCode.Update, $"Ошибка обновления элемента с ID {id} в {_endpoint}: {response.StatusCode} - {errorBody}"));
            }
        }

        public virtual async Task<Result> DeleteAsync(TKey id)
        {
            HttpResponseMessage response = null!;

            try
            {
                var endpointUrl = $"{_endpoint}/{id}";
                response = await _httpClient.DeleteAsync(endpointUrl);

                if (response.IsSuccessStatusCode)
                    return Result.Success();

                var errorBody = await response.Content.ReadAsStringAsync();
                return Result.Failure(new Error(ErrorCode.Delete, $"Ошибка удаления элемента с ID {id} из {endpointUrl}: {response.StatusCode} - {errorBody}"));
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                    return Result.Failure(new Error(ErrorCode.Delete, $"Сетевая ошибка при удалении элемента с ID {id} из {_endpoint}: {ex.Message}"));

                var errorBody = await response.Content.ReadAsStringAsync();
                return Result.Failure(new Error(ErrorCode.Delete, $"Ошибка удаления элемента с ID {id} из {_endpoint}: {response.StatusCode} - {errorBody}"));
            }
        }
    }
}