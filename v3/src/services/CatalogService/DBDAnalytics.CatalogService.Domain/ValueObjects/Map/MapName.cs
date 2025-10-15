using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Map
{
    public sealed record MapName
    {
        public const int MAX_LENGTH = 200;
        public const int MIN_LENGTH = 1;

        public string Value { get; private set; } = null!;

        internal MapName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static MapName Create(string mapName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(mapName), () => new NameException(new Error(ErrorCode.Validation, "Название карты не может быть пустым.")));
            GuardException.Against.That(mapName.Length < 1 || mapName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для названия карты от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new MapName(mapName);
        }

        public override string ToString() => Value;

        public static implicit operator string(MapName name) => name.Value;
    }
}