using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct GameEventId
    {
        public int Value { get; }

        internal GameEventId(int value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static GameEventId From(int value)
        {
            GuardException.Against.That(value <= 0, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id игрового ивента должен быть положительным числом.")));

            return new GameEventId(value);
        }

        public static implicit operator int(GameEventId gameModeId) => gameModeId.Value;
    }
}