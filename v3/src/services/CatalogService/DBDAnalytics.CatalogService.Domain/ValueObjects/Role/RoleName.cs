using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Role
{
    public sealed record RoleName
    {
        public const int MAX_LENGTH = 50;
        public const int MIN_LENGTH = 1;

        public string Value { get; }

        internal RoleName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static RoleName Create(string roleName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(roleName), () => new NameException(new Error(ErrorCode.Validation, "Название роли не может быть пустым.")));
            GuardException.Against.That(roleName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для роли от {MIN_LENGTH} до {MAX_LENGTH} символов")));

            return new RoleName(roleName);
        }

        public override string ToString() => Value;

        public static implicit operator string(RoleName roleName) => roleName.Value;
    }
}