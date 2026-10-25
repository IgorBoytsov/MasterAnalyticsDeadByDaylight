namespace DBDAnalytics.UserService.Client.Models.Responses
{
    public sealed record AuthenticationResponse(string AccessToken, string RefreshToken);
}