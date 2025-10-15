using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.PlayerAssociation
{
    public sealed record PlayerAssociationName
    {
        public const int MAX_LENGTH = 50;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        internal PlayerAssociationName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static PlayerAssociationName Create(string playerAssociationName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(playerAssociationName), () => new NameException(new Error(ErrorCode.Validation, "Название игровой ассоциации не может быть пустым.")));
            GuardException.Against.That(playerAssociationName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для игровой ассоциации от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new PlayerAssociationName(playerAssociationName);
        }

        public override string ToString() => Value;

        public static implicit operator string(PlayerAssociationName playerAssociationName) => playerAssociationName.Value;
    }
}