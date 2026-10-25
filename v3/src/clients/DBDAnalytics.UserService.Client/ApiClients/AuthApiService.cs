using DBDAnalytics.UserService.Client.Models.Requests;
using DBDAnalytics.UserService.Client.Models.Responses;
using Shared.Kernel.Results;
using System.Net.Http.Json;
using System.Text.Json;

namespace DBDAnalytics.UserService.Client.ApiClients
{
    public sealed class AuthApiService(HttpClient client) : IAuthService
    {
        private readonly HttpClient _httpClient = client;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
            {
                PropertyNameCaseInsensitive = true,
            };

        public async Task<Result<AuthenticationResponse?>> Login(LoginUserRequest request)
        {
            HttpResponseMessage response = null!;
            try
            {
                response = await _httpClient.PostAsJsonAsync("api/auth/login", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                return Result<AuthenticationResponse?>.Success(await response.Content.ReadFromJsonAsync<AuthenticationResponse>(_jsonSerializerOptions));
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                    return Result<AuthenticationResponse?>.Failure(new Error(ErrorCode.Create, $"Сетевая ошибка при добавлении элемента в api/users/login: {ex.Message}"));

                var errorBody = await response.Content.ReadAsStringAsync();
                return Result<AuthenticationResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка добавления элемента в api/users/login: {response.StatusCode} - {errorBody}"));
            }
            catch (JsonException jsonEx)
            {
                return Result<AuthenticationResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка десериализации ответа от api/users/login: {jsonEx.Message}"));
            }
        }

        public async Task<Result<AuthenticationResponse>> LoginByTokenAsync(LoginByTokenRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/token-login", request, _jsonSerializerOptions, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadFromJsonAsync<AuthenticationResponse>(cancellationToken);

                return Result<AuthenticationResponse>.Success(responseData!);
            }
            catch (HttpRequestException ex)
            {
                return Result<AuthenticationResponse>.Failure(new Error(ErrorCode.ApiError, ex.Message));
            }
            catch (Exception ex)
            {
                return Result<AuthenticationResponse>.Failure(new Error(ErrorCode.ApiError, $"Произошла критическая ошибки при отправки запроса: {ex.Message}"));
            }
        }
    }
}