using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.ItemAddon
{
    public sealed record ItemAddonName
    {
        public const int MAX_LENGTH = 200;

        public string Value { get; } = null!;

        internal ItemAddonName(string value) => Value = value;

        public static ItemAddonName Create(string itemAddonName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(itemAddonName), () => new InvalidOperationException("Название аддона для предмета не может быть пустым."));

            return new ItemAddonName(itemAddonName);
        }

        public override string ToString() => Value;

        public static implicit operator string(ItemAddonName itemAddonName) => itemAddonName.Value;
    }
}