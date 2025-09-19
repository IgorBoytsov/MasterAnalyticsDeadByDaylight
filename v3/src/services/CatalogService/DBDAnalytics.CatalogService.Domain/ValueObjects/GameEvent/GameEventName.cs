using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.GameEvent
{
    public sealed record GameEventName
    {
        public const int MAX_LENGTH = 100;
        public string Value { get; } = null!;

        internal GameEventName(string value) => Value = value;

        public static GameEventName Create(string value)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(value), () => new ArgumentException("Название игрового ивента не может быть пустым.", nameof(value)));
            GuardException.Against.That(value.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для игрового ивента {MAX_LENGTH} символов"));

            return new GameEventName(value);
        }

        public override string ToString() => Value;

        public static implicit operator string(GameEventName value) => value.Value;
    }
}