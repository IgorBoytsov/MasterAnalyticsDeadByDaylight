using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.GameMode
{
    public sealed record GameModeName
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; } = null!;

        internal GameModeName(string value) => Value = value;

        public static GameModeName Create(string value)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(value), () => new ArgumentException("Название игрового режима не может быть пустым.", nameof(value)));
            GuardException.Against.That(value.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для игрового режима {MAX_LENGTH} символов"));
            
            return new GameModeName(value);
        }

        public override string ToString() => Value;

        public static implicit operator string(GameModeName value) => value.Value;
    }
}