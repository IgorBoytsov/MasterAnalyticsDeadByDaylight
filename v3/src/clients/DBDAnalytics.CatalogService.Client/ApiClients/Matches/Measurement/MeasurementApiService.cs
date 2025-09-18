using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients;
using Shared.Utilities;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Measurement
{
    public class MeasurementApiService(HttpClient httpClient)
        : BaseApiService<MeasurementSoloResponse, string>(httpClient, "api/measurements"), IMeasurementService
    {
        public async Task<Result<MeasurementSoloResponse>> AddAsync(ClientAddMeasurementRequest request)
        {
            using var formData = new MultipartFormDataContent
            {
                { new StringContent(request.OldId.ToString()), nameof(request.OldId) },
                { new StringContent(request.Name), nameof(request.Name) }
            };

            for (int i = 0; i < request.Maps.Count; i++)
            {
                var map = request.Maps[i];
                var prefix = $"{nameof(request.Maps)}[{i}]"; // => "Maps[0]", "Maps[1]", ...

                formData.Add(new StringContent(map.OldId.ToString()), $"{prefix}.OldId");
                formData.Add(new StringContent(map.Name), $"{prefix}.Name");
                formData.Add(new StringContent(map.SemanticImageName), $"{prefix}.SemanticImageName");

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
                    var createdMeasurement = await response.Content.ReadFromJsonAsync<MeasurementSoloResponse>();

                    if (createdMeasurement is null)
                        return Result<MeasurementSoloResponse>.Failure(new Error(ErrorCode.ApiError, "API вернул пустой успешный ответ."));

                    return Result<MeasurementSoloResponse>.Success(createdMeasurement);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Result<MeasurementSoloResponse>.Failure(new Error(ErrorCode.ApiError, $"Ошибка API: {response.StatusCode}, {errorContent}"));
                }
            }
            catch (Exception ex)
            {
                return Result<MeasurementSoloResponse>.Failure(new Error(ErrorCode.ApiError, ex.Message));
            }
        }
    }
}