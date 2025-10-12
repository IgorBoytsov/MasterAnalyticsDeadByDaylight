using DBDAnalytics.UserService.Client.Models.Responses;
using Shared.Kernel.Results;

namespace DBDAnalytics.UserService.Client.Services
{
    public interface IAuthorizationService
    {
        UserResponse? CurrentUser { get; }

        Task<Result<UserResponse?>> AuthenticationAsync(string login, string password, string email);
    }
}