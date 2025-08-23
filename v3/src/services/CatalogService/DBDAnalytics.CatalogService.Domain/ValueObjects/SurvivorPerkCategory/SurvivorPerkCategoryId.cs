using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory
{
    public readonly struct SurvivorPerkCategoryId
    {
        public int Value { get; }

        internal SurvivorPerkCategoryId(int value) => Value = value;

        public static SurvivorPerkCategoryId From(int value)
        {
            GuardException.Against.That(value <= 0, () => new ArgumentException("Id для категории перков выжившего должно быть положительным числом."));
            
            return new SurvivorPerkCategoryId(value);
        }

        public static implicit operator int(SurvivorPerkCategoryId survivorPerkCategoryId) => survivorPerkCategoryId.Value;
    }
}