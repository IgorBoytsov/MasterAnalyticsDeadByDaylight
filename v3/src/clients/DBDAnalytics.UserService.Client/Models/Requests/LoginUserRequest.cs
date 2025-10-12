namespace DBDAnalytics.UserService.Client.Models.Requests
{
    public sealed record LoginUserRequest(string Password, string Login, string Email);
}