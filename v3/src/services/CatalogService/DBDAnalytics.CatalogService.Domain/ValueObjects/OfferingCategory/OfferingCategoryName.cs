using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory
{
    public sealed record OfferingCategoryName
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; }

        internal OfferingCategoryName(string value) => Value = value;

        public static OfferingCategoryName Create(string offeringCategoryName)
        {
            GuardException.Against.NullOrWhiteSpace(offeringCategoryName, nameof(offeringCategoryName));
            GuardException.Against.That(offeringCategoryName.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для категории {MAX_LENGTH} символов"));

            return new OfferingCategoryName(offeringCategoryName);
        }

        public override string ToString() => Value;

        public static implicit operator string(OfferingCategoryName offeringCategoryName) => offeringCategoryName.Value;
    }
}