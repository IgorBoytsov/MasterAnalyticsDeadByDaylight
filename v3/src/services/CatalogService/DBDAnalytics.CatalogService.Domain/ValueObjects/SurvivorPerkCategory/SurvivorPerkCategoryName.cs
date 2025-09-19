using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory
{
    public sealed record SurvivorPerkCategoryName
    {
        public const int MAX_LENGTH = 50;

        public string Value { get; } = null!;

        internal SurvivorPerkCategoryName(string value) => Value = value;

        public static SurvivorPerkCategoryName Create(string survivorPerkCategoryName)
        {
            GuardException.Against.NullOrWhiteSpace(survivorPerkCategoryName, nameof(survivorPerkCategoryName));
            GuardException.Against.That(survivorPerkCategoryName.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для категории {MAX_LENGTH} символов"));

            return new SurvivorPerkCategoryName(survivorPerkCategoryName);
        }

        public override string ToString() => Value;

        public static implicit operator string(SurvivorPerkCategoryName survivorPerkCategoryName) => survivorPerkCategoryName.Value;
    }
}