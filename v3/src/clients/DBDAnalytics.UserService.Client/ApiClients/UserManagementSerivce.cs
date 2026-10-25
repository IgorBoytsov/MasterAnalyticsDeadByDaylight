using DBDAnalytics.UserService.Client.Models.Responses;
using Shared.Kernel.Results;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace DBDAnalytics.UserService.Client.ApiClients
{
    public sealed class UserManagementSerivce(HttpClient client) : IUserManagementSerivce
    {
        private readonly HttpClient _client = client;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<Result<UserResponse?>> Me(string accesToken)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesToken);

                var responseMessage = await _client.GetAsync("api/users/me");

                responseMessage.EnsureSuccessStatusCode();
                var userResponse = await responseMessage.Content.ReadFromJsonAsync<UserResponse>(_jsonSerializerOptions);

                return Result<UserResponse?>.Success(userResponse);
            }
            catch (HttpRequestException ex)
            {
                return Result<UserResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка: {ex}"));
            }
            catch (JsonException jsonEx)
            {
                return Result<UserResponse?>.Failure(new Error(ErrorCode.Create, $"Ошибка десериализации ответа от api/users/login: {jsonEx.Message}"));
            }
        }
    }
}