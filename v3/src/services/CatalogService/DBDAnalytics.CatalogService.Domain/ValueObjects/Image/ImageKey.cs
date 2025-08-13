using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Image
{
    public sealed record ImageKey
    {
        private const char Delimiter = ':';

        public Guid Id { get; private set; }
        public string Category { get; private set; } = null!;
        public string Extension { get; private set; } = null!;
        public string SemanticName { get; private set; }
        public string Value { get; private set; } = null!;

        private static readonly string[] _allowedExtensions = { ".png", ".jpg", ".jpeg" };

        private ImageKey(string value, string semanticName, Guid id, string extension)
        {
            Value = value;
            Id = id;
            Extension = extension;
            SemanticName = semanticName;
        }

        /// <summary>
        /// Парсит существующую строку ключа.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public static ImageKey Create(string keyString)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(keyString), () => new ArgumentException("Image key не может быть пустым.", nameof(keyString)));

            var extension = Path.GetExtension(keyString);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(keyString);

            GuardException.Against.That(string.IsNullOrEmpty(extension) || string.IsNullOrEmpty(fileNameWithoutExtension), () => new FormatException("Image key должен иметь имя и расширение."));

            var parts = fileNameWithoutExtension.Split(Delimiter);
            GuardException.Against.That(parts.Length != 2, () => new FormatException($"Ключ изображения должен содержать семантическое имя и GUID, разделенные '{Delimiter}'."));

            var semanticName = parts[0];
            var guidString = parts[1];

            GuardException.Against.That(!Guid.TryParse(guidString, out var guid), () => new FormatException($"Имя (основная часть) ключа изображения '{fileNameWithoutExtension}' не является допустимым GUID."));

            GuardException.Against.That(!_allowedExtensions.Contains(extension.ToLowerInvariant()), () => new FormatException($"Расширение изображения '{extension}' не поддерживается."));

            return new ImageKey(keyString, semanticName, guid, extension);
        }

        /// <summary>
        /// Создает новый ключ с заданным семантическим именем.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static ImageKey New(string semanticName, string extension = ".png")
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(semanticName), () => new ArgumentException("Семантическое имя не может быть пустым.", nameof(semanticName)));
            GuardException.Against.That(semanticName.Contains(Delimiter), () => new ArgumentException($"Семантическое имя не должно содержать разделитель '{Delimiter}'.", nameof(semanticName)));
            GuardException.Against.That(string.IsNullOrEmpty(extension) || !extension.StartsWith("."), () => new ArgumentException("Расширение должно содержать в начале точку.", nameof(extension)));

            var guid = Guid.NewGuid();
            var keyString = $"{semanticName}{Delimiter}{guid}{extension}";
            return new ImageKey(keyString, semanticName, guid, extension);
        }

        /// <summary>
        /// Возвращает полное строковое представление ключа - "Название:guidId.расширение"
        /// </summary>
        public override string ToString() => Value;

        public static implicit operator string(ImageKey key) => key.Value;

        /// <summary>
        /// Позволяет явно преобразовывать string в ImageKey. Может выбросить исключение.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static explicit operator ImageKey(string keyString) => Create(keyString);
    }
}