using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Map
{
    public sealed record MapName
    {
        public const int MAX_LENGTH = 200;

        public string Value { get; private set; } = null!;

        internal MapName(string value) => Value = value;

        /// <exception cref="ArgumentException"></exception>
        public static MapName Create(string name)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(name), () => new ArgumentException("Название карты не может быть пустым.", nameof(name)));
            GuardException.Against.That(name.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Максимально допустимая длина для названия карты {MAX_LENGTH}", nameof(name)));

            return new MapName(name);
        }

        public override string ToString() => Value;

        public static implicit operator string(MapName name) => name.Value;
    }
}