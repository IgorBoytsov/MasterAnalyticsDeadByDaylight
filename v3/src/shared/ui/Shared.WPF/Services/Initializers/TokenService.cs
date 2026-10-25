using Shared.WPF.Enums;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Shared.WPF.Services.Initializers
{
    public sealed class TokenService : ITokenService
    {
        private readonly string _fileAdminPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DBDAnalytics", "tokenAdmin.data");
        private readonly string _fileClientPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DBDAnalytics", "tokenClient.data");
        private static readonly byte[] s_entropy = Encoding.Unicode.GetBytes("3yl0uJIFfN7jI432aUJe23wrFXWhygCQ");

        public void SaveToken(string token, AppType type)
        {
            byte[] tokenBytes = Encoding.Unicode.GetBytes(token);

            byte[] encryptedBytes = ProtectedData.Protect(tokenBytes, s_entropy, DataProtectionScope.CurrentUser);

            Action action = type switch
            {
                AppType.Admin => () =>
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_fileAdminPath)!);
                    File.WriteAllBytes(_fileAdminPath, encryptedBytes);
                }
                ,
                AppType.Client => () =>
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_fileClientPath)!);
                    File.WriteAllBytes(_fileClientPath, encryptedBytes);
                },
                _ => () => throw new Exception($"Такого типа не зарегестрировано {type}")
            };
            action?.Invoke();  
        }

        public string GetToken(AppType type)
        {
            Func<string> func = type switch
            {
                AppType.Admin => () =>
                {
                    if (!File.Exists(_fileAdminPath))
                        return null;

                    try
                    {
                        byte[] encryptedBytes = File.ReadAllBytes(_fileAdminPath);

                        byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, s_entropy, DataProtectionScope.CurrentUser);

                        return Encoding.Unicode.GetString(decryptedBytes);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                ,
                AppType.Client => () =>
                {
                    if (!File.Exists(_fileClientPath))
                        return null;

                    try
                    {
                        byte[] encryptedBytes = File.ReadAllBytes(_fileClientPath);

                        byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, s_entropy, DataProtectionScope.CurrentUser);

                        return Encoding.Unicode.GetString(decryptedBytes);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                ,
                _ => () => throw new Exception($"Такого типа не зарегестрировано {type}")
            };

            return func?.Invoke();
        }
    }
}