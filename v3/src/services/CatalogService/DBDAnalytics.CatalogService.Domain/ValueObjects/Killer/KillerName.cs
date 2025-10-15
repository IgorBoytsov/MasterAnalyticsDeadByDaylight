using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Killer
{
    public sealed record KillerName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 1;

        public string Value { get; private set; } = null!;

        internal KillerName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static KillerName Create(string killerName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(killerName), () => new NameException(new Error(ErrorCode.Validation, "Имя киллера не может быть пустым.")));
            GuardException.Against.That(killerName.Length < 1 || killerName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для названия киллера от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new KillerName(killerName);
        }

        public override string ToString() => Value;

        public static implicit operator string(KillerName killerName) => killerName.Value;
    }
}