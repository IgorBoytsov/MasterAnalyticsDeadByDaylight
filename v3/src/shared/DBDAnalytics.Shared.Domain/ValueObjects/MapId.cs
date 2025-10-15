using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct MapId
    {
        public Guid Value { get; }

        internal MapId(Guid value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static MapId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id киллера не должен быть пустым GUID.")));

            return new MapId(value);
        }

        public static MapId CreateNew() => new(Guid.NewGuid());

        public static implicit operator Guid(MapId matchId) => matchId.Value;
    }
}