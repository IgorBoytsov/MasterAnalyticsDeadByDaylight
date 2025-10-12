using DBDAnalytics.UserService.Client.Models.Requests;
using DBDAnalytics.UserService.Client.Models.Responses;
using Shared.Kernel.Results;
using System.Net.Http.Json;
using System.Text.Json;

namespace DBDAnalytics.UserService.Client.ApiClients
{
    public sealed class UserApiService(HttpClient client) : IUserService
    {
        private readonly HttpClient _httpClient = client;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
            {
                PropertyNameCaseInsensitive = true,
            };

        public async Task<Result<UserResponse?>> Login(LoginUserRequest request)
        {
            HttpResponseMessage response = null!;
            try
            {
                response = await _httpClient.PostAsJsonAsync($"api/users/login", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                return Result<UserResponse?>.Success(await response.Content.ReadFromJsonAsync<UserResponse>(_jsonSerializerOptions));
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                    return Result<UserResponse?>.Failure(new Error(ErrorCode.Create, $"Сетевая ошибка при добавлении элемента в api/users/login: {ex.Message}"));

                var errorBody = await response.Content.ReadAsStringAsync();
                return Result<UserResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка добавления элемента в api/users/login: {response.StatusCode} - {errorBody}"));
            }
            catch (JsonException jsonEx)
            {
                return Result<UserResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка десериализации ответа от api/users/login: {jsonEx.Message}"));
            }
        }
    }
}