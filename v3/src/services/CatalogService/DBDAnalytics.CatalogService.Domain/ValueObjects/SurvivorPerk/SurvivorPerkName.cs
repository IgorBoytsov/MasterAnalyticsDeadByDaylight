using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerk
{
    public sealed record SurvivorPerkName
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; }

        internal SurvivorPerkName(string value) => Value = value;

        public static SurvivorPerkName Create(string survivorPerkName)
        {
            GuardException.Against.NullOrWhiteSpace(survivorPerkName, nameof(survivorPerkName));
            GuardException.Against.That(survivorPerkName.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для названия перка {MAX_LENGTH} символов"));

            return new SurvivorPerkName(survivorPerkName);
        }

        public override string ToString() => Value;

        public static implicit operator string(SurvivorPerkName survivorPerkName) => survivorPerkName.Value; 
    }
}