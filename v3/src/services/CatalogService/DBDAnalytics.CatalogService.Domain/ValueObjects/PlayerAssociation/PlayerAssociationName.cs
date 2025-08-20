using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.PlayerAssociation
{
    public sealed record PlayerAssociationName
    {
        public const int MAX_LENGTH = 50;

        public string Value { get; } = null!;

        internal PlayerAssociationName(string value) => Value = value;

        public static PlayerAssociationName Create(string playerAssociationName)
        {
            GuardException.Against.NullOrWhiteSpace(playerAssociationName, nameof(playerAssociationName));
            GuardException.Against.That(playerAssociationName.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для игровой ассоциации {MAX_LENGTH} символов"));

            return new PlayerAssociationName(playerAssociationName);
        }

        public override string ToString() => Value;

        public static implicit operator string(PlayerAssociationName playerAssociationName) => playerAssociationName.Value;
    }
}