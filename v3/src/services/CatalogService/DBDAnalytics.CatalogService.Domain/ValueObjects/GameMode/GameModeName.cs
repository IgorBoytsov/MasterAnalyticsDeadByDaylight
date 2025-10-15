using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.GameMode
{
    public sealed record GameModeName
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; } = null!;

        internal GameModeName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static GameModeName Create(string value)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(value), () => new NameException(new Error(ErrorCode.Validation, "Название игрового режима не может быть пустым.")));
            GuardException.Against.That(value.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для игрового режима {MAX_LENGTH} символов")));

            return new GameModeName(value);
        }

        public override string ToString() => Value;

        public static implicit operator string(GameModeName value) => value.Value;
    }
}