using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.HttpClients;
using Shared.Kernel.Results;
using Shared.Utilities;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Item
{
    public sealed class ItemApiService(HttpClient client) 
        : BaseApiService<ItemSoloResponse, string>(client, "api/items"), IItemService
    {
        public async Task<Result<ItemResponse>> AddAsync(ClientAddItemRequest request)
        {
            using var formData = new MultipartFormDataContent
            {
                { new StringContent(request.OldId.ToString()), nameof(request.OldId) },
                { new StringContent(request.Name), nameof(request.Name) },
                { new StringContent(request.Name), "SemanticImageName" },
            };

            if (!string.IsNullOrEmpty(request.ImagePath) && File.Exists(request.ImagePath))
            {
                var fileStream = File.OpenRead(request.ImagePath);
                var streamContent = new StreamContent(fileStream);

                var mimeType = MimeTypeHelper.GetMimeType(request.ImagePath);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                formData.Add(streamContent, "Image", Path.GetFileName(request.ImagePath));
            }

            for (int i = 0; i < request.Addons.Count; i++)
            {
                var addon = request.Addons[i];
                var prefix = $"{nameof(request.Addons)}[{i}]";

                formData.Add(new StringContent(addon.OldId.ToString()), $"{prefix}.OldId");
                formData.Add(new StringContent(addon.Name), $"{prefix}.Name");
                formData.Add(new StringContent(addon.Name), $"{prefix}.SemanticImageName");

                if (!string.IsNullOrEmpty(addon.ImagePath) && File.Exists(addon.ImagePath))
                {
                    var fileStream = File.OpenRead(addon.ImagePath);
                    var streamContent = new StreamContent(fileStream);

                    var mimeType = MimeTypeHelper.GetMimeType(addon.ImagePath);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                    formData.Add(streamContent, $"{prefix}.Image", Path.GetFileName(addon.ImagePath));
                }
            }

            try
            {
                var response = await _httpClient.PostAsync(_endpoint, formData);

                if (response.IsSuccessStatusCode)
                {
                    var createdItem = await response.Content.ReadFromJsonAsync<ItemResponse>();

                    if (createdItem is null)
                        return Result<ItemResponse>.Failure(new Error(ErrorCode.ApiError, "API вернул пустой успешный ответ."));

                    return Result<ItemResponse>.Success(createdItem);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Result<ItemResponse>.Failure(new Error(ErrorCode.ApiError, $"Ошибка API: {response.StatusCode}, {errorContent}"));
                }
            }
            catch (Exception ex)
            {
                return Result<ItemResponse>.Failure(new Error(ErrorCode.ApiError, ex.Message));
            }
        }

        public async Task<Result<string>> UpdateAsync(ClientUpdateItemRequest request)
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

                formData.Add(streamContent, "NewImage", Path.GetFileName(request.ImagePath));
            }

            try
            {
                var response = await _httpClient.PatchAsync($"{_endpoint}/{request.Id}", formData);

                if (response.IsSuccessStatusCode)
                {
                    var imageKey = await response.Content.ReadAsStringAsync();
                    return Result<string>.Success(imageKey!);
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