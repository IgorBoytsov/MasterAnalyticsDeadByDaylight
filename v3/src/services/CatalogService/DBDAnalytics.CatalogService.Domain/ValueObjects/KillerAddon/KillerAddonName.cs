using DBDAnalytics.CatalogService.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.KillerAddon
{
    public sealed record KillerAddonName
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; private set; } = null!;

        internal KillerAddonName(string value) => Value = value;

        /// <exception cref="InvalidKillerPropertyException"></exception>
        public static KillerAddonName Create(string killerName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(killerName), () => new InvalidKillerPropertyException("Название улучшения убийцы не может быть пустым."));

            return new KillerAddonName(killerName);
        }

        public override string ToString() => Value;

        public static implicit operator string(KillerAddonName killerName) => killerName.Value;
    }
}