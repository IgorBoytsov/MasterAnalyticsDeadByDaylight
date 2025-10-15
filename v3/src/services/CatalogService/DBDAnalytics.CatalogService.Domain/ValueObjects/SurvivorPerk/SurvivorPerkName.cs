using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerk
{
    public sealed record SurvivorPerkName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 100;

        public string Value { get; }

        internal SurvivorPerkName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static SurvivorPerkName Create(string survivorPerkName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(survivorPerkName), () => new NameException(new Error(ErrorCode.Validation, "Название перка не может быть пустым.")));
            GuardException.Against.That(survivorPerkName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для названия перка от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new SurvivorPerkName(survivorPerkName);
        }

        public override string ToString() => Value;

        public static implicit operator string(SurvivorPerkName survivorPerkName) => survivorPerkName.Value; 
    }
}