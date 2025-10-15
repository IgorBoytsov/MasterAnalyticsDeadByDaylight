using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct TypeDeathId
    {
        public int Value { get; }

        internal TypeDeathId(int value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static TypeDeathId From(int typeDeathId)
        {
            GuardException.Against.That(typeDeathId <= 0, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id типа смерти должен быть положительным числом.")));

            return new TypeDeathId(typeDeathId);
        }

        public static implicit operator int(TypeDeathId typeDeathId) => typeDeathId.Value;
    }
}