using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct WhoPlacedMapId
    {
        public int Value { get; }

        internal WhoPlacedMapId(int value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static WhoPlacedMapId From(int value)
        {
            GuardException.Against.That(value <= 0, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id того, кто поставил карту должен быть положительным числом.")));

            return new WhoPlacedMapId(value);
        }

        public static implicit operator int(WhoPlacedMapId whoPlacedMapId) => whoPlacedMapId.Value;
    }
}