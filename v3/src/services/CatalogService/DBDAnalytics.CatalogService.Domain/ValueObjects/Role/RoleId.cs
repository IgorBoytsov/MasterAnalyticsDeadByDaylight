using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Role
{
    public readonly record struct RoleId
    {
        public int Value { get; }

        internal RoleId(int value) => Value = value;

        public static RoleId Form(int value)
        {
            GuardException.Against.That(value <= 0, () => new ArgumentException("Id для роли должно быть положительным числом."));

            return new RoleId(value);
        }

        public static implicit operator int(RoleId roleId) => roleId.Value; 
    }
}