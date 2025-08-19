using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.WhoPlacedMap
{
    public sealed record PlacedMapName
    {
        public const int MAX_LENGTH = 50;

        public string Value { get; private set; } = null!;

        internal PlacedMapName(string value) => Value = value;

        /// <exception cref="ArgumentException"></exception>
        public static PlacedMapName Create(string name)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(name), () => new ArgumentException("Нельзя оставлять пустым с информацией о том, кто поставил карту.", nameof(name)));
            GuardException.Against.That(name.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Максимально допустимая длина для названия того, кто поставил карту {MAX_LENGTH}", nameof(name)));

            return new PlacedMapName(name);
        }

        public override string ToString() => Value;

        public static implicit operator string(PlacedMapName name) => name.Value;
    }
}