using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;

namespace DBDAnalytics.CatalogService.Application.Common.Abstractions
{
    public interface IFileStorageService
    {
        Task<ImageKey?> UploadImage(Stream stream, string fileName, string contentType, string category, string semanticName, CancellationToken cancellationToken = default);
        Task DeleteImageAsync(string fullObjectPath, CancellationToken cancellationToken = default);
    }
}