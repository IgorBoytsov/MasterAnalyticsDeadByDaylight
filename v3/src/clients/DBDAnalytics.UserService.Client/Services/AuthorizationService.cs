using DBDAnalytics.UserService.Client.ApiClients;
using DBDAnalytics.UserService.Client.Models.Requests;
using DBDAnalytics.UserService.Client.Models.Responses;
using Shared.Kernel.Results;

namespace DBDAnalytics.UserService.Client.Services
{
    public class AuthorizationService(IUserService userService) : IAuthorizationService
    {
        private readonly IUserService _userService = userService;

        public UserResponse? CurrentUser { get; private set; }

        public async Task<Result<UserResponse?>> AuthenticationAsync(string login, string password, string email)
        {
            var loginUserRequest = new LoginUserRequest(password!, login!, email);

            var result = await _userService.Login(loginUserRequest);

            if (result.IsSuccess)
                CurrentUser = result.Value!;

            return result;
        }
    }
}