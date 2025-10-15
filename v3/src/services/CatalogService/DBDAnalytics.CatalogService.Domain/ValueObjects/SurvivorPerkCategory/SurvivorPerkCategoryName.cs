using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory
{
    public sealed record SurvivorPerkCategoryName
    {
        public const int MAX_LENGTH = 50;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        internal SurvivorPerkCategoryName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static SurvivorPerkCategoryName Create(string survivorPerkCategoryName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(survivorPerkCategoryName), () => new NameException(new Error(ErrorCode.Validation, "Название категории не может быть пустым.")));
            GuardException.Against.That(survivorPerkCategoryName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для категории от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new SurvivorPerkCategoryName(survivorPerkCategoryName);
        }

        public override string ToString() => Value;

        public static implicit operator string(SurvivorPerkCategoryName survivorPerkCategoryName) => survivorPerkCategoryName.Value;
    }
}