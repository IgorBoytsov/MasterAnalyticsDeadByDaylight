using DBDAnalytics.UserService.Client.Models.Responses;
using Shared.Kernel.Results;

namespace DBDAnalytics.UserService.Client.ApiClients
{
    public interface IUserManagementSerivce
    {
        Task<Result<UserResponse?>> Me(string accesToken);
    }
}