namespace DBDAnalytics.UserService.Client.Models.Requests
{
    public sealed record RecoveryAccessRequest(string Login, string Email, string NewPassword);
}