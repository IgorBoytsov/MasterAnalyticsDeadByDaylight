using DBDAnalytics.UserService.Client.ApiClients;
using DBDAnalytics.UserService.Client.Models.Requests;
using DBDAnalytics.UserService.Client.Models.Responses;
using Shared.Kernel.Results;
using Shared.WPF.Enums;

namespace Shared.WPF.Services.Initializers
{
    public class AuthorizationService(IAuthService userService, IUserManagementSerivce userManagementSerivce, ITokenService tokenService) : IAuthorizationService
    {
        private readonly IAuthService _authService = userService;
        private readonly IUserManagementSerivce _userManagementSerivce = userManagementSerivce;
        private readonly ITokenService _tokenService = tokenService;

        public UserResponse? CurrentUser { get; private set; }
        public AuthenticationResponse? AuthData { get; private set; }

        public async Task<Result<UserResponse?>> AuthenticationAsync(string login, string password, AppType appType)
        {
            var result = await _authService.Login(new LoginUserRequest(password!, login!));

            if (result.IsSuccess)
            {
                AuthData = result.Value!;

                _tokenService.SaveToken(AuthData.RefreshToken, appType);

                var resultUserData = await _userManagementSerivce.Me(AuthData.AccessToken);

                if (resultUserData.IsSuccess)
                    CurrentUser = resultUserData.Value;

                return resultUserData;
            }

            return Result<UserResponse?>.Failure(result.Errors);
        }

        public async Task<Result> AuthenticationByTokenAsync(AppType appType)
        {
            var refrash = _tokenService.GetToken(appType);

            if (refrash is null)
                return Result.Failure(new Error(ErrorCode.NotFound, "Не найден токен"));

            var result = await _authService.LoginByTokenAsync(new LoginByTokenRequest(refrash));

            if (result.IsSuccess)
            {
                AuthData = result.Value!;

                _tokenService.SaveToken(AuthData.RefreshToken, appType);

                var resultUserData = await _userManagementSerivce.Me(AuthData.AccessToken);

                if (resultUserData.IsSuccess)
                    CurrentUser = resultUserData.Value;

                return resultUserData;
            }

            return Result.Failure(result.Errors);
        }
    }
}