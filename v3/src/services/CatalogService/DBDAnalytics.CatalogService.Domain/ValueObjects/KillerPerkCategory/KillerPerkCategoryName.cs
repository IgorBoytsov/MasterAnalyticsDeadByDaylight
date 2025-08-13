using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Killer
{
    public sealed record KillerPerkCategoryName
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; private set; } = null!;

        internal KillerPerkCategoryName(string value) => Value = value;

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static KillerPerkCategoryName Create(string categoryName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(categoryName), () => new ArgumentException("Название категории для перка не может быть пустым.", nameof(categoryName)));
            GuardException.Against.That(categoryName.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для категории {MAX_LENGTH} символов"));

            return new KillerPerkCategoryName(categoryName);
        }

        public override string ToString() => Value;

        public static implicit operator string(KillerPerkCategoryName perkName) => perkName.Value;
    }
}