using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.MatchService.Domain.ValueObjects.KillerPerformance
{
    public readonly record struct KillerPerformanceId
    {
        public Guid Value { get; }

        internal KillerPerformanceId(Guid value) => Value = value;

        public static KillerPerformanceId From(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new ArgumentException("Идентификатор участника матча в виде киллера не может быть пустым"));

            return new KillerPerformanceId(value);
        }

        public static KillerPerformanceId CreateNew() => new(Guid.NewGuid());

        public static implicit operator Guid(KillerPerformanceId killerPerformanceId) => killerPerformanceId.Value;
    }
}