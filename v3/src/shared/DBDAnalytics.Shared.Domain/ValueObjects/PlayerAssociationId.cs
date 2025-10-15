using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct PlayerAssociationId
    {
        public int Value { get; }

        internal PlayerAssociationId(int value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static PlayerAssociationId Form(int playerAssociationId)
        {
            GuardException.Against.That(playerAssociationId <= 0, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id игровой ассоциации должен быть положительным числом.")));

            return new PlayerAssociationId(playerAssociationId);
        }

        public static implicit operator int(PlayerAssociationId playerAssociationId) => playerAssociationId.Value;
    }
}