using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects.Match
{
    public readonly record struct MatchDuration
    {
        public static readonly MatchDuration Zero = new(TimeSpan.Zero);

        public TimeSpan Value { get; }

        internal MatchDuration(TimeSpan value) => Value = value;

        public static MatchDuration Create(TimeSpan value)
        {
            GuardException.Against.That(value < TimeSpan.Zero, () => new ArgumentException("Длительность матча не может быть отрицательной"));
            GuardException.Against.That(value.TotalMinutes > 62, () => new ArgumentException("Длительность матча не может превышать часа. (Может быть погрешность на +2м)"));

            return new MatchDuration(value);
        }

        public static implicit operator TimeSpan(MatchDuration matchDuration) => matchDuration.Value;
    }
}