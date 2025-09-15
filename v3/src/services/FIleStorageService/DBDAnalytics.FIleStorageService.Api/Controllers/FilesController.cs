using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.IO.Compression;

namespace DBDAnalytics.FIleStorageService.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;

        public FilesController(IMinioClient minioClient, IConfiguration configuration)
        {
            _minioClient = minioClient;
            _bucketName = configuration.GetValue<string>("Minio:BucketName") ?? throw new InvalidOperationException("Minio:BucketName не сконфигурирован.");
        }

        [HttpGet("link")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPresignedLink([FromQuery] string key)
        {
            try
            {
                var statArgs = new StatObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(key);
                await _minioClient.StatObjectAsync(statArgs);

                var args = new PresignedGetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(key)
                    .WithExpiry(3600);

                var presignedUrl = await _minioClient.PresignedGetObjectAsync(args);
                return Ok(presignedUrl);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound($"Файл с '{key}' ключом не найден.");
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}