using Shared.WPF.Enums;

namespace Shared.WPF.Services.Initializers
{
    public interface ITokenService
    {
        string GetToken(AppType type);
        void SaveToken(string token, AppType type);
    }
}