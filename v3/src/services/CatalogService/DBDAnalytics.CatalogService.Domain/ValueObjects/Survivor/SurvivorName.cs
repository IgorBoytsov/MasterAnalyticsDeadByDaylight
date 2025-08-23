using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Survivor
{
    public sealed record SurvivorName
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; }

        internal SurvivorName(string value) => Value = value;

        public static SurvivorName Create(string survivorName)
        {
            GuardException.Against.NullOrWhiteSpace(survivorName, nameof(survivorName));
            GuardException.Against.That(survivorName.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для имени выжившего {MAX_LENGTH} символов"));

            return new SurvivorName(survivorName);
        }

        public override string ToString() => Value;

        public static implicit operator string(SurvivorName survivorName) => survivorName.Value;
    }
}