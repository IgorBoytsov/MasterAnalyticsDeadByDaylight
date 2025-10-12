using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct UserId
    {
        public Guid Value { get; }

        internal UserId(Guid value) => Value = value;

        public static UserId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new ArgumentException("Идентификатор пользователя не может быть пустым"));

            return new UserId(value);
        }

        public static implicit operator Guid(UserId userId) => userId.Value;
    }
}