using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.GameEvent
{
    public sealed record GameEventName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        internal GameEventName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static GameEventName Create(string value)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(value), () => new NameException(new Error(ErrorCode.Validation, "Название игрового ивента не может быть пустым.")));
            GuardException.Against.That(value.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для игрового ивента от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new GameEventName(value);
        }

        public override string ToString() => Value;

        public static implicit operator string(GameEventName value) => value.Value;
    }
}