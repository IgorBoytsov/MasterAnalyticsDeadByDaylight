using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity
{
    public readonly record struct RarityId
    {
        public int Value { get; }

        internal RarityId(int rarityId) => Value = rarityId;

        public static RarityId From(int value)
        {
            GuardException.Against.That(value <= 0, () => new ArgumentException("Id для качества должно быть положительным числом."));
            
            return new RarityId(value);
        }

        public static implicit operator int(RarityId rarityId) => rarityId.Value;
    }
}