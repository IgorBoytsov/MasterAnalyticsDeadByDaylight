using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Role
{
    public readonly record struct RoleId
    {
        public int Value { get; }

        internal RoleId(int value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static RoleId Form(int value)
        {
            GuardException.Against.That(value <= 0, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id для роли должно быть положительным числом.")));

            return new RoleId(value);
        }

        public static implicit operator int(RoleId roleId) => roleId.Value; 
    }
}