using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct GameModeId
    {
        public int Value { get; }

        internal GameModeId(int value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static GameModeId From(int value)
        {
            GuardException.Against.That(value <= 0, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id игрового режима должен быть положительным числом.")));

            return new GameModeId(value);
        }

        public static implicit operator int(GameModeId gameModeId) => gameModeId.Value;
    }
}