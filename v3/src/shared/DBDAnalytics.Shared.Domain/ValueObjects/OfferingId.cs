using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct OfferingId
    {
        public Guid Value { get; }

        internal OfferingId(Guid value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static OfferingId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id подношения не должен быть пустым GUID.")));

            return new OfferingId(value);
        }

        public static implicit operator Guid(OfferingId offeringId) => offeringId.Value;
    }
}