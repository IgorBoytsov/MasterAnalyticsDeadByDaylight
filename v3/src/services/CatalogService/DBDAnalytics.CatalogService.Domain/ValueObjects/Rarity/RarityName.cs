using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity
{
    public sealed record RarityName
    {
        public const int MAX_LENGTH = 50;

        public string Value { get; } = null!;

        internal RarityName(string value) => Value = value;

        public static RarityName Create(string rarityName)
        {
            GuardException.Against.NullOrWhiteSpace(rarityName, nameof(rarityName));
            GuardException.Against.That(rarityName.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для категории {MAX_LENGTH} символов"));

            return new RarityName(rarityName);
        }

        public override string ToString() => Value;

        public static implicit operator string(RarityName rarityName) => rarityName.Value;
    }
}