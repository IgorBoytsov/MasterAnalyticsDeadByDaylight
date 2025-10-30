using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct MatchId
    {
        public Guid Value { get; }

        internal MatchId(Guid value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static MatchId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id матча не должен быть пустым GUID.")));

            return new MatchId(value);
        }

        public static MatchId CreateNew() => new(Guid.NewGuid());

        public static implicit operator Guid(MatchId matchId) => matchId.Value;
    }
}
