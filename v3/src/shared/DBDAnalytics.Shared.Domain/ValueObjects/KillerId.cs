using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct KillerId
    {
        public Guid Value { get; }

        internal KillerId(Guid value) => Value = value;

        public static KillerId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new ArgumentException("Идентификатор киллера не может быть пустым"));

            return new KillerId(value);
        }

        public static implicit operator Guid(KillerId killerId) => killerId.Value;
    }
}