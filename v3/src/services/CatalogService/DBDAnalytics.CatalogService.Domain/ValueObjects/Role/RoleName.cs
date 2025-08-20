using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Role
{
    public sealed record RoleName
    {
        public const int MAX_LENGTH = 50;

        public string Value { get; }

        internal RoleName(string value) => Value = value;

        public static RoleName Create(string roleName)
        {
            GuardException.Against.NullOrWhiteSpace(roleName, nameof(roleName));
            GuardException.Against.That(roleName.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для роли {MAX_LENGTH} символов"));

            return new RoleName(roleName);
        }

        public override string ToString() => Value;

        public static implicit operator string(RoleName roleName) => roleName.Value;
    }
}