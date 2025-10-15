using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Item
{
    public sealed record ItemName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        internal ItemName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static ItemName Create(string itemName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(itemName), () => new NameException(new Error(ErrorCode.Validation, "Название предмета не может быть пустым.")));
            GuardException.Against.That(itemName.Length < 1 || itemName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для название предмета от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new ItemName(itemName);
        }

        public override string ToString() => Value;

        public static implicit operator string(ItemName itemName) => itemName.Value;
    }
}