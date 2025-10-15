using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Offering
{
    public sealed record OfferingName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 1;

        public string Value { get; private set; } = null!;

        internal OfferingName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static OfferingName Create(string offeringName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(offeringName), () => new NameException(new Error(ErrorCode.Validation, "Название предмета не может быть пустым.")));
            GuardException.Against.That(offeringName.Length < 1 || offeringName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для название подношения от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new OfferingName(offeringName);
        }

        public override string ToString() => Value;

        public static implicit operator string(OfferingName offeringName) => offeringName.Value;
    }
}