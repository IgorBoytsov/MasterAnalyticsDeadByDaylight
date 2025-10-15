using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.ItemAddon
{
    public sealed record ItemAddonName
    {
        public const int MAX_LENGTH = 200;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        internal ItemAddonName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static ItemAddonName Create(string itemAddonName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(itemAddonName), () => new NameException(new Error(ErrorCode.Validation, "Название улучшения не может быть пустым.")));
            GuardException.Against.That(itemAddonName.Length < 1 || itemAddonName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для названия улучшения от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new ItemAddonName(itemAddonName);
        }

        public override string ToString() => Value;

        public static implicit operator string(ItemAddonName itemAddonName) => itemAddonName.Value;
    }
}