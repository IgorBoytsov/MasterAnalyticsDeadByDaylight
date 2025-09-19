using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct GameModeId
    {
        public int Value { get; }

        internal GameModeId(int value) => Value = value;

        public static GameModeId From(int value)
        {
            GuardException.Against.That(value <= 0, () => new ArgumentException("Id игрового режима должен быть положительным числом.", nameof(value)));

            return new GameModeId(value);
        }

        public static implicit operator int(GameModeId gameModeId) => gameModeId.Value;
    }
}