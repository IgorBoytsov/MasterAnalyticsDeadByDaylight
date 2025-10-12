namespace DBDAnalytics.UserService.Client.Models.Responses
{
    public sealed record UserResponse(string Id, string Login, string UserName, string Email, string? Phone, string StatusName, DateTime DateRegistration, DateTime? DateEntry);
}