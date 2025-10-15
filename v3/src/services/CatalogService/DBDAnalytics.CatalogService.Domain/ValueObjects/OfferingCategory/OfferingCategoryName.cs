using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory
{
    public sealed record OfferingCategoryName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 1;

        public string Value { get; }

        internal OfferingCategoryName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static OfferingCategoryName Create(string offeringCategoryName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(offeringCategoryName), () => new NameException(new Error(ErrorCode.Validation, "Название категории не может быть пустым.")));
            GuardException.Against.That(offeringCategoryName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для категории от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new OfferingCategoryName(offeringCategoryName);
        }

        public override string ToString() => Value;

        public static implicit operator string(OfferingCategoryName offeringCategoryName) => offeringCategoryName.Value;
    }
}