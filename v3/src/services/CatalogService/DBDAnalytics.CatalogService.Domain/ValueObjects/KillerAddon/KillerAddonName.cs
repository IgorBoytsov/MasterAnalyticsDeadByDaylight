using DBDAnalytics.CatalogService.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.KillerAddon
{
    public sealed record KillerAddonName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 1;

        public string Value { get; private set; } = null!;

        internal KillerAddonName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static KillerAddonName Create(string killerName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(killerName), () => new NameException(new Error(ErrorCode.Validation, "Название улучшения убийцы не может быть пустым.")));
            GuardException.Against.That(killerName.Length < 1 || killerName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для название улучшения от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            GuardException.Against.That(string.IsNullOrWhiteSpace(killerName), () => new InvalidKillerPropertyException(""));

            return new KillerAddonName(killerName);
        }

        public override string ToString() => Value;

        public static implicit operator string(KillerAddonName killerName) => killerName.Value;
    }
}