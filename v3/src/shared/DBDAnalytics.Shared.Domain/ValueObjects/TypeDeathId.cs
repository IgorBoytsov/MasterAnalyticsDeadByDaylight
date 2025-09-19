using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct TypeDeathId
    {
        public int Value { get; }

        internal TypeDeathId(int value) => Value = value;

        public static TypeDeathId From(int typeDeathId)
        {
            GuardException.Against.That(typeDeathId <= 0, () => new ArgumentException("Id для типа смерти должно быть положительным числом."));

            return new TypeDeathId(typeDeathId);
        }

        public static implicit operator int(TypeDeathId typeDeathId) => typeDeathId.Value;
    }
}