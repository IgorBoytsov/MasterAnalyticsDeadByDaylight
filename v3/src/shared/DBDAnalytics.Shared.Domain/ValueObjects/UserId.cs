using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct UserId
    {
        public Guid Value { get; }

        internal UserId(Guid value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static UserId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id пользователя не должен быть пустым GUID.")));

            return new UserId(value);
        }

        public static implicit operator Guid(UserId userId) => userId.Value;
    }
}