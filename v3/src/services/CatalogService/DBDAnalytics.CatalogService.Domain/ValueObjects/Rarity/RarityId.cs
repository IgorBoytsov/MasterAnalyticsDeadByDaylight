using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity
{
    public readonly record struct RarityId
    {
        public int Value { get; }

        internal RarityId(int rarityId) => Value = rarityId;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static RarityId From(int value)
        {
            GuardException.Against.That(value <= 0, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id для редкости должно быть положительным числом.")));

            return new RarityId(value);
        }

        public static implicit operator int(RarityId rarityId) => rarityId.Value;
    }
}