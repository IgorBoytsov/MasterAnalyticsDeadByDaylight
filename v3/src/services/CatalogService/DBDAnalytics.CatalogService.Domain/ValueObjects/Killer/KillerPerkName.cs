using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Killer
{
    public sealed record KillerPerkName
    {
        public const int MAX_LENGTH = 200;

        public string Value { get; private set; } = null!;

        internal KillerPerkName(string value) => Value = value;

        /// <exception cref="ArgumentException"></exception>
        public static KillerPerkName Create(string perkName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(perkName), () => new ArgumentException("Название перка не может быть пустым.", nameof(perkName)));
            GuardException.Against.That(perkName.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Максимально допустимая длина для названия перка {MAX_LENGTH}", nameof(perkName)));

            return new KillerPerkName(perkName);
        }

        public override string ToString() => Value;

        public static implicit operator string(KillerPerkName perkName) => perkName.Value;
    }
}