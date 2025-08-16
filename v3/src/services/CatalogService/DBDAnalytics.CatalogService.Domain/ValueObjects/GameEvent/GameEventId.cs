using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.GameEvent
{
    public readonly struct GameEventId
    {
        public int Value { get; }

        internal GameEventId(int value) => Value = value;

        public static GameEventId From(int value)
        {
            GuardException.Against.That(value <= 0, () => new ArgumentException("Id игрового ивента должен быть положительным числом.", nameof(value)));

            return new GameEventId(value);
        }

        public static implicit operator int(GameEventId gameModeId) => gameModeId.Value;
    }
}