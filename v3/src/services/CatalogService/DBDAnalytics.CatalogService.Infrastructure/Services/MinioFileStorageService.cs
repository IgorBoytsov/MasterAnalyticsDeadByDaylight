using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using Minio;
using Minio.DataModel.Args;

namespace DBDAnalytics.CatalogService.Infrastructure.Services
{
    internal sealed class MinioFileStorageService(IMinioClient minioClient) : IFileStorageService
    {
        private readonly IMinioClient _minioClient = minioClient;
        private readonly string _bucketName = "dbd-catalog-service";

        public async Task<ImageKey?> UploadImage(Stream stream, string fileName, string contentType, string category, string semanticName, CancellationToken cancellationToken = default)
        {
            var extension = Path.GetExtension(fileName);
            var imageKey = ImageKey.New(semanticName, extension);
            var objectNameInStorage = $"{category}/{imageKey.Value}";

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectNameInStorage)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return imageKey;
        }

        public async Task DeleteImageAsync(string fullObjectPath, CancellationToken cancellationToken = default)
        {
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fullObjectPath);

                await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Произошла ошибка при удалении изображения из хранилища.", ex);
            }
        }
    }
}