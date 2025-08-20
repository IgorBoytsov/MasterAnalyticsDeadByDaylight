using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.PlayerAssociation
{
    public readonly struct PlayerAssociationId
    {
        public int Value { get; }

        internal PlayerAssociationId(int value) => Value = value;

        public static PlayerAssociationId Form(int playerAssociationId)
        {
            GuardException.Against.That(playerAssociationId <= 0, () => new ArgumentException("Id для игровой ассоциации должно быть положительным числом."));

            return new PlayerAssociationId(playerAssociationId);
        }

        public static implicit operator int(PlayerAssociationId playerAssociationId) => playerAssociationId.Value;
    }
}