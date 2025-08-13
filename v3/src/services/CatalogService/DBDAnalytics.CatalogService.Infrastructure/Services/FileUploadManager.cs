using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Requests.Shared;

namespace DBDAnalytics.CatalogService.Infrastructure.Services
{
    internal sealed class FileUploadManager(IFileStorageService fileStorageService) : IFileUploadManager
    {
        private readonly IFileStorageService _fileStorageService = fileStorageService;
        private readonly List<string> _uploadedFilePaths = [];

        public async Task<ImageKey?> UploadImageAsync(FileInput? file, string folder, string semanticName, CancellationToken cancellationToken)
        {
            if (file is null)
                return null;

            await using var stream = file.Content;
            var imageKey = await _fileStorageService.UploadImage(stream, file.FileName, file.ContentType, folder, semanticName, cancellationToken);

            if (imageKey is not null)
            {
                var fullPath = $"{folder}/{imageKey.Value}";
                _uploadedFilePaths.Add(fullPath);
            }

            return imageKey;
        }

        public async Task RollbackUploadsAsync(CancellationToken cancellationToken)
        {
            foreach (var path in _uploadedFilePaths)
            {
                try
                {
                    await _fileStorageService.DeleteImageAsync(path, cancellationToken);
                }
                catch (Exception)
                {
                    // TODO: Залогировать как критическую ошибку.
                }
            }
        }
    }
}