namespace DBDAnalytics.UserService.Client.Models.Requests
{
    public sealed record class RegisterUserRequest(string Login, string UserName, string Password, string Email, string? Phone, string? IdGender, string? IdCountry);
}