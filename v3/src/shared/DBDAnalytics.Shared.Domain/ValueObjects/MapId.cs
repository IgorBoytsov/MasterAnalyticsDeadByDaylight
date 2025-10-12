using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct MapId
    {
        public Guid Value { get; }

        internal MapId(Guid value) => Value = value;

        public static MapId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new ArgumentException("Идентификатор карты не может быть пустым"));

            return new MapId(value);
        }

        public static MapId CreateNew() => new(Guid.NewGuid());

        public static implicit operator Guid(MapId matchId) => matchId.Value;
    }
}