using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Survivor
{
    public sealed record SurvivorName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 1;

        public string Value { get; }

        internal SurvivorName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static SurvivorName Create(string survivorName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(survivorName), () => new NameException(new Error(ErrorCode.Validation, "Имя выжившего не может быть пустым.")));
            GuardException.Against.That(survivorName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для имени выжившего от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new SurvivorName(survivorName);
        }

        public override string ToString() => Value;

        public static implicit operator string(SurvivorName survivorName) => survivorName.Value;
    }
}