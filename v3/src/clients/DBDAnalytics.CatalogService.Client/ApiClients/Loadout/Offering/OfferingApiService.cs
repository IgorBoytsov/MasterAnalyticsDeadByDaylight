using DBDAnalytics.Shared.Contracts.Responses.Offering;
using Shared.HttpClients;
using Shared.Kernel.Results;
using Shared.Utilities;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Offering
{
    public class OfferingApiService(HttpClient httpClient)
        : BaseApiService<OfferingResponse, string>(httpClient, "api/offerings"), IOfferingService
    {
        public async Task<Result<string>> UpdateAsync(
            string Id,
            string name,
            string semanticName,
            string imagePath,
            int roleId,
            int? rarityId,
            int? categoryId)
        {
            using var formData = new MultipartFormDataContent
            {
                { new StringContent(name), "NewName" },
                { new StringContent(semanticName), "SemanticName" },
                { new StringContent(roleId.ToString()), "RoleId" },
            };

            if (rarityId.HasValue)
                formData.Add(new StringContent(rarityId.Value.ToString()), "RarityId");

            if (categoryId.HasValue)
                formData.Add(new StringContent(categoryId.Value.ToString()), "CategoryId");

            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                var fileStream = File.OpenRead(imagePath);
                var streamContent = new StreamContent(fileStream);

                var mimeType = MimeTypeHelper.GetMimeType(imagePath);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                formData.Add(streamContent, "Image", Path.GetFileName(imagePath));
            }

            try
            {
                var response = await _httpClient.PatchAsync($"{_endpoint}/{Id}", formData);

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

        public async Task<Result<OfferingResponse>> AddAsync(
            int oldId,
            string name,
            string semanticName,
            string imagePath,
            int roleId,
            int? rarityId,
            int? categoryId)
        {
            using var formData = new MultipartFormDataContent
            {
                { new StringContent(oldId.ToString()), "OldId" },
                { new StringContent(name), "Name" },
                { new StringContent(semanticName), "SemanticName" },
                { new StringContent(roleId.ToString()), "RoleId" }
            };

            if (rarityId.HasValue)
                formData.Add(new StringContent(rarityId.Value.ToString()), "RarityId");

            if (categoryId.HasValue)
                formData.Add(new StringContent(categoryId.Value.ToString()), "CategoryId");

            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                var fileStream = File.OpenRead(imagePath);
                var streamContent = new StreamContent(fileStream);

                var mimeType = MimeTypeHelper.GetMimeType(imagePath);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                formData.Add(streamContent, "Image", Path.GetFileName(imagePath));
            }

            try
            {
                var response = await _httpClient.PostAsync(_endpoint, formData);

                if (response.IsSuccessStatusCode)
                {
                    var createdOffering = await response.Content.ReadFromJsonAsync<OfferingResponse>();
                    return Result<OfferingResponse>.Success(createdOffering!);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Result<OfferingResponse>.Failure(new Error(ErrorCode.ApiError, $"Ошибка API: {response.StatusCode}, {errorContent}"));
                }
            }
            catch (Exception ex)
            {
                return Result<OfferingResponse>.Failure(new Error(ErrorCode.ApiError, ex.Message));
            }
        }

        public async Task<List<OfferingResponse>> GetAllForKillerAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_endpoint}/killers");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<List<OfferingResponse>>(_jsonSerializerOptions);
                return result ?? [];
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при извлечении всех элементов из {_endpoint}: {ex.Message}");
                return [];
            }
        }

        public async Task<List<OfferingResponse>> GetAllForSurvivorAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_endpoint}/survivors");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<List<OfferingResponse>>(_jsonSerializerOptions);
                return result ?? [];
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при извлечении всех элементов из {_endpoint}: {ex.Message}");
                return [];
            }
        }
    }
}