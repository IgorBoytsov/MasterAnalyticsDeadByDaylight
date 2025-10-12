using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct SurvivorId
    {
        public Guid Value { get; }

        internal SurvivorId(Guid value) => Value = value;

        public static SurvivorId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new ArgumentException("Идентификатор выжившего не может быть пустым"));

            return new SurvivorId(value);
        }

        public static implicit operator Guid(SurvivorId survivorId) => survivorId.Value;
    }
}