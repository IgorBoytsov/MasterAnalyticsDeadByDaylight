using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.TypeDeath
{
    public sealed record TypeDeathName
    {
        public const int MAX_LENGTH = 50;

        public string Value { get; }

        internal TypeDeathName(string value) => Value = value;
        
        public static TypeDeathName Create(string typeDeathName)
        {
            GuardException.Against.NullOrWhiteSpace(typeDeathName, nameof(typeDeathName));
            GuardException.Against.That(typeDeathName.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для типа смерти {MAX_LENGTH} символов"));

            return new TypeDeathName(typeDeathName);
        }

        public override string ToString() => Value;

        public static implicit operator string(TypeDeathName typeDeathName) => typeDeathName.Value;
    }
}