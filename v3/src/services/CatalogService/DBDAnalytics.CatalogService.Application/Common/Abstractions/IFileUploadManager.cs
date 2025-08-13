using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Requests.Shared;

namespace DBDAnalytics.CatalogService.Application.Common.Abstractions
{
    public interface IFileUploadManager
    {
        /// <summary>
        /// Загружает изображение, сохраняя его ключ для возможного отката.
        /// </summary>
        Task<ImageKey?> UploadImageAsync(FileInput? file, string folder, string semanticName, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет все файлы, загруженные в рамках текущей операции.
        /// Вызывается в блоке catch при возникновении ошибки.
        /// </summary>
        Task RollbackUploadsAsync(CancellationToken cancellationToken);
    }
}