using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.HttpClients;
using Shared.Kernel.Results;
using Shared.Utilities;
using System.Net.Http.Headers;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerk
{
    public sealed class SurvivorPerkApiService(HttpClient client, string survivorId)
        : BaseApiService<SurvivorPerkResponse, string>(client, $"api/survivors/{survivorId}/perks"), ISurvivorPerkService
    {
        public async Task<Result<string>> UpdateAsync(ClientUpdateSurvivorPerkRequest request)
        {
            using var formData = new MultipartFormDataContent
            {
                { new StringContent(request.NewName), "NewName" },
                { new StringContent(request.SemanticName), "SemanticName" },
            };

            if (!string.IsNullOrEmpty(request.ImagePath) && File.Exists(request.ImagePath))
            {
                var fileStream = File.OpenRead(request.ImagePath);
                var streamContent = new StreamContent(fileStream);

                var mimeType = MimeTypeHelper.GetMimeType(request.ImagePath);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                formData.Add(streamContent, "Image", Path.GetFileName(request.ImagePath));
            }

            try
            {
                var response = await _httpClient.PatchAsync($"{_endpoint}/{request.Id}", formData);

                if (response.IsSuccessStatusCode)
                {
                    var imageKey = await response.Content.ReadAsStringAsync();
                    return Result<string>.Success(imageKey);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Result<string>.Failure(new Error(ErrorCode.ApiError, $"Ошибка API: {response.StatusCode}, {errorContent}"));
                }
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(new Error(ErrorCode.ApiError, ex.Message));
            }
        }
    }
}