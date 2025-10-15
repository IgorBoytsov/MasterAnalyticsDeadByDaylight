using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct SurvivorId
    {
        public Guid Value { get; }

        internal SurvivorId(Guid value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static SurvivorId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id выжившего не должен быть пустым GUID.")));

            return new SurvivorId(value);
        }

        public static implicit operator Guid(SurvivorId survivorId) => survivorId.Value;
    }
}