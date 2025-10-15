using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Killer
{
    public sealed record KillerPerkName
    {
        public const int MAX_LENGTH = 200;
        public const int MIN_LENGTH = 1;

        public string Value { get; private set; } = null!;

        internal KillerPerkName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static KillerPerkName Create(string perkName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(perkName), () => new NameException(new Error(ErrorCode.Validation, "Название перка не может быть пустым")));
            GuardException.Against.That(perkName.Length < 1 || perkName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для название перка от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new KillerPerkName(perkName);
        }

        public override string ToString() => Value;

        public static implicit operator string(KillerPerkName perkName) => perkName.Value;
    }
}