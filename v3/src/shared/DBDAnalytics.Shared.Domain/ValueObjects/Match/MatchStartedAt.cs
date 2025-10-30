using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects.Match
{
    public readonly record struct MatchStartedAt
    {
        public static readonly DateTime MinValue = new(2016, 6, 14, 0, 0, 0, DateTimeKind.Utc);

        public DateTime Value { get; }

        internal MatchStartedAt(DateTime value) => Value = value;

        public static MatchStartedAt Create(DateTime value)
        {
            var utcDate = value.ToUniversalTime();

            GuardException.Against.That(utcDate > DateTime.UtcNow.AddMinutes(1), () => new ArgumentException("Дата начала матча не может быть в будущем."));
            GuardException.Against.That(utcDate < MinValue, () => new ArgumentException($"Дата начала матча не может быть раньше {MinValue:yyyy-MM-dd}."));

            return new MatchStartedAt(utcDate);
        }

        public override string ToString() => Value.ToString("o");

        public static implicit operator DateTime(MatchStartedAt startDate) => startDate.Value;
    }
}