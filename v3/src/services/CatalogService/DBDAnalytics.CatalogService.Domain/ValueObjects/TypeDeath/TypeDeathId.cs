using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.TypeDeath
{
    public readonly struct TypeDeathId
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