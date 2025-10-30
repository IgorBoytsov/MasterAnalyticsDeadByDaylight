using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.MatchService.Domain.ValueObjects.SurvivorPerformance
{
    public readonly record struct SurvivorItemId
    {
        public Guid Value { get; }

        internal SurvivorItemId(Guid value) => Value = value;

        public static SurvivorItemId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new ArgumentException("Идентификатор матча не может быть пустым"));

            return new SurvivorItemId(value);
        }

        public static SurvivorItemId CreateNew() => new(Guid.NewGuid());

        public static implicit operator Guid(SurvivorItemId matchId) => matchId.Value;
    }
}