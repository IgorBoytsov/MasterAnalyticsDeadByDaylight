using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.WhoPlacedMap
{
    public sealed record PlacedMapName
    {
        public const int MAX_LENGTH = 50;
        public const int MIN_LENGTH = 1;

        public string Value { get; private set; } = null!;

        internal PlacedMapName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static PlacedMapName Create(string name)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(name), () => new NameException(new Error(ErrorCode.Validation, "Информация о том кто поставил карту не может быть пустым.")));
            GuardException.Against.That(name.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для информация о том кто поставил карту от {MIN_LENGTH} до {MAX_LENGTH} символов")));


            return new PlacedMapName(name);
        }

        public override string ToString() => Value;

        public static implicit operator string(PlacedMapName name) => name.Value;
    }
}