using DBDAnalytics.UserService.Client.Models.Requests;
using DBDAnalytics.UserService.Client.Models.Responses;
using Shared.Kernel.Results;

namespace DBDAnalytics.UserService.Client.ApiClients
{
    public interface IUserService
    {
        Task<Result<UserResponse?>> Login(LoginUserRequest request);
    }
}