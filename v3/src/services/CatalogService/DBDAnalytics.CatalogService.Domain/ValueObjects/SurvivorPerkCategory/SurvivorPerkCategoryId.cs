using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory
{
    public readonly record struct SurvivorPerkCategoryId
    {
        public int Value { get; }

        internal SurvivorPerkCategoryId(int value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static SurvivorPerkCategoryId From(int value)
        {
            GuardException.Against.That(value <= 0, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id для категории должно быть положительным числом.")));

            return new SurvivorPerkCategoryId(value);
        }

        public static implicit operator int(SurvivorPerkCategoryId survivorPerkCategoryId) => survivorPerkCategoryId.Value;
    }
}