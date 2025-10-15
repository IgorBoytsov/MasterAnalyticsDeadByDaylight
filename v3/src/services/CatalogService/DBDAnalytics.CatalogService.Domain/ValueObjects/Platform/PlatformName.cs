using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Platform
{
    public sealed record PlatformName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 1;

        public string Value { get; }

        internal PlatformName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static PlatformName Create(string platformName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(platformName), () => new NameException(new Error(ErrorCode.Validation, "Название игровой платформы не может быть пустым.")));
            GuardException.Against.That(platformName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для игровой платформы от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new PlatformName(platformName);
        }

        public override string ToString() => Value;

        public static implicit operator string(PlatformName platformName) => platformName.Value;
    }
}