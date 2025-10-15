using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.KillerPerkCategory
{
    public readonly record struct KillerPerkCategoryId
    {
        public int Value { get; }

        internal KillerPerkCategoryId(int value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static KillerPerkCategoryId From(int value)
        {
            GuardException.Against.That(value <= 0, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id категории перка должен быть положительным числом.")));

            return new KillerPerkCategoryId(value);
        }

        public static implicit operator int(KillerPerkCategoryId killerPerkCategoryId) => killerPerkCategoryId.Value;
    }
}