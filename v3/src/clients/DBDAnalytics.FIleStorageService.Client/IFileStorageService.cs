namespace DBDAnalytics.FIleStorageService.Client
{
    public interface IFileStorageService
    {
        Task<string> GetPresignedLinkAsync(string key, CancellationToken cancellationToken = default);
    }
}