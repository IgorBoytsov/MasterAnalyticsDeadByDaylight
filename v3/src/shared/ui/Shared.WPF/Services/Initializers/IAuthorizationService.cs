using DBDAnalytics.UserService.Client.Models.Responses;
using Shared.Kernel.Results;
using Shared.WPF.Enums;

namespace Shared.WPF.Services.Initializers
{
    public interface IAuthorizationService
    {
        UserResponse? CurrentUser { get; }
        AuthenticationResponse? AuthData { get; }

        Task<Result<UserResponse?>> AuthenticationAsync(string login, string password, AppType appType);
        Task<Result> AuthenticationByTokenAsync(AppType appType);
    }
}