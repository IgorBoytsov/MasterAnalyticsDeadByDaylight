using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct OfferingId
    {
        public Guid Value { get; }

        internal OfferingId(Guid value) => Value = value;

        public static OfferingId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new ArgumentException("Идентификатор подношения не может быть пустым"));

            return new OfferingId(value);
        }

        public static implicit operator Guid(OfferingId offeringId) => offeringId.Value;
    }
}