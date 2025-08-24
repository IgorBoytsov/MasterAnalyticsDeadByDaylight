using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Item
{
    public sealed record ItemName
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; } = null!;

        internal ItemName(string value) => Value = value;

        public static ItemName Create(string itemName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(itemName), () => new InvalidOperationException("Имя предмета не может быть пустым."));

            return new ItemName(itemName);
        }

        public override string ToString() => Value;

        public static implicit operator string(ItemName itemName) => itemName.Value;
    }
}