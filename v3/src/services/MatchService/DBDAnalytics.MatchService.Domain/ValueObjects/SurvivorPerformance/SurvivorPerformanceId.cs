using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.MatchService.Domain.ValueObjects.SurvivorPerformance
{
    public readonly record struct SurvivorPerformanceId
    {
        public Guid Value { get; }

        internal SurvivorPerformanceId(Guid value) => Value = value;

        public static SurvivorPerformanceId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new ArgumentException("Идентификатор участника матча в виде выжившего не может быть пустым"));

            return new SurvivorPerformanceId(value);
        }

        public static SurvivorPerformanceId CreateNew() => new(Guid.NewGuid());

        public static implicit operator Guid(SurvivorPerformanceId survivorPerformanceId) => survivorPerformanceId.Value;
    }
}