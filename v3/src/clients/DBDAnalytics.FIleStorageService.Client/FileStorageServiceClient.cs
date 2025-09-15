namespace DBDAnalytics.FIleStorageService.Client
{
    public class FileStorageServiceClient : IFileStorageService
    {
        private readonly HttpClient _httpClient;

        public FileStorageServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GetPresignedLinkAsync(string key, CancellationToken cancellationToken = default)
        {
            var encodedKey = Uri.EscapeDataString(key);

            var response = await _httpClient.GetAsync($"api/files/link?key={encodedKey}", cancellationToken);


            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}