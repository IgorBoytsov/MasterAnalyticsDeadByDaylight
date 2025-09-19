using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Measurement
{
    public sealed record MeasurementName
    {
        public const int MAX_LENGTH = 200;

        public string Value { get; private set; } = null!;

        internal MeasurementName(string value) => Value = value;

        /// <exception cref="ArgumentException"></exception>
        public static MeasurementName Create(string name)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(name), () => new ArgumentException("Название измерения не может быть пустым.", nameof(name)));
            GuardException.Against.That(name.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Максимально допустимая длина для названия измерения {MAX_LENGTH}", nameof(name)));

            return new MeasurementName(name);
        }

        public override string ToString() => Value;

        public static implicit operator string(MeasurementName name) => name.Value;
    }
}