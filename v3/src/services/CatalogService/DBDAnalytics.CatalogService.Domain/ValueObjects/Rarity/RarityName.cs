using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity
{
    public sealed record RarityName
    {
        public const int MAX_LENGTH = 50;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        internal RarityName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static RarityName Create(string rarityName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(rarityName), () => new NameException(new Error(ErrorCode.Validation, "Название категории не может быть пустым.")));
            GuardException.Against.That(rarityName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для категории от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new RarityName(rarityName);
        }

        public override string ToString() => Value;

        public static implicit operator string(RarityName rarityName) => rarityName.Value;
    }
}