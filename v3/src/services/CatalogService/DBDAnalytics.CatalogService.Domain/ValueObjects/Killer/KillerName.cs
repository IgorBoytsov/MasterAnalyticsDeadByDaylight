using DBDAnalytics.CatalogService.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Killer
{
    public sealed record KillerName
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; private set; } = null!;

        internal KillerName(string value) => Value = value;

        /// <exception cref="InvalidKillerPropertyException"></exception>
        public static KillerName Create(string killerName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(killerName), () => new InvalidKillerPropertyException("Имя убийцы не может быть пустым."));

            return new KillerName(killerName);
        }

        public override string ToString() => Value;

        public static implicit operator string(KillerName killerName) => killerName.Value;
    }
}