using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Offering
{
    public sealed record OfferingName
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; private set; } = null!;

        internal OfferingName(string value) => Value = value;

        public static OfferingName Create(string offeringName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(offeringName), () => new ArgumentException("Название подношения не может быть пустым."));

            return new OfferingName(offeringName);
        }

        public override string ToString() => Value;

        public static implicit operator string(OfferingName offeringName) => offeringName.Value;
    }
}