using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.TypeDeath
{
    public sealed record TypeDeathName
    {
        public const int MAX_LENGTH = 50;
        public const int MIN_LENGTH = 1;

        public string Value { get; }

        internal TypeDeathName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static TypeDeathName Create(string typeDeathName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(typeDeathName), () => new NameException(new Error(ErrorCode.Validation, "Название типа смерти не может быть пустым.")));
            GuardException.Against.That(typeDeathName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для названия типа смерти от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new TypeDeathName(typeDeathName);
        }

        public override string ToString() => Value;

        public static implicit operator string(TypeDeathName typeDeathName) => typeDeathName.Value;
    }
}