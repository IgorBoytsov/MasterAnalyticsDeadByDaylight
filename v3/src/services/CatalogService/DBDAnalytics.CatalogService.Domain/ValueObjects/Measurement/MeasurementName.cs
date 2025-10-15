using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Measurement
{
    public sealed record MeasurementName
    {
        public const int MAX_LENGTH = 200;
        public const int MIN_LENGTH = 1;

        public string Value { get; private set; } = null!;

        internal MeasurementName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static MeasurementName Create(string measurementName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(measurementName), () => new NameException(new Error(ErrorCode.Validation, "Название измерения не может быть пустым.")));
            GuardException.Against.That(measurementName.Length < 1 || measurementName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для названия измерения от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new MeasurementName(measurementName);
        }

        public override string ToString() => Value;

        public static implicit operator string(MeasurementName measurementName) => measurementName.Value;
    }
}