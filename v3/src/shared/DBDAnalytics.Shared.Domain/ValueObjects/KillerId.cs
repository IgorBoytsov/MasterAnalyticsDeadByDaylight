using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct KillerId
    {
        public Guid Value { get; }

        internal KillerId(Guid value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static KillerId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id киллера не должен быть пустым GUID.")));

            return new KillerId(value);
        }

        public static implicit operator Guid(KillerId killerId) => killerId.Value;
    }
}