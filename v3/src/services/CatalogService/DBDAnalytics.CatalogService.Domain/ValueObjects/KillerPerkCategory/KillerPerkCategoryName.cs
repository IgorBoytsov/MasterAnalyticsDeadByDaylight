using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.KillerPerkCategory
{
    public sealed record KillerPerkCategoryName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 1;

        public string Value { get; private set; } = null!;

        internal KillerPerkCategoryName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static KillerPerkCategoryName Create(string categoryName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(categoryName), () => new NameException(new Error(ErrorCode.Validation, "Название категории для перка не может быть пустым.")));
            GuardException.Against.That(categoryName.Length < 1 || categoryName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для категории от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new KillerPerkCategoryName(categoryName);
        }

        public override string ToString() => Value;

        public static implicit operator string(KillerPerkCategoryName perkName) => perkName.Value;
    }
}