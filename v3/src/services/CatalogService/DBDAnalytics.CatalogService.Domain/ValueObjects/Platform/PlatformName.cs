using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Platform
{
    public sealed record PlatformName
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; }

        internal PlatformName(string value) => Value = value;

        public static PlatformName Create(string platformName)
        {
            GuardException.Against.NullOrWhiteSpace(platformName, nameof(platformName));
            GuardException.Against.That(platformName.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для платформы {MAX_LENGTH} символов"));

            return new PlatformName(platformName);
        }

        public override string ToString() => Value;

        public static implicit operator string(PlatformName platformName) => platformName.Value;
    }
}