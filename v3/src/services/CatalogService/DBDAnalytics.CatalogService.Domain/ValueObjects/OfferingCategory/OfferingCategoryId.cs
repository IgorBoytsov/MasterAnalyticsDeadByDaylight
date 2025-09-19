using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory
{
    public readonly record struct OfferingCategoryId
    {
        public int Value { get; }

        internal OfferingCategoryId(int value) => Value = value;

        public static OfferingCategoryId From(int offeringCategoryId)
        {
            GuardException.Against.That(offeringCategoryId <= 0, () => new ArgumentException("Id для платформы должно быть положительным числом."));

            return new OfferingCategoryId(offeringCategoryId);
        }

        public static implicit operator int(OfferingCategoryId offeringCategoryId) => offeringCategoryId.Value;
    }
}