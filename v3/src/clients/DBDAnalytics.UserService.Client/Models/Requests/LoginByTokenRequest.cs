namespace DBDAnalytics.UserService.Client.Models.Requests
{
    public sealed record LoginByTokenRequest(string RefreshToken);
}