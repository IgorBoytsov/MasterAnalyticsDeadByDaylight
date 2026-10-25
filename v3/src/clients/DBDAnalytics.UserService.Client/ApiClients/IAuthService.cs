using DBDAnalytics.UserService.Client.Models.Requests;
using DBDAnalytics.UserService.Client.Models.Responses;
using Shared.Kernel.Results;

namespace DBDAnalytics.UserService.Client.ApiClients
{
    public interface IAuthService
    {
        Task<Result<AuthenticationResponse?>> Login(LoginUserRequest request);
        Task<Result<AuthenticationResponse>> LoginByTokenAsync(LoginByTokenRequest request, CancellationToken cancellationToken = default);
    }
}