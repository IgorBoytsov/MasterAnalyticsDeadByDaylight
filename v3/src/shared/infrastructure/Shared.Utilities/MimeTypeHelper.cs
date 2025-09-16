namespace Shared.Utilities
{
    public static class MimeTypeHelper
    {
        private static readonly Dictionary<string, string> MimeMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" },
            { ".svg", "image/svg+xml" },
        };

        public static string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            if (extension == null || !MimeMappings.TryGetValue(extension, out var mimeType))
                return "application/octet-stream";

            return mimeType;
        }
    }
}