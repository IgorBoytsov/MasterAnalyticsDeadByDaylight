using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients;
using Shared.Utilities;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.Killer
{
    public sealed class KillerApiService(HttpClient client) 
        : BaseApiService<KillerSoloResponse, string>(client, "api/killers"), IKillerService
    {
        public async Task<Result<KillerResponse>> AddAsync(ClientAddKillerRequest request)
        {
            using var formData = new MultipartFormDataContent
            {
                { new StringContent(request.OldId.ToString()), nameof(request.OldId) },
                { new StringContent(request.Name), nameof(request.Name) },
                { new StringContent(request.Name), "SemanticKillerImageName" },
                { new StringContent(request.Name), "SemanticAbilityImageName" },
            };

            if (!string.IsNullOrEmpty(request.KillerImagePath) && File.Exists(request.KillerImagePath))
            {
                var fileStream = File.OpenRead(request.KillerImagePath);
                var streamContent = new StreamContent(fileStream);

                var mimeType = MimeTypeHelper.GetMimeType(request.KillerImagePath);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                formData.Add(streamContent, "KillerImage", Path.GetFileName(request.KillerImagePath));
            }

            if (!string.IsNullOrEmpty(request.KillerAbilityImagePath) && File.Exists(request.KillerAbilityImagePath))
            {
                var fileStream = File.OpenRead(request.KillerAbilityImagePath);
                var streamContent = new StreamContent(fileStream);

                var mimeType = MimeTypeHelper.GetMimeType(request.KillerAbilityImagePath);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                formData.Add(streamContent, "AbilityImage", Path.GetFileName(request.KillerAbilityImagePath));
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

            for (int i = 0; i < request.Perks.Count; i++)
            {
                var map = request.Perks[i];
                var prefix = $"{nameof(request.Perks)}[{i}]";

                formData.Add(new StringContent(map.OldId.ToString()), $"{prefix}.OldId");
                formData.Add(new StringContent(map.Name), $"{prefix}.Name");
                formData.Add(new StringContent(map.Name), $"{prefix}.SemanticImageName");

                if (!string.IsNullOrEmpty(map.ImagePath) && File.Exists(map.ImagePath))
                {
                    var fileStream = File.OpenRead(map.ImagePath);
                    var streamContent = new StreamContent(fileStream);

                    var mimeType = MimeTypeHelper.GetMimeType(map.ImagePath);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                    formData.Add(streamContent, $"{prefix}.Image", Path.GetFileName(map.ImagePath));
                }
            }

            try
            {
                var response = await _httpClient.PostAsync(_endpoint, formData);

                if (response.IsSuccessStatusCode)
                {
                    var createdKiller = await response.Content.ReadFromJsonAsync<KillerResponse>();

                    if (createdKiller is null)
                        return Result<KillerResponse>.Failure(new Error(ErrorCode.ApiError, "API вернул пустой успешный ответ."));

                    return Result<KillerResponse>.Success(createdKiller);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Result<KillerResponse>.Failure(new Error(ErrorCode.ApiError, $"Ошибка API: {response.StatusCode}, {errorContent}"));
                }
            }
            catch (Exception ex)
            {
                return Result<KillerResponse>.Failure(new Error(ErrorCode.ApiError, ex.Message));
            }
        }

        public async Task<Result<KillersImageKeysResponse>> UpdateAsync(ClientUpdateKillerRequest request)
        {
            using var formData = new MultipartFormDataContent
            {
                { new StringContent(request.Name), "Name" },
                { new StringContent(request.SemanticImageAbilityName), "SemanticImageAbilityName" },
                { new StringContent(request.SemanticImagePortraitName), "SemanticImagePortraitName" },
            };

            if (!string.IsNullOrEmpty(request.ImagePortraitPath) && File.Exists(request.ImagePortraitPath))
            {
                var fileStream = File.OpenRead(request.ImagePortraitPath);
                var streamContent = new StreamContent(fileStream);

                var mimeType = MimeTypeHelper.GetMimeType(request.ImagePortraitPath);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                formData.Add(streamContent, "ImagePortrait", Path.GetFileName(request.ImagePortraitPath));
            }

            if (!string.IsNullOrEmpty(request.ImageAbilityPath) && File.Exists(request.ImageAbilityPath))
            {
                var fileStream = File.OpenRead(request.ImageAbilityPath);
                var streamContent = new StreamContent(fileStream);

                var mimeType = MimeTypeHelper.GetMimeType(request.ImageAbilityPath);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                formData.Add(streamContent, "ImageAbility", Path.GetFileName(request.ImageAbilityPath));
            }

            try
            {
                var response = await _httpClient.PatchAsync($"{_endpoint}/{request.Id}", formData);

                if (response.IsSuccessStatusCode)
                {
                    var imageKeys = await response.Content.ReadFromJsonAsync<KillersImageKeysResponse>();
                    return Result<KillersImageKeysResponse>.Success(imageKeys!);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Result<KillersImageKeysResponse>.Failure(new Error(ErrorCode.ApiError, $"Ошибка API: {response.StatusCode}, {errorContent}"));
                }
            }
            catch (Exception ex)
            {
                return Result<KillersImageKeysResponse>.Failure(new Error(ErrorCode.ApiError, ex.Message));
            }
        }
    }
}