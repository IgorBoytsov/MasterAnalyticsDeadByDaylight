using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.KillerPerkCategory
{
    public readonly record struct KillerPerkCategoryId
    {
        public int Value { get; }

        internal KillerPerkCategoryId(int value) => Value = value;

        public static KillerPerkCategoryId From(int value)
        {
            GuardException.Against.That(value <= 0, () => new ArgumentException("Id категории перка должен быть положительным числом.", nameof(value)));

            return new KillerPerkCategoryId(value);
        }

        public static implicit operator int(KillerPerkCategoryId killerPerkCategoryId) => killerPerkCategoryId.Value;
    }
}